using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal abstract class BaseRepository<T> : IRepository<T>
    {
        protected ICollection<T> _entityCollection;

        protected BaseRepository(ICollection<T> entityCollection)
        {
            _entityCollection = entityCollection;
            //SetEntityCollection(communicatorFactory);
        }

        public void Add(T entity) => _entityCollection.Add(entity);
        public void AddRange(ICollection<T> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }
        public bool Delete(T entity) => _entityCollection.Remove(entity);
        /*public bool DeleteRange(ICollection<T> entities)
        {
            foreach (var item in entities)
            {
                Delete(item);
            }
        }*/
        public T Update(T entity, Action<T> updateAction)
        {
            if (entity != null)
            {
                updateAction(entity);
            }
            return entity;
        }
        public void Clear() => _entityCollection.Clear();
        public IQueryable<T> GetAll() => _entityCollection.AsQueryable();
        //public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null, params Expression<Func<T, object>>[] includesPredicate)
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null)
        {
            try
            {
                var result = searchPredicate == null ? GetAll() : GetAll().Where(searchPredicate);

                /*if (includesPredicate.Any())
                {
                    result = includesPredicate
                        .Aggregate(result, (current, include) => current.Include(include));
                }*/

                return result;
            }
            catch
            {
                throw;
            }
        }
        /*protected void SetEntityCollection(IDbModelCommunicatorFactory communicatorFactory)
        {
            var reader = communicatorFactory.CreateDbModelReader();
            _entityCollection = reader.Get();
        }*/
    }
}
