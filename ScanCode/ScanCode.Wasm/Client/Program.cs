using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

namespace ScanCode.Web.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient
            //{
            //    BaseAddress = new Uri("https://www.rewt.cn/")
            //});

            builder.Services.AddSingleton<DataStateContainer>();

            builder.Services.AddHttpClient("local", client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("weixin", client =>
                client.BaseAddress = new Uri("https://www.rewt.cn/"));

            builder.Services.AddMudServices();
            builder.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
            });

            await builder.Build().RunAsync();
        }
    }
}