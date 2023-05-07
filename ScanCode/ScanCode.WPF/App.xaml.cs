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

            // 注册ViewModels

            //services.AddTransient<MainWindow>();
            services.AddTransient<HomeWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<WtdlSqlService>(sp => new WtdlSqlService(hub, login));
            //services.AddSingleton<IFilesService, FilesService>();
            //services.AddSingleton<ISettingsService, SettingsService>();
            //services.AddSingleton<IClipboardService, ClipboardService>();
            //services.AddSingleton<IShareService, ShareService>();
            //services.AddSingleton<IEmailService, EmailService>();

            //services.Add();

            services.AddAutoMapper(typeof(OrganizationProfile));

            return services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            return Current.ServiceProvider.GetRequiredService<T>();

            //return Services.GetRequiredService<T>();
        }
    }
}