using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPI.Data;

namespace VillaAPI.Repository
{
    public class Repository<T>:IRepository<T> where T:class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> dbSet; 

        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            dbSet = db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}
