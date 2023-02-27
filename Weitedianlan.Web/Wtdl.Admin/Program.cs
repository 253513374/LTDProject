using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using System.Text;
using Wtdl.Admin.Data;
using Wtdl.Repository;
using MediatR;
using MudBlazor;
using Quartz;
using Wtdl.Repository.MediatRHandler.Events;
using StackExchange.Redis;
using Wtdl.Admin.BackgroundTask;
using Microsoft.EntityFrameworkCore;
using Wtdl.Admin.Authenticated;
using Wtdl.Admin.Authenticated.IdentityModel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
using Radzen;
using Wtdl.Admin.Authenticated.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Wtdl.Admin.SignalRHub;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()//new JsonFormatter()

    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(); // <-- Add this line

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

    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();

        q.AddJob<LoadStockOutCacheJob>(j => j.WithIdentity("MyJob"));
        q.AddTrigger(t => t
            .WithIdentity("MyJobTrigger")
            .ForJob("MyJob")
            .WithCronSchedule("0 0 0 1 1 ?")); // 没年1月1日
    });

    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    var redisconnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisconnectionString));
    //builder.Services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddControllers();
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<WeatherForecastService>();
    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    builder.Services.AddSignalR();
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
    // builder.Services.AddHttpContextAccessor();
    //builder.Services.AddAuthorizationCore();
    //builder.Services.AddAuthentication(options =>
    //{
    //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //});
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

    builder.Services.AddAuthorization(options =>
    {
        // options.a.AuthorizePage("/CustomUnauthorized");
        RegisterPermissionClaims(options);
        //options.AddPolicy("Admin", policy =>
        //    policy.RequireRole("admin"));
    });

    var app = builder.Build();

    app.UseResponseCompression();
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBlazorHub();
    app.MapControllers();
    app.MapHub<APPHub>("/apphub");
    app.MapFallbackToPage("/_Host");

    //  endpoints.MapHub<AuthHub>("/authhub");

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
        Console.WriteLine(propertyValue.ToString());
        if (propertyValue is not null)
        {
            options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(CustomClaimTypes.Permission, propertyValue.ToString()));
        }
    }
}