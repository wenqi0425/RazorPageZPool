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
using Microsoft.AspNetCore.Identity.UI.Services;
using ZPool.Models;
using ZPool.Services.EFServices.RideService;
using ZPool.Services.Interfaces;
using ZPool.Services.EFServices;

namespace ZPool
{
    // initialize configurations 
    public class Startup
    {
        // (DI) IConfiguration loading configuration, JSON, etc.
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }



        public IConfiguration Configuration { get; }

        // To tell the container to produce instances by the runtime, methods to register services in container
        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext: data access object, to implement ORM, object-relationship
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CloudConnection")));

            // DB -- DbContext --> EFService --> IService --> PageModel
            services.AddTransient<IRideService, EFRideService>();
            services.AddTransient<ICarService, EFCarService>();
            services.AddTransient<IBookingService, EFBookingService>();
            services.AddTransient<IMessageService, EFMessageService>();
            services.AddTransient<IReviewService, EFReviewService>();
            services.AddTransient<IEmailSender, EmailService>();

            services.AddRazorPages();

            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                    options.SignIn.RequireConfirmedAccount = true;
                    
            })
                
                .AddEntityFrameworkStores<AppDbContext>()  
                .AddDefaultTokenProviders()                
                .AddRoles<IdentityRole<int>>()                      
                .AddDefaultUI();       

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
                
                
                // Lockout options:
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;

            });
            
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Administration");  // need Authority to visit it
            });
            
        }

        // to configure the HTTP request pipeline by the runtime.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())  
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else     // IsEnvironment, IsProduction
            {
                app.UseExceptionHandler("/Error");  
                app.UseHsts();  // for SSL security(https)
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();  // valid user? (password, username, emailConfirmed?)
            app.UseAuthorization();   // Authorities, role

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
