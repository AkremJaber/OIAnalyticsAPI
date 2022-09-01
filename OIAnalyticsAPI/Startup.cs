using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using OIAnalyticsAPI.Configs;
using OIAnalyticsAPI.IService;
using OIAnalyticsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI
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
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration)
                    .EnableTokenAcquisitionToCallDownstreamApi()
                    .AddInMemoryTokenCaches();

            services.AddScoped<ITenantsService,TenantsService>();
            services.AddScoped<IDialogConfigParmService,DialogConfigParmService>();
            services.AddScoped<IAssignPersonTenant, AssignPersonTenant>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPowerBIService, PowerBIService>();
            services.AddScoped<ITenantsHasPersonsService, TenantsHasPersonsService>();
            services.AddScoped<IEmbeddedReportService, EmbeddedReportService>();
            services.AddScoped<IEmbeddedDashboardService, EmbeddedDashboardService>();
            services.AddScoped<IEmbeddedDataSetService, EmbeddedDataSetService>();
            services.AddScoped<ITenantDetailsService, TenantDetailsService>();
            services.AddCors();
            //DBCONTEXT CONFIG
            string connectString = Configuration["ConnectionStrings:ConnectString"];
            services.AddDbContext <OIAnalyticsDBconfig>(opt => opt.UseSqlServer(connectString));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OIAnalyticsAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OIAnalyticsAPI v1"));
            }
            app.ConfigureExceptionHandler(logger);

            app.UseExceptionHandler("/error");

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
