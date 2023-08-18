using Blog_system.Blog_system.Context;
using Blog_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_system.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogSystemDbContext _context;

        public IRepository<Blog> Blogs { get; private set; }
        public IRepository<Comment> Comments { get; private set; }

        public UnitOfWork(BlogSystemDbContext context)
        {
            _context = context;
            Blogs = new Repository<Blog>(_context);
            Comments = new Repository<Comment>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
