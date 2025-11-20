using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IRepository<T> where T : class
    {

        Task<T>? GetByIdAsync(Guid id);
        Task<IEnumerable<T>>? ListAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
