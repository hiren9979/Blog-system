using Blog_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_system.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Blog> Blogs { get; }
        IRepository<Comment> Comments { get; }
        Task<int> SaveChangesAsync();
    }
}
