using Microsoft.EntityFrameworkCore;
using Web.Infrastructure.DBContext;
using System.Linq.Expressions;
using Web.Domains.Core;

namespace Web.Infrastructure.Base
{
    public class BaseRepository<TId, T> : IBaseRepository<TId, T>, IDisposable where T : BaseEntity<TId>
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public T? GetFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return GetAllAsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetAllAsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public T? GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return GetAllAsNoTracking().SingleOrDefault(predicate);
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetAllAsNoTracking().SingleOrDefaultAsync(predicate);
        }

        private IQueryable<T> GetAllAsNoTracking(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>().AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var item in includeProperties)
                {
                    items = items.Include(item);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var item in includeProperties)
                {
                    items = items.Include(item);
                }
            }
            return predicate != null ? items.Where(predicate) : items;
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public int SaveChanges(CancellationToken token = default)
        {
            return _context.SaveChanges();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            return await _context.SaveChangesAsync(token);
        }

        public bool Exist(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return _context.Set<T>().Any(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
    }
}
