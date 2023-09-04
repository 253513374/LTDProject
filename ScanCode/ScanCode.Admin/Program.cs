using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MudBlazor.Services;
using Quartz;
using Radzen;
using ScanCode.Model.Entity;
using ScanCode.RedisCache;
using ScanCode.Repository;
using ScanCode.Repository.MediatRHandler.Events;
using ScanCode.Web.Admin.Authenticated;
using ScanCode.Web.Admin.Authenticated.IdentityModel;
using ScanCode.Web.Admin.Authenticated.Services;
using ScanCode.Web.Admin.Data;
using ScanCode.Web.Admin.Quartzs;
using ScanCode.Web.Admin.SignalRHub;
using Serilog;
using System.Reflection;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()//new JsonFormatter()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .MinimumLevel.Debug()
        .ReadFrom.Configuration(ctx.Configuration));

    //builder.Configuration.GetConnectionString("LotteryDbConnection")
    var connectionString = builder.Configuration.GetConnectionString("LotteryDbConnection");
    builder.Services.AddLotteryDbContext(connectionString);
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    //注入认证授权数据库IdentityDbContext
    var identitydbcontext = builder.Configuration.GetConnectionString("IdentityDbContext");
    builder.Services.AddDbContextFactory<CustomIdentityDbContext>(options =>
        options.UseSqlServer(identitydbcontext));
    //注入Identity 认证授权，启用CustomIdentityDbContext数据库
    builder.Services.AddIdentity<WtdlUser, WtdlRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = null;
    }).AddEntityFrameworkStores<CustomIdentityDbContext>();

    //  builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    // Add services to the container.
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

    //builder.Services.AddQuartz(q =>
    //{
    //    q.UseMicrosoftDependencyInjectionJobFactory();
    //    q.AddJob<LoadStockOutCacheJob>(j => j.WithIdentity("MyJob"));
    //    q.AddTrigger(t => t
    //        .WithIdentity("MyJobTrigger")
    //        .ForJob("MyJob")
    //        .WithCronSchedule("0 * * ? * *")); // 没年1月1日
    //});

    // builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    var redisconnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");

#if DEBUG
    redisconnectionString = builder.Configuration.GetConnectionString("DebugRedisConnectionString");
#endif
    builder.Services.AddRedisCache(redisconnectionString);
    // builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisconnectionString));
    //builder.Services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
    //builder.Services.AddScoped<IRedisCache, RedisCacheService>();

    var oracleconnectionString = builder.Configuration.GetConnectionString("OracleDbConnection");

    builder.Services.AddOracleContext(oracleconnectionString);

    builder.Services.AddSingleton<ExportService>();
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });
    builder.Services.AddControllers();
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<WeatherForecastService>();
    builder.Services.AddMediatR(op =>
    {
        op.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });
    builder.Services.AddSignalR(options =>
    {
        options.MaximumReceiveMessageSize = 512 * 1024; // 设置最大发送信息容量0.5 MB （512KB）
    });
    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });

    builder.Services.AddScoped<Radzen.DialogService>();
    builder.Services.AddScoped<NotificationService>();
    builder.Services.AddScoped<TooltipService>();
    builder.Services.AddScoped<ContextMenuService>();

    builder.Services.AddMudServices(config =>
    {
        // config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

        //config.SnackbarConfiguration.PreventDuplicates = false;
        //config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 10000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
    });

    builder.Services.AddScoped<AccountService>();
    builder.Services.AddScoped<RoleClaimService>();
    builder.Services.AddScoped<CustomAuthenticationService>();
    builder.Services.AddScoped<CustomAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

    // 添加身份验证和授权服务
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
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

    builder.Services.AddAuthorization(options =>
    {
        // options.a.AuthorizePage("/CustomUnauthorized");
        RegisterPermissionClaims(options);
        //options.AddPolicy("Admin", policy =>
        //    policy.RequireRole("admin"));
    });

    ////Senparc.Weixin 注册（必须）
    // builder.Services.AddSenparcWeixinServices(builder.Configuration);

    var app = builder.Build();

    //#region 启用微信配置

    //var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

    ////启用微信配置（必须）
    //var registerService = app.UseSenparcWeixin(app.Environment,
    //    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
    //    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
    //    register => { /* CO2NET 全局配置 */ },
    //    (register, weixinSetting) =>
    //    {
    //        //注册公众号信息（可以执行多次，注册多个公众号）
    //        register.RegisterMpAccount(weixinSetting, "【威特电缆】公众号");
    //        register.RegisterTenpayV3(weixinSetting, "【威特电缆】微信支付（V2）");
    //    });
    //#endregion 启用微信配置

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "微信H5后台API");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "抽奖系统 API 接口");
    });

    app.UseResponseCompression();
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseWebSockets();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBlazorHub();
    app.MapControllers();
    app.MapHub<APPHub>("/apphub");
    app.MapFallbackToPage("/_Host");
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

static void RegisterPermissionClaims(AuthorizationOptions options)
{
    foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
    {
        var propertyValue = prop.GetValue(null);

#if DEBUG
        Console.WriteLine($"{prop.Name}:{propertyValue}");
#endif

        if (propertyValue is not null)
        {
            options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(CustomClaimTypes.Permission, propertyValue.ToString()));
        }
    }
}