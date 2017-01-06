using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Medico.Model;

namespace Medico.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(Func<T, bool> @where);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        private IDbSet<T> _entities;

        protected Repository(DbContext context)
        {
            Context = context;
        }

        protected virtual IDbSet<T> Entities => _entities ?? (_entities = Context.Set<T>());

        public IQueryable<T> Get(Func<T, bool> @where)
        {
            IQueryable<T> dbQuery = Context.Set<T>();

            var list = dbQuery.Where(@where);

            //Apply eager loading
            return list.AsQueryable();
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException("entity");

            foreach (var entity in entities)
            {
                Entities.Remove(entity);
            }

            Context.SaveChanges();
        }
    }
}