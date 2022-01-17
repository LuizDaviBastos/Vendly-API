using System;
using System.Linq;
using System.Linq.Expressions;

namespace ASM.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void AddOrUpdate(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
        public TEntity Get(int id);
        public IQueryable<TEntity> GetQueryableAsNoTracking(Expression<Func<TEntity, bool>> expression);
        public IQueryable<TEntity> GetQueryableAsNoTracking();
        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression);
        public IQueryable<TEntity> GetQueryable();
    }
}
