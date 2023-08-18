using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog_system.Blog_system.Context;
using Blog_system.Models;
using Blog_system.Repository;

namespace Blog_system.Controllers
{
    public class BlogsController : Controller
    {
        private readonly BlogSystemDbContext _context;

        //public BlogsController(BlogSystemDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork, BlogSystemDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            Console.WriteLine(_unitOfWork.Blogs);
            try
            {
                var blogs = await _unitOfWork.Blogs.GetAllAsync();
                return View(blogs);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while retrieving blogs.", statusCode: 500);
            }
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);

            var comment = _context.Comments.FirstOrDefault(c => c.BlogId == id);

            var viewModel = new BlogCommentView
            {
                Blog = blog,
                Comment = comment
            };
            if (blog == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Blog blog,string commentContent)
        {
            using (var transaction = _context.Database.BeginTransaction()) //start a new transaction
            {
                try
                {
                    _context.Add(blog);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrWhiteSpace(commentContent))
                    {
                        // Create a new Comment entity and associate it with the newly created blog
                        var comment = new Comment
                        {
                            Content = commentContent,
                            BlogId = blog.Id
                        };
                        _context.Add(comment);
                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    transaction.Rollback(); // revert all commit 
                    return Problem("An Error occured while creating the blog : ", statusCode: 500);
                }
                
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            var comment = _context.Comments.FirstOrDefault(c => c.BlogId == id);

            var viewModel = new BlogCommentView
            {
                Blog = blog,
                Comment = comment,
            };

            if (blog == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Timestamp")] Blog blog, string commentContent)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(blog);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                var entry = ex.Entries.Single();
                var clientValues = (Blog)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "The record has been deleted by another user.");
                }
                else
                {
                    var databaseValues = (Blog)databaseEntry.ToObject();

                    if (databaseValues.Title != clientValues.Title)
                    {
                        ModelState.AddModelError("Title", "Current value: " + databaseValues.Title);
                    }
                    if (databaseValues.Content != clientValues.Content)
                    {
                        ModelState.AddModelError("Content", "Current value: " + databaseValues.Content);
                    }
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you. The edit operation was canceled.");
                    blog.Timestamp = databaseValues.Timestamp;
                }
            }

            if (ModelState.IsValid)
            {
                // Handle comment update if provided
                if (!string.IsNullOrWhiteSpace(commentContent))
                {
                    var existingComment = _context.Comments.FirstOrDefault(c => c.BlogId == blog.Id);
                    if (existingComment != null)
                    {
                        existingComment.Content = commentContent;
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return to the view with error messages
            return View(blog);
        }

        // ... Other actions ...

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            var comment = _context.Comments.FirstOrDefault(c => c.BlogId == id);

            var viewModel = new BlogCommentView
            {
                Blog = blog,
                Comment = comment,
            };

            if (blog == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.Blogs == null)
            {
                return Problem("Entity set 'BlogSystemDbContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _unitOfWork.Blogs.Remove(blog);
            }
            
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
