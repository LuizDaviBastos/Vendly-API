using ASM.Core.Interfaces;
using ASM.Data.Contexts;
using ASM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ASM.Data.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AsmContext context;
        protected readonly DbSet<TEntity> dbSet;
        public Repository(AsmContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            if(entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public TEntity Get(int id)
        {
            return dbSet.Where(x => x.Id == id).FirstOrDefault();
        }

        public IQueryable<TEntity> GetQueryableAsNoTracking(Expression<Func<TEntity, bool>> expression)
        {
            return dbSet.Where(expression).AsNoTracking();
        }

        public IQueryable<TEntity> GetQueryableAsNoTracking()
        {
            return dbSet.AsQueryable().AsNoTracking();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return dbSet.AsQueryable();
        }


        public void Update(TEntity entity)
        {
            try
            {
                dbSet.Update(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
