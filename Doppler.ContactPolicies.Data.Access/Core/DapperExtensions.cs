using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using Microsoft.Extensions.DependencyInjection;

namespace Doppler.ContactPolicies.Data.Access.Core
{
    public static class DapperExtensions
    {
        public static IServiceCollection AddAccessData(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseConnectionFactory, DataBaseConnectionFactory>();
            services.AddScoped<IContactPoliciesSettingsRepository, ContactPoliciesSettingsRepository>();
            return services;
        }
    }
}
