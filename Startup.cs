using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Data.Seeder;
using PizzaWebsite.Services.GoogleMaps;
using PizzaWebsite.Services.reCAPTCHA_v2;
using PizzaWebsite.Services.SendGrid;
using System;

namespace PizzaWebsite
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("PizzaWebsiteConnection")));

            // add database context
            services.AddDbContext<PizzaWebsiteContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PizzaWebsiteConnection"))
                        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                        .EnableSensitiveDataLogging() // Logging is built into ASP.NET Core
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // no entity tracking to improve performance
            });

            // add repository
            services.AddScoped<IPizzaWebsiteRepository, PizzaWebsiteRepository>();

            // add data seeder
            services.AddTransient<PizzaWebsiteDataSeeder>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // add reCAPTCHA verfier to controller
            services.AddTransient<IReCaptchaVerifier, ReCaptchaVerifier>();

            services.Configure<ReCaptchaOptions>(options =>
            {
                options.SiteKey = Configuration["ExternalProviders:reCAPTCHA_v2:SiteKey"];
                options.SecretKey = Configuration["ExternalProviders:reCAPTCHA_v2:SecretKey"];
            });

            // set up Google Maps options
            services.Configure<GoogleMapsOptions>(options =>
            {
                options.ApiKey = Configuration["ExternalProviders:GoogleMaps:ApiKey"];
                options.CompanyAddress = Configuration["ExternalProviders:GoogleMaps:CompanyAddress"];
            });

            services.AddTransient<IGeocoder, Geocoder>();

            // add SendGrid email sender to controller
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
                options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
                options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
                options.CompanyEmail = Configuration["ExternalProviders:SendGrid:CompanyEmail"];
            });

            services.AddControllersWithViews();
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
