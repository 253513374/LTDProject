using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScanCode.WPF.HubServer;
using ScanCode.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ScanCode.WPF.HubServer.Services;
using AutoMapper;
using ScanCode.Share;
using ScanCode.WPF.Model;
using ScanCode.WPF.View;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.RegularExpressions;

namespace ScanCode.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public App()
        {
            //Services = ConfigureServices();
            this.InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg.ConstructServicesUsing(ObjectFactory);

            //    cfg.CreateMap<Source, Destination>();
            //});
            //IMapper mapper = config.CreateMapper();

            base.OnStartup(e);
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var hub = Configuration.GetSection("HubUrl").Value;
            var login = Configuration.GetSection("LoginUrl").Value;

            //注册View窗口与iewModels

            //主窗体注册使用AddSingleton 声明周期为单例，每次请求都会使用同一个实例
            services.AddSingleton<HomeWindow>();
            services.AddSingleton<HomeViewModel>();

            //子窗体注册使用AddTransient 声明周期为短暂，每次请求都会创建一个新的实例
            services.AddTransient<ScanCodeOutWindow>();
            services.AddTransient<ScanCodeOutViewModel>();

            services.AddTransient<SplashScreenLoginWindow>();
            services.AddTransient<LoginViewModel>();

            services.AddTransient<ScanCodeReturnWindow>();
            services.AddTransient<ScanCodeReturnViewModel>();

            services.AddTransient<DialogWindow>();
            services.AddTransient<DialogViewModel>();
            // 注册服务

            services.AddSingleton<OutOrderService>();
            services.AddSingleton<HubClientService>(sp => new HubClientService(hub, login));
            //services.AddSingleton<IFilesService, FilesService>();
            //services.AddSingleton<ISettingsService, SettingsService>();
            //services.AddSingleton<IClipboardService, ClipboardService>();
            //services.AddSingleton<IShareService, ShareService>();
            //services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ObjectFileStorage>();
            //services.Add();

            services.AddAutoMapper(typeof(OrganizationProfile));

            return services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            return Current.ServiceProvider.GetRequiredService<T>();

            //return Services.GetRequiredService<T>();
        }

        public static string ReplaceScanCode(string v)
        {
            string pattern = @"^[\w\W]*?(\b\d+\b)$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);//正则表达式
            Match match = regex.Match(v);
            if (string.IsNullOrEmpty(match.Groups[1].Value)) return "";
            return match.Groups[1].Value;
        }
    }
}