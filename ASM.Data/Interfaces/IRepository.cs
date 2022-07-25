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
        public void Delete(string id);
        public TEntity Get(string id);
    }
}
