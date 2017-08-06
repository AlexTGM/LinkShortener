using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Repository.Impl
{
    public class Repository : IRepository
    {
        private readonly LinkShortenerContext _context;
        private readonly DbSet<ShortLink> _entities;

        public Repository(LinkShortenerContext context)
        {
            _entities = (_context = context).Set<ShortLink>();
        }

        public async Task<IEnumerable<ShortLink>> GetAllAsync(User user = null)
        {
            var query = user == null ? _entities : _entities.Where(l => l.User == user);

            return await query.ToListAsync();
        }

        public async Task<ShortLink> GetAsync(long id)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PaginatedData<ShortLink>> GetPageAsync(int skip, int take, User user = null)
        {
            var query = user == null ? _entities : _entities.Where(l => l.User == user);

            var data = await query.Skip(skip).Take(take).ToListAsync();
            var totalCount = query.Count();

            return new PaginatedData<ShortLink> {TotalCount = totalCount, Data = data};
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(ShortLink entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}