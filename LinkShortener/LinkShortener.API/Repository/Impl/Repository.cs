using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.API.Repository.Impl
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly LinkShortenerContext _context;
        private readonly DbSet<T> _entities;

        public Repository(LinkShortenerContext context)
        {
            _entities = (_context = context).Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetAsync(long id)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity)); 

            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}