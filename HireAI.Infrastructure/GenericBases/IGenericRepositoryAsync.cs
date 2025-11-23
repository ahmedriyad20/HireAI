using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.GenaricBasies
{
    public interface IGenericRepositoryAsync<T> where T : class
    {

        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
