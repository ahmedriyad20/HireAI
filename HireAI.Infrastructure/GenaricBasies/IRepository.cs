using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.GenaricBasies
{
    public interface IRepository<T> where T : class
    {
        Task<T>? GetByIdAsync(int id);
        Task<IEnumerable<T>>? ListAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
