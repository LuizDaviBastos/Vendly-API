using System;
using System.Linq;
using System.Linq.Expressions;

namespace ASM.Core.Interfaces
{
    public interface IRepository<TEntity, IdType> where TEntity : class
    {
        public void Add(TEntity entity);
        public void AddOrUpdate(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(IdType id);
        public TEntity? Get(IdType id);
    }
}
