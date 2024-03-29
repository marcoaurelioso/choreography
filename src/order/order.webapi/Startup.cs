﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using order.webapi.Models;

namespace order.webapi
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
            // MONGODB
            var config = new Config.ServerConfig();
            Configuration.Bind(config);

            var orderContext = new OrderContext(config.MongoDB);
            // we can hook up that repository and make a Singleton that we can inject in our controller
            var repo = new OrderRepository(orderContext);

            services.AddSingleton<IOrderRepository>(repo);

            services.AddSwaggerDocument(c =>
            {
                c.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Order API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
