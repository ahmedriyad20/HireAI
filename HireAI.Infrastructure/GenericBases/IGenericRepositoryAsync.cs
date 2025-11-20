using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.GenaricBasies
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<IEnumerable<T>>? GetAll();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
