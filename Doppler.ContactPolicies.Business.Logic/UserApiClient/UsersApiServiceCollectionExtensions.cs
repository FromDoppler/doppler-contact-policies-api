using Doppler.ContactPolicies.Business.Logic.UserApiClient.Handlers;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient
{
    public static class UsersApiServiceCollectionExtensions
    {
        public static IServiceCollection AddUsersApiService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserFeaturesService, UserFeaturesService>();
            services.AddTransient<TokenHandler>();
            services.AddHttpClient("users-api").AddHttpMessageHandler<TokenHandler>();
            return services;
        }
    }
}
