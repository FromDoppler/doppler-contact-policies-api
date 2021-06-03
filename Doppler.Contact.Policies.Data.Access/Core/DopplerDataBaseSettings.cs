using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.Contact.Policies.Data.Access.Core
{
  public  class DopplerDataBaseSettings
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
