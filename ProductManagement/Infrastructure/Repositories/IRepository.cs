namespace Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task Add(T newEntity);
        Task Update(T updatedEntity);
        Task Delete(int id);
    }
}
