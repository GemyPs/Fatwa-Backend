using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Repository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext dbContext;
        public Repository(ApplicationContext context)
        {
            dbContext = context;
        }

        public virtual IQueryable<TEntity> AsQuerable()
        {
            return dbContext.Set<TEntity>();
        }


        public virtual void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual List<TEntity> Fetch()
        {
            return dbContext.Set<TEntity>().ToList();
        }

        public virtual TEntity GetById(int id) 
        {
            return dbContext.Set<TEntity>().Find(id);
        }
        public virtual void Insert(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
        }

        public virtual bool IsFound(int id)
        {
            return dbContext.Set<TEntity>().Find(id) == null ? false : true;
        }
        public virtual void Update(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            dbContext.Set<TEntity>().RemoveRange(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            dbContext.Set<TEntity>().AddRange(entity);
        }
    }
}