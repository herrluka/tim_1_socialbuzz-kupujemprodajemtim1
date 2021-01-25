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

           services.AddSingleton<Logger, FakeLoggerService>();
           services.AddSingleton<ILogger, FakeLoggerService>();

           services.AddHttpContextAccessor();
           services.AddDbContext<ContextDB>();



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

           app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
