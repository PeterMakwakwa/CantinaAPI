using CantinaAPI.Data;
using CantinaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CantinaAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CantinaDbContext _context;

        public Repository(CantinaDbContext context)
        {
            _context = context;
        }       
        public async Task<T> CreateAsync(T entity)
        {
            var addedEntity = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(); 
            return addedEntity.Entity; 
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async  Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public T Update(T entity)
        {
            return _context.Set<T>().Update(entity).Entity;
        }
    }
}
