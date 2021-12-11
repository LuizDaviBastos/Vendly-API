using System;
using System.Linq;
using System.Linq.Expressions;

namespace ASM.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void AddOrUpdate(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
        public TEntity Get(int id);
        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression);

    }
}
