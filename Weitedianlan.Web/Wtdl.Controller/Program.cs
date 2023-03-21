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

    // ��������֤����
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
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "�齱ϵͳ API �ӿ�", Version = "v1" });

        var xmlFiles = new[] { $"{Assembly.GetExecutingAssembly().GetName().Name}.xml", $"{typeof(LotteryActivity).Assembly.GetName().Name}.xml" };
        foreach (var xmlFile in xmlFiles)
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        }
        // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        // c.IncludeXmlComments(xmlPath);

        //ʹ�� AddSecurityDefinition �������һ����Ϊ "Bearer" �İ�ȫ���壬
        //������Ϊ JWT ��Ȩ��ͷʹ�� Bearer ������
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "ʹ�� Bearer ������ JWT ��Ȩ��ͷ������,ע��ո�: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        //ʹ�� AddSecurityRequirement ������Ӱ�ȫҪ�󣬲�����ǰ�涨��� "Bearer" ��ȫ����
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
    // ��ӻ������
    //builder.Services.AddDistributedRedisCache(options =>
    //{
    //    options.Configuration = "redis-server:6379";
    //    options.InstanceName = "SampleInstance";
    //});

    builder.Services.AddResponseCaching();
    builder.Services.AddMemoryCache();

    ////Senparc.Weixin ע�ᣨ���룩
    builder.Services.AddSenparcWeixinServices(builder.Configuration);

    var connectionString = builder.Configuration.GetConnectionString("LotteryDbConnection");
    builder.Services.AddLotteryDbContext(connectionString);

    builder.Services.AddSingleton<HubService>();
    builder.Services.AddScoped<UserItemsService>();
    builder.Services.AddScoped<LotteryService>();//ע��齱����
    builder.Services.AddScoped<SearchByCodeService>();//ע���α��Դ����
    builder.Services.AddScoped<ScanByRedPacketService>();//ע��������
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
            builder.WithOrigins(new string[] { "http://www.chn315.top", "http://localhost:5276" })
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    var app = builder.Build();

    #region ����΢������

    var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

    //����΢�����ã����룩
    var registerService = app.UseSenparcWeixin(app.Environment,
        null /* ��Ϊ null �򸲸� appsettings  �е� SenpacSetting ����*/,
        null /* ��Ϊ null �򸲸� appsettings  �е� SenpacWeixinSetting ����*/,
        register => { /* CO2NET ȫ������ */ },
        (register, weixinSetting) =>
        {
            //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע�������ںţ�
            register.RegisterMpAccount(weixinSetting, "�����ص��¡����ں�");
            register.RegisterTenpayV3(weixinSetting, "�����ص��¡�΢��֧����V2��");
        });

    #endregion ����΢������

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
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "΢��H5��̨API");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "�齱ϵͳ API �ӿ�");
    });

    app.UseCors();
    // ���� Redis �м��

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