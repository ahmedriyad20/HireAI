using HireAI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.GenaricBasies
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HireAIDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public Repository(HireAIDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
            
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;
            _dbSet.Remove(entity);
        }

        public virtual async Task<T>? GetByIdAsync(int id)
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
    
