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
        /// Cookie��֤��������
        /// </summary>
        public const string CookieScheme = "MyCookieSchemeName";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddAuthentication(CookieScheme)//ָ��Ĭ�ϵ���֤�������ƣ�����ָ��ΪCookie����������
            services.AddAuthentication(option => { option.DefaultScheme = CookieScheme; })
                    .AddCookie(CookieScheme, options =>
                    {
                        options.AccessDeniedPath = "/account/denied";
                        options.LoginPath = "/account/login";
                    });
        }

        /// <summary>
        /// ���ùܵ���ע���м����
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

            //�����֤�����м����AuthenticationMiddleware�������м������ݵ�ǰ����֤��������ͼ��ȡ������Я�����û���Ϣ������ɹ���ΪHttpContext.User��ֵ
            app.UseAuthentication();
            //�����Ȩ�����м����AuthorizationMiddleware�������м������ݲ��Խ�����֤��������֤�Ƿ�ͨ�����
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
