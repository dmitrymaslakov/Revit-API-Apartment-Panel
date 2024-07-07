using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApartmentPanel.Infrastructure.Interfaces.DataAccess
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T Update(T entity, Action<T> updateAction);
        void Delete(T entity);
        void AddRange(ICollection<T> entities);
        void Clear();
        //IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null, params Expression<Func<T, object>>[] includesPredicate);
        IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null);
    }
}
