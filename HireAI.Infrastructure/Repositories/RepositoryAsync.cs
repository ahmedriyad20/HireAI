using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _db;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;
            _dbSet.Remove(entity);
        }

        public virtual async Task<T>? GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>>? ListAsync()
        {
            return await _dbSet.ToListAsync(); //
        }

        public virtual  Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask; // Defer SaveChanges to UnitOfWork
        }
    }
}
    
