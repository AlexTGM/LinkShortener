using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models.Database;

namespace LinkShortener.API.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(long id);
        Task InsertAsync(T entity);
    }
}