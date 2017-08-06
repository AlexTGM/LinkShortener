using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;

namespace LinkShortener.API.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<ShortLink>> GetAllAsync(User user = null);
        Task<ShortLink> GetAsync(long id);
        Task<PaginatedData<ShortLink>> GetPageAsync(int skip, int take, User user = null);
        Task InsertAsync(ShortLink entity);
        Task UpdateAsync();
    }
}