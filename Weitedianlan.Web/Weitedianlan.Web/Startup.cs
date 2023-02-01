using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Weitedianlan.Service;
using Weitedianlan.Web.Application;

namespace Weitedianlan.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<UserService>();
            services.AddTransient<AgentService>();
            services.AddTransient<WLabelService>();
            services.AddTransient<Db>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//用于获取请求上下文
            services.AddTransient<IUserService, UserService>();
            //services.AddMvc();  net core 2.2

            services.AddControllers();//net 7
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            ConfigApp.SetAppSetting(Configuration.GetSection("SqlContext"));
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            //   app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                // Endpoint.
                endpoint.MapControllers();
                endpoint.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            ServiceLocator.Instance = app.ApplicationServices;
        }
    }
}