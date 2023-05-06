using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ScanCode.WPF.ViewModels;

namespace ScanCode.WPF
{
    public static class IocContainer
    {
        private static ServiceProvider _serviceProvider;

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // 注册ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();

            // 注册其他服务（如有需要）

            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}