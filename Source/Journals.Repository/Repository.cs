using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get(Func<T, bool> @where);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        private IDbSet<T> _entities;

        public Repository(DbContext context)
        {
            Context = context;
        }

        protected virtual IDbSet<T> Entities
        {
            get
            {
                return _entities ?? (_entities = Context.Set<T>());
            }
        }

        public IEnumerable<T> Get(Func<T, bool> @where)
        {
            IQueryable<T> dbQuery = Context.Set<T>();

            var list = dbQuery.Where(@where);

            //Apply eager loading
            return list;
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

            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Add(entity);
            Context.SaveChanges();
        }
    }
}