using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Recommendation_Service.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("ConnectionStr"));
            builder.UserID = Environment.GetEnvironmentVariable("DB_USERNAME");
            builder.Password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            builder.Authentication = SqlAuthenticationMethod.SqlPassword;
            Console.WriteLine(builder.ConnectionString);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.ConnectionString));

    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "Recommendation service API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
