using Blog_system.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog_system.Blog_system.Context
{
    public class BlogSystemDbContext : DbContext
    {

        public BlogSystemDbContext(DbContextOptions<BlogSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
