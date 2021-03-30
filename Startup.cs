using AnimalChip.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using AnimalChip.Options;
using AnimalChip.Models;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;
using AnimalChip.Services;

namespace AnimalChip
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
            services.AddMemoryCache();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(
                opt =>
                {
                    var supportedCulteres = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("fr"),
                    new CultureInfo("pl")
                };
                    opt.DefaultRequestCulture = new RequestCulture("en");
                    opt.SupportedCultures = supportedCulteres;
                    opt.SupportedUICultures = supportedCulteres;
                    opt.SupportedUICultures = supportedCulteres;
                });
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //    {
            //        options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
            //    });
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.AccessDeniedPath = "/Home/AccessDeniedPathInfo";
            });
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AnimalContext")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(opt =>
               opt.UseInMemoryDatabase("Animal"));
            services.AddControllers();
            services.AddAuthorization();
            services.AddSwaggerGen();
        }
 
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            //var supportedCultres = new[] { "en", "fr", "es" };
            //var locationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultres[0])
            //.AddSupportedCultures(supportedCultres)
            //.AddSupportedUICultures(supportedCultres); 

            //app.UseRequestLocalization(locationOptions);

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapControllerRoute(
                   name: "index",
                   pattern: "Animals/YourAnimals",
                   defaults: new
                   {
                       controller = "Animals",
                       Action = "IndexYourAnimal"
                   });

                 endpoints.MapControllerRoute(
                   name: "search",
                   pattern: "Animals/SearchAnimals",
                   defaults: new
                   {
                       controller = "Animals",
                       Action = "AdvancedSearch"
                   });

                endpoints.MapControllerRoute(
                    name: "result",
                    pattern: "Animals/Presence",
                    defaults: new
                    {
                        controller = "Animals",
                        Action = "ShowResults"
                    });

                endpoints.MapControllerRoute(
                   name: "resultadvanced",
                   pattern: "Animals/Results",
                   defaults: new
                   {
                       controller = "Animals",
                       Action = "ShowResultsAdvanced"
                   });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();


            });
        }
    }
}
