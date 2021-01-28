using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CommentingService.Data;
using CommentingService.Data.Comment;
using CommentingService.Data.FollowingMock;
using CommentingService.Entities;
using CommentingService.FakeLogger;
using CommunicationKeyAuthClassLibrary;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommentingService
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

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFollowingRepository, FollowingRepository>();
            services.AddScoped<IProductMockRepository, ProductMockRepository>();
            services.AddScoped<IBlackListMockRepository, BlackListMockRepository>();


            services.AddSingleton<Logger, FakeLoggerService>();
            services.AddSingleton<ILogger, FakeLoggerService>();

            services.AddDbContext<ContextDB>();

            services.AddHttpContextAccessor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(setupAction =>
            {
               setupAction.SwaggerDoc("CommentingApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Commenting API",
                        Version = "1",
                        Description = "Pomoću ovog API-ja moguće je pregledati dodate komentare, dodavati nove, modifikovati i brisati postojeće",
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

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction=> {
                setupAction.SwaggerEndpoint("/swagger/CommentingApiSpecification/swagger.json", "Commenting API");
            });

            app.UseRouting();

            app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
