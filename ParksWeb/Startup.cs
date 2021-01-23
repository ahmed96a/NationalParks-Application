using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParksWeb.Repository;
using ParksWeb.Repository.IRepository;

namespace ParksWeb
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
            services.AddControllersWithViews();

            // 13. Part 7
            // -----------------------

            // we will use Cookie authentication, to be the default authentication scheme for our frontend mvc project.
            // since we did not use the authentication by identity framework, then we need to implement few properties at AddCookies ()
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                // There are many options that you can configure with cookies but just implement few of the basic ones.
                .AddCookie(options =>
                 {
                     options.Cookie.HttpOnly = true;
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                     options.LoginPath = "/Account/Login";
                     options.AccessDeniedPath = "/Home/AccessDenied";
                     options.SlidingExpiration = true;
                 });

            services.AddHttpContextAccessor(); // that is not required for adding CookieAuthenticationDefaults scheme, thus idon't know what it use 

            // -----------------------

            // 8. Part 8
            // ----------------------------

            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            // ----------------------------

            services.AddScoped<IAccountRepository, AccountRepository>(); // 13. Part 1

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // 13. Part 5

            // 8. Part 3
            // ----------------------------

            // Add HttpClient services, to use it in making the Http Calls to the API.
            services.AddHttpClient();

            // ----------------------------

            // 13. Part 3
            // ----------------------------

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-3.1#cookies

            services.AddSession(configure =>
            {
                configure.IdleTimeout = TimeSpan.FromMinutes(10);
                configure.Cookie.HttpOnly = true;
                configure.Cookie.IsEssential = true;
            });

            // ----------------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            // 13. Part 3
            // ----------------------------
            // we comment them to know if they will effect our project, but maybe in some scenarioes we will need them

            /*
            app.UseCors(configurePolicy => {

                configurePolicy.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
            });
            */

            app.UseSession();

            // ----------------------------

            app.UseAuthentication(); // // 13. Part 7

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
