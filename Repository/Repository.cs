using Blog_system.Blog_system.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog_system.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BlogSystemDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(BlogSystemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }

}
