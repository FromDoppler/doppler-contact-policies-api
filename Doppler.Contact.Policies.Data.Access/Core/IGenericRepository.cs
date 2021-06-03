using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.Contact.Policies.Data.Access.Core
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteRowAsync(Guid id);
        Task<T> GetAsync(Guid id);
        Task<int> SaveRangeAsync(IEnumerable<T> list);
        Task<int> UpdateAsync(T t);
        Task<int> InsertAsync(T t);
    }
}
