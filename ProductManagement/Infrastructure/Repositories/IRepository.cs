namespace Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task Add(T newEntity);
        Task Update(int id, T updatedEntity);
        Task Delete(int id);
    }
}
