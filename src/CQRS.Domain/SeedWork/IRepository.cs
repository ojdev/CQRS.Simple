using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Domain.SeedWork
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetAsync(object key, CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<TEntity> Query();
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteAsync(object key, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="megrePropertyAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> MergeAsync<TKey>(TKey key, Action<MegreProperty<TEntity>> megrePropertyAction, CancellationToken cancellationToken = default(CancellationToken));
    }
}
