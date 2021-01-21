using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Recommendation_Service.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using CommunicationKeyAuthClassLibrary;
using LoggingClassLibrary;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Recommendation_Service.Models;
using Recommendation_Service.Data.Fakes;
using Microsoft.Extensions.Logging;

namespace Recommendation_Service
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
            services.AddSwaggerGen(s => {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Recommendation service API", Version = "v1" });
            });
            services.AddScoped<IProductService, FakeProductService>();
            services.AddSingleton<ILogger, Logger>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("ConnectionStr"))
            {
                UserID = Environment.GetEnvironmentVariable("DB_USERNAME"),
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
                Authentication = SqlAuthenticationMethod.SqlPassword
            };
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.ConnectionString));

    }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //TODO: add Logger
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "Recommendation service API");
            });

            app.UseMiddleware<CommunicationKeyAuthMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
