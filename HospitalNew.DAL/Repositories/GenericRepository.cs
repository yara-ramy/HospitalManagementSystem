using HospitalNew.DAL.Data;
using HospitalNew.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalNew.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<T> Add(T obj)
        {
            await _context.AddAsync(obj);
            _context.SaveChanges();
            return obj;
        }

        public T Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
            return obj;
        }

        public IEnumerable<T> filter(Func<T, bool> predicate = null)
        {
            if (predicate != null)
                _context.Set<T>().Where(predicate).ToList();
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public  T Update(T obj)
        {
            _context.Set<T>().Update(obj);
            return obj;
        }
    }
}
