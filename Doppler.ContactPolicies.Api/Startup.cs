using Doppler.ContactPolicies.Api.DopplerSecurity;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient;
using Doppler.ContactPolicies.Data.Access.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

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
            services.Configure<DopplerDataBaseSettings>(Configuration.GetSection(nameof(DopplerDataBaseSettings)));
            services.AddScoped<IContactPoliciesService, ContactPoliciesService>();
            services.AddUsersApiService();
            services.AddAccessData();
            services.AddDopplerSecurity();
            services.AddControllers();
            services.AddCors();
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Doppler.ContactPolicies.Api", Version = "v1" });
                if (!string.IsNullOrEmpty(_baseUrl))
                {
                    c.AddServer(new OpenApiServer() { Url = _baseUrl });
                }
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Doppler.ContactPolicies.Api v1"));

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(policy => policy
                .SetIsOriginAllowed(isOriginAllowed: _ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
