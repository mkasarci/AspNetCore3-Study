using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreKudvenkat
{
    
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            
            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequireUppercase= false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();
            
            // services.AddMvc(options => {
            //     options.EnableEndpointRouting = false;
            // }).AddXmlSerializerFormatters();

            services.AddRazorPages().AddMvcOptions(options =>{
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthorization(options => {
            
            });

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };

                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
            
            // logger.LogInformation("Before Use MVC.");
            // app.UseMvcWithDefaultRoute();
            // logger.LogInformation("After Use MVC.");

            // app.Use(async (context, next) =>
            // {
            //     logger.LogInformation("MW1: Incoming Request");
            //     await next.Invoke();
            //     logger.LogInformation("MW1: Outgoing Response");
            // });

            // app.Use(async (context, next) =>
            // {
            //     logger.LogInformation("MW2: Incoming Request");
            //     await next.Invoke();
            //     logger.LogInformation("MW2: Outgoing Response");
            // });

            app.UseAuthorization();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvc();
        }
    }
}
