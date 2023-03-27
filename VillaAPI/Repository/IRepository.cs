using System.Linq.Expressions;

namespace VillaAPI.Repository
{
    public interface IRepository<T> where T:class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T villa);

        Task DeleteAsync(T villa);
        Task Save();
    }
}
