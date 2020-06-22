using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
            });
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
            
            app.UseStaticFiles();
            app.UseRouting();
            
            logger.LogInformation("Before Use MVC.");
            app.UseMvcWithDefaultRoute();
            logger.LogInformation("After Use MVC.");

            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultFilesOptions);
            //These two middlewares can be replaced with UseFileServer middleware. And also instead of deafultFileOptions we need to use FileServerOptions object for overload the startup page.

            // FileServerOptions fileServerOptions = new FileServerOptions();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");
            // app.UseFileServer(fileServerOptions);
            

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW1: Incoming Request");
                await next.Invoke();
                logger.LogInformation("MW1: Outgoing Response");
            });

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW2: Incoming Request");
                await next.Invoke();
                logger.LogInformation("MW2: Outgoing Response");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    logger.LogInformation("EndPoint: Incoming Request.");
                    await context.Response.WriteAsync("Hosting Environment: " + env.EnvironmentName);
                    logger.LogInformation("EndPoint: Outgoing Response.");
                });
            });

            app.Run(async (context) => {
                logger.LogInformation("MVC: Incoming Request.");
                await context.Response.WriteAsync("Hello World!");
                logger.LogInformation("MVC: Outgoing Response.");
            });
        }
    }
}
