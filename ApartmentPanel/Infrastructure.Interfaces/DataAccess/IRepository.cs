using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApartmentPanel.Infrastructure.Interfaces.DataAccess
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        T Update(T entity, Action<T> updateAction);
        bool Delete(T entity);
        //bool DeleteRange(ICollection<T> entities);
        void Clear();
        //IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null, params Expression<Func<T, object>>[] includesPredicate);
        IQueryable<T> FindBy(Expression<Func<T, bool>> searchPredicate = null);
    }
}
