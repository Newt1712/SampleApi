using System.Linq.Expressions;

namespace Web.Infrastructure.Base
{
    public interface IBaseRepository<TId, T>
    {

        T? GetFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T? GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);
        ValueTask AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);

        void RemoveMultiple(List<T> entities);

        bool Exist(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        int SaveChanges(CancellationToken token = default);
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
