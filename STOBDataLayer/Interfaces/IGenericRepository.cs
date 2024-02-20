
namespace STOBDataLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        int AddAsync(T entity);
        int UpdateAsync(T entity);
        int DeleteAsync(int id);
    }
}
