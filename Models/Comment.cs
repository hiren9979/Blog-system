namespace Blog_system.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogId { get; set; } //foreign key

        public virtual Blog Blog { get; set; }
    }
}
