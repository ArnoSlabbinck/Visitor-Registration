using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using VisitorRegistrationApp.Data.Repository;
using Model;
using BL.Services;
using BLL.Helper;
using DAL.Repositories;
using VisitorRegistrationApp.Helper;

namespace VisitorRegistrationApp
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
            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeFilter()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddScoped<IVisitorService, VisitorService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<ICompanyRespository, CompanyRepository>();

            services.AddScoped<IEmployeeRespository, EmployeeRepository>();

            services.AddScoped<IVisitorRepository, VisitorRespository>();





            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsFactory>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                         // services.AddDefaultIdentity<IdentityUser>()
                         .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>()
                         .AddEntityFrameworkStores<ApplicationDbContext>()
                         .AddDefaultTokenProviders();



            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            
            app.UseHttpsRedirection();
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Register.cshtml");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
    
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }
    }
}
