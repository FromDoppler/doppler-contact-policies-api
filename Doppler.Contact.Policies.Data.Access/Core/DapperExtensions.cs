using Doppler.Contact.Policies.Data.Access.Entities.Student;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.Contact.Policies.Data.Access.Core
{
   public static class DapperExtensions
    {
        public static IServiceCollection AddAccessData(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();

            //AddTypeHandlers();

            return services;
        }

        //private static void AddTypeHandlers()
        //{
        //    SqlMapper.AddTypeHandler(new SupportValidationReasonTypeHandler());
        //}
    }
}
