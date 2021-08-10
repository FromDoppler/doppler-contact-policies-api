using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Doppler.ContactPolicies.Api.UsersApi
{
    public static class UsersApiServiceCollectionExtensions
    {
        public static IServiceCollection AddUsersApiService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUsersApiTokenGetter, UsersApiTokenGetter>();
            services.AddScoped<IUserFeaturesService, UserFeaturesService>();
            return services;
        }
    }
}
