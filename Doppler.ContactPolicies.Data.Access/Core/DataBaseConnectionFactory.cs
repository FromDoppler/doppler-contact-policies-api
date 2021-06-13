using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Doppler.ContactPolicies.Data.Access.Core
{
    public class DataBaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public DataBaseConnectionFactory(IOptions<DopplerDataBaseSettings> dopplerDataBaseSettings)
        {
            _connectionString = dopplerDataBaseSettings.Value.GetSqlConnectionString();
        }

        /// <summary>
        /// Open new connection and return it for use
        /// </summary>
        /// <returns></returns>
        public async Task<IDbConnection> GetConnection()
        {
            var cn = new SqlConnection(_connectionString);
            await cn.OpenAsync();
            return cn;
        }
    }
}
