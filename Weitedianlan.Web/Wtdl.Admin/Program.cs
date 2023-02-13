using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using Wtdl.Admin.Data;
using Wtdl.Repository;
using MediatR;
using MudBlazor;
using Wtdl.Repository.MediatRHandler.Events;

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

    // Add services to the container.
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddSingleton<WeatherForecastService>();
    builder.Services.AddMudServices(config =>
    {
        // config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

        //config.SnackbarConfiguration.PreventDuplicates = false;
        //config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 10000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    });

    //builder.Services.AddSingleton<IWebHostEnvironment, we>();

    var app = builder.Build();

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

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

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