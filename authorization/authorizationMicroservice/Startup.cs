using authorizationMicroservice.Data;
using authorizationMicroservice.Helpers;
using AutoMapper;
using CommunicationKeyAuthClassLibrary;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace authorizationMicroservice
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Authorization Microservice",
                    Version = "1",
                    Description = "Mikroservis koji služi za autorizaciju korisnika i admin i putem kojeg se dobijaju tokeni za autentifikaciju.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Marko Puzović",
                        Email = "markopuzovi98@gmail.com",
                        Url = new Uri("http://markopuzovic.website/")
                    }
                });
                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);
                c.IncludeXmlComments(xmlCommentsPath);
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IAuthorizationHelper, AuthorizationHelper>();
            services.AddScoped<IFakeUserRepository, FakeUserRepository>();
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ILoggerProvider, LoggerProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,ILoggerProvider loggerProvider, ILogger logger)
        {
            loggerFactory.AddProvider(loggerProvider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization Microservice"));
            }

            app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
