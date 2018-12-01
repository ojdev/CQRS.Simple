using CQRS.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.Repositories
{
    public class DefaultRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly CQRSDomainContext _context;
        protected DbSet<TEntity> Table => _context.Set<TEntity>();
        public DefaultRepository(CQRSDomainContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveEntitiesAsync(cancellationToken);
            return entity;

        }
        public async Task<bool> DeleteAsync(object key, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var entity = await Table.FindAsync(key, cancellationToken);
            if (entity != null)
            {
                _context.Remove(entity);
                return await _context.SaveEntitiesAsync(cancellationToken);
            }
            return false;
        }
        public async Task<bool> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entities = Table.Where(predicate);
            foreach (var entity in entities)
            {
                _context.Remove(entity);
            }
            return await _context.SaveEntitiesAsync(cancellationToken);
        }
        public async Task<TEntity> GetAsync(object key, CancellationToken cancellationToken = default(CancellationToken)) => await Table.FindAsync(keyValues: new object[] { key }, cancellationToken: cancellationToken);
        public IQueryable<TEntity> Query() => Table.AsNoTracking();
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var old = _context.Find<TEntity>(entity.Id);
            _context.Entry(old).CurrentValues.SetValues(entity);
            await _context.SaveEntitiesAsync(cancellationToken);
            return entity;
        }
        public async Task<TEntity> MergeAsync<TKey>(TKey key, Action<MegreProperty<TEntity>> megrePropertyAction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var old = await _context.FindAsync<TEntity>(key);
            var megreOption = new MegreProperty<TEntity>(_context.Entry(old));
            megrePropertyAction(megreOption);
            await _context.SaveEntitiesAsync(cancellationToken);
            return old;
        }
    }
}
