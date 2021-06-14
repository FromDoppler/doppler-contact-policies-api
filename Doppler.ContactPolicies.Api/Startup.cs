using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Api.DopplerSecurity;

namespace Doppler.ContactPolicies.Api
{
    public class Startup
    {
        private readonly string _baseUrl;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _baseUrl = Configuration.GetValue<string>("BaseURL");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDopplerSecurity();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter the token into field as 'Bearer {token}'",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme},
                        },
                        Array.Empty<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "doppler_contact_policies_api", Version = "v1" });
                if (!string.IsNullOrEmpty(_baseUrl)) { c.AddServer(new OpenApiServer() { Url = _baseUrl }); };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "doppler_contact_policies_api v1"));

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
