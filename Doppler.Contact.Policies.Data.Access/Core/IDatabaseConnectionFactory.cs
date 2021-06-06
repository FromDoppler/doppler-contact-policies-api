using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.Contact.Policies.Data.Access.Core
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> GetConnection();
    }
}
