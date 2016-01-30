using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProDekT.DataAccess
{
    public interface IDataAccessBase<TItem>
        where TItem : class, new()
    {
        IQueryable<TItem> GetList(String[] childCollectionProperties);

        IQueryable<TItem> GetList(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause);

        IQueryable<TItem> GetList(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause, string sortExpression, string sortDirection,
            int pageIndex, int pageSize);

        IQueryable<TItem> GetList(String[] childCollectionProperties, string sortExpression,
            string sortDirection, int pageIndex, int pageSize);

        IQueryable<TItem> GetList(String childCollectionPropertyName,
            Expression<Func<TItem, bool>> whereClause, Expression<Func<TItem, bool>> whereClauseForChildCollection,
            string sortExpression, string sortDirection, int pageIndex, int pageSize);

        void Dispose();

        int GetCount<T>() where T : class, new();

        int GetCount<T>(Expression<Func<T, bool>> whereClause) where T : class, new();

        T Insert<T>(T newObject) where T : class, new();

        T GetById<T>(int objectId, String[] childCollectionProperties) where T : class, new();

        T GetById<T>(int objectId, String[] childCollectionProperties,
            Expression<Func<T, bool>> whereClause) where T : class, new();

        T Update<T>(T modifiedObject) where T : class, new();

        Boolean Delete<T>(int objectId) where T : class, new();

        Boolean Delete<T>(T objectToBeDeleted) where T : class, new();

        List<TItem> ExecuteProcedure(string spName, Hashtable keyValuePair);
    }
}
