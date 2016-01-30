using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProDekT.DataAccess
{
    public class DataAccessBase<TItem> : IDataAccessBase<TItem>
        where TItem : class, new()
    {
        protected virtual IDBOperations dataManager { get; set; }

        public DataAccessBase()
        {
            dataManager = new DBOperations<ProDekTDBContext>("ProDekTDBContext");
        }

        public virtual IQueryable<TItem> GetList(String[] childCollectionProperties)
        {
            return dataManager.Select<TItem>(childCollectionProperties);
        }

        public virtual IQueryable<TItem> GetList(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause)
        {
            return dataManager.Select<TItem>(childCollectionProperties, whereClause);
        }

        public virtual IQueryable<TItem> GetList(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause, string sortExpression, string sortDirection,
            int pageIndex, int pageSize)
        {
            return dataManager.Select<TItem>(childCollectionProperties, whereClause, sortExpression,
                sortDirection, pageIndex, pageSize);
        }

        public virtual IQueryable<TItem> GetList(String[] childCollectionProperties, string sortExpression,
            string sortDirection, int pageIndex, int pageSize)
        {
            return dataManager.Select<TItem>(childCollectionProperties, sortExpression, sortDirection,
                pageIndex, pageSize);
        }

        public virtual IQueryable<TItem> GetList(String childCollectionPropertyName,
            Expression<Func<TItem, bool>> whereClause, Expression<Func<TItem, bool>> whereClauseForChildCollection,
            string sortExpression, string sortDirection, int pageIndex, int pageSize)
        {
            return dataManager.Select<TItem>(childCollectionPropertyName, whereClause,
                whereClauseForChildCollection, sortExpression, sortDirection, pageIndex, pageSize);
        }

        public virtual void Dispose()
        {
            if (dataManager != null)
            {
                dataManager.Dispose();
                dataManager = null;
            }
        }

        public virtual int GetCount<T>() where T : class, new()
        {
            return dataManager.Count<T>();
        }

        public virtual int GetCount<T>(Expression<Func<T, bool>> whereClause) where T : class, new()
        {
            return dataManager.Count<T>(whereClause);
        }

        public virtual T Insert<T>(T newObject) where T : class, new()
        {
            return dataManager.Insert<T>(newObject);
        }

        public virtual T GetById<T>(int objectId, String[] childCollectionProperties) where T : class, new()
        {
            return dataManager.Get<T>(objectId, childCollectionProperties);
        }

        public virtual T GetById<T>(int objectId, String[] childCollectionProperties,
            Expression<Func<T, bool>> whereClause) where T : class, new()
        {
            return dataManager.Get<T>(objectId, childCollectionProperties, whereClause);
        }

        public virtual T Update<T>(T modifiedObject) where T : class, new()
        {
            return dataManager.Update<T>(modifiedObject);
        }

        public virtual Boolean Delete<T>(int objectId) where T : class, new()
        {
            T objectToBeDeleted = GetById<T>(objectId, null);

            return dataManager.Delete<T>(objectToBeDeleted);
        }

        public virtual Boolean Delete<T>(T objectToBeDeleted) where T : class, new()
        {
            return dataManager.Delete<T>(objectToBeDeleted);
        }

        public virtual List<TItem> ExecuteProcedure(string spName, Hashtable keyValuePair)
        {
            return dataManager.ExecuteProcedure<TItem>(spName, keyValuePair);
        }
    }
}
