using ASM.Core.Interfaces;
using ASM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ASM.Data.Common
{
    public class Repository<TEntity> : IRepository<TEntity, Guid> where TEntity : EntityBase
    {
        protected DbSet<TEntity> dbSet { get; set; }
        protected AsmContext dbContext { get; set; }

        public Repository(AsmContext dbContext)
        {
            dbSet = dbContext.Set<TEntity>();
            this.dbContext = dbContext;

        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public virtual TEntity? Get(Guid id)
        {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }
    }
}
