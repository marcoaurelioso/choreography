using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using order.api.Config;
using order.api.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;

namespace order.api
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

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Title = "Order API",
            //        Version = "v1",
            //        Description = "Order API using MongoDB and Kafka",
            //    });
            //    // Set the comments path for the Swagger JSON and UI.
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //    //string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
            //    //string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
            //    //string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
            //    //c.IncludeXmlComments(caminhoXmlDoc);
            //});
            services.AddSwaggerDocument(c =>
            {
                c.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ToDo API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                };
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            //var swaggerOptions = new SwaggerOptions();
            //Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            //app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            //app.UseSwaggerUI(option =>
            //{
            //    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            //});

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseOpenApi();
            app.UseSwaggerUi3();
            
        }
    }
}
