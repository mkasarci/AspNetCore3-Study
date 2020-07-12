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
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireUppercase= false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            
            // services.AddMvc(options => {
            //     options.EnableEndpointRouting = false;
            // }).AddXmlSerializerFormatters();

            services.AddRazorPages().AddMvcOptions(options =>{
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            
            services.AddAuthentication().AddGoogle(options => 
            {
                options.ClientId = "290620975582-aso2qt748ohl5bc3pq75s2pheddmk8rd.apps.googleusercontent.com";
                options.ClientSecret = "4KEf-GHrPkpYD1S5vLNaFSAk"; 
            });

            services.AddAuthorization(options => 
            {
                options.AddPolicy("CreateRolePolicy" ,
                                    policy => policy.RequireClaim("Create Role", "true"));
                options.AddPolicy("EditRolePolicy" ,
                                    policy => policy.RequireClaim("Edit Role", "true"));
                options.AddPolicy("DeleteRolePolicy", 
                                    policy => policy.RequireClaim("Delete Role", "true"));
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
