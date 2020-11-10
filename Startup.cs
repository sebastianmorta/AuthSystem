using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices
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
            //services.AddDbContext<DeviceDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("AuthDbContextConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DeviceDbContext>()
            //    .AddDefaultTokenProviders();


            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<DeviceSetDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("PartsConnection")));


            services.AddMvc();
 

            services.AddControllersWithViews();
            services.AddRazorPages();
            //services.AddDbContext<DeviceDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("AuthDbContextConnection")));
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //});




            services.AddDbContext<DeviceDbContext>(options =>

            options.UseSqlServer(Configuration.GetConnectionString("AuthDbContextConnection"))
            );

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
