using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp
{
    public class Startup
    {
        /// <summary>
        /// Cookie认证方案名称
        /// </summary>
        public const string CookieScheme = "MyCookieSchemeName";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddAuthentication(CookieScheme)//指定默认的认证方案名称，这里指定为Cookie方案的名称
            services.AddAuthentication(option => { option.DefaultScheme = CookieScheme; })
                    .AddCookie(CookieScheme, options =>
                    {
                        options.AccessDeniedPath = "/account/denied";
                        options.LoginPath = "/account/login";
                    });
        }

        /// <summary>
        /// 配置管道（注册中间件）
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //添加认证服务中间件（AuthenticationMiddleware），该中间件会根据当前的认证方案，试图获取请求中携带的用户信息，如果成功则为HttpContext.User赋值
            app.UseAuthentication();
            //添加授权服务中间件（AuthorizationMiddleware），该中间件会根据策略进行验证，返回验证是否通过结果
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
