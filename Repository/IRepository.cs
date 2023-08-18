namespace Blog_system.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

}
