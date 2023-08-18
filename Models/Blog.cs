using System.ComponentModel.DataAnnotations;

namespace Blog_system.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; } // Concurrency token property

        public virtual List<Comment> comments { get; set; }

    }
}
