using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReactionsService.Data;
using WebApplication1.Data;
using WebApplication1.Entities;
using CommunicationKeyAuthClassLibrary;
using LoggingClassLibrary;
using ReactionsService.FakeLogger;
using ReactionsService.Data.FollowingMock;
using System.Reflection;
using System.IO;

namespace ReactionsService
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
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IProductMockRepository, ProductMockRepository>();
            services.AddScoped<IBlackListMockRepository, BlackListMockRepository>();
            services.AddScoped<ITypeOfReactionRepository, TypeOfReactionRepository>();

            services.AddScoped<IFollowingRepository, FollowingRepository>();

            services.AddSingleton<Logger, FakeLoggerService>();
            services.AddSingleton<ILogger, FakeLoggerService>();

            services.AddHttpContextAccessor();
            services.AddDbContext<ContextDB>();

            //definisem svoj swagger dokument
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("ReactionsOpenApiSpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Reactions API",
                    Version = "1",
                    Description = "Pomoću ovog API-ja moguće je pregledati dodate reakcije, dodavati nove, modifikovati i brisati postojeće",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Nataša Zvekić",
                        Email = "natasa.zvekic@uns.ac.rs"
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "FTN licenca"
                    }
                    
                    
                  
                });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments); //spaja vise stringova

                setupAction.IncludeXmlComments(xmlCommentsPath); //da bi swagger mogao citati xml komenatare
            });



            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/ReactionsOpenApiSpecification/swagger.json", "Reactions API");
             
            });

            app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
