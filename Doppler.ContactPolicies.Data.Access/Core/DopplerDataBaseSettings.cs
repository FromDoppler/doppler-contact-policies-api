using System;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Doppler.ContactPolicies.Data.Access.Core
{
    public class DopplerDataBaseSettings : IDopplerDataBaseSettings
    {
        public string ConnectionString { get; set; }

        public string Password { get; set; }

        public int? DbCommandTimeoutInMilliseconds { get; set; }

        public string GetSqlConnectionString()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new Exception("ConnectionString cannot be null or empty");
            }

            var builder = new SqlConnectionStringBuilder(ConnectionString)
            {
                ApplicationName = Assembly.GetEntryAssembly().GetName().Name
            };
            if (!string.IsNullOrWhiteSpace(Password))
            {
                builder.Password = Password;
            }

            return builder.ConnectionString;
        }
    }
}
