using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Reflection;
using System.Text;

using Wtdl.Controller.Services;
using Wtdl.Model.Entity;
using Wtdl.Mvc.Services;
using Wtdl.Repository;
using Wtdl.Repository.MediatRHandler.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);

    // builder.Host.UseSerilog();
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    // 添加身份验证服务
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
            };
        });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "抽奖系统 API 接口", Version = "v1" });

        var xmlFiles = new[] { $"{Assembly.GetExecutingAssembly().GetName().Name}.xml", $"{typeof(LotteryActivity).Assembly.GetName().Name}.xml" };
        foreach (var xmlFile in xmlFiles)
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        }
        // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        // c.IncludeXmlComments(xmlPath);

        //使用 AddSecurityDefinition 方法添加一个名为 "Bearer" 的安全定义，
        //并定义为 JWT 授权标头使用 Bearer 方案。
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "使用 Bearer 方案的 JWT 授权标头。例子,注意空格: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        //使用 AddSecurityRequirement 方法添加安全要求，并引用前面定义的 "Bearer" 安全定义
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    });

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddControllers();
    builder.Services.AddSignalR();

    builder.Services.AddHttpClient();

    //var hubUrl = builder.Configuration.GetValue<string>("SignalR:HubUrl");
    //builder.Services.AddSingleton(provider => new HubConnectionBuilder()
    //    .WithUrl(hubUrl, options =>
    //    {
    //        options.AccessTokenProvider = async () =>
    //        {
    //            return await GetSignalRAppToken();

    //            static async Task<string?> GetSignalRAppToken()
    //            {
    //               // throw new NotImplementedException();
    //               return "";
    //            }
    //        };
    //    })
    //    .WithAutomaticReconnect()
    //    .Build());

    //var redisconnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");
    //builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisconnectionString));
    //builder.Services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

    //builder.Services.AddStackExchangeRedisCache(options =>
    //{
    //    options.Configuration = redisconnectionString;
    //    // options.InstanceName = "MyRedisCache";
    //});
    // 添加缓存服务
    //builder.Services.AddDistributedRedisCache(options =>
    //{
    //    options.Configuration = "redis-server:6379";
    //    options.InstanceName = "SampleInstance";
    //});

    builder.Services.AddResponseCaching();
    builder.Services.AddMemoryCache();

    ////Senparc.Weixin 注册（必须）
    builder.Services.AddSenparcWeixinServices(builder.Configuration);

    var connectionString = builder.Configuration.GetConnectionString("LotteryDbConnection");
    builder.Services.AddLotteryDbContext(connectionString);

    builder.Services.AddSingleton<HubService>();
    builder.Services.AddScoped<UserItemsService>();
    builder.Services.AddScoped<LotteryService>();//注入抽奖服务
    builder.Services.AddScoped<SearchByCodeService>();//注入防伪溯源服务
    builder.Services.AddScoped<ScanByRedPacketService>();//注入红包服务
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
            builder.WithOrigins(new string[] { "http://www.chn315.top", "http://localhost:5276" })
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    var app = builder.Build();

    #region 启用微信配置

    var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

    //启用微信配置（必须）
    var registerService = app.UseSenparcWeixin(app.Environment,
        null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
        null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
        register => { /* CO2NET 全局配置 */ },
        (register, weixinSetting) =>
        {
            //注册公众号信息（可以执行多次，注册多个公众号）
            register.RegisterMpAccount(weixinSetting, "【威特电缆】公众号");
            register.RegisterTenpayV3(weixinSetting, "【威特电缆】微信支付（V2）");
        });

    #endregion 启用微信配置

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "微信H5后台API");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "抽奖系统 API 接口");
    });

    app.UseCors();
    // 配置 Redis 中间件

    app.UseResponseCaching();

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}