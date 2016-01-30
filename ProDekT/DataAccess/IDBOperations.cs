using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProDekT.DataAccess
{
    public interface IDBOperations
    {
        void Dispose();

        int Count<TItem>() where TItem : class, new();
        
        int Count<TItem>(Expression<Func<TItem, bool>> whereClause) where TItem : class, new();

        System.Linq.IQueryable<TItem> Select<TItem>(string[] childCollectionProperties) where TItem : class, new();
        
        System.Linq.IQueryable<TItem> Select<TItem>(string[] childCollectionProperties, Expression<Func<TItem, bool>> whereClause) where TItem : class, new();
        
        System.Linq.IQueryable<TItem> Select<TItem>(string[] childCollectionProperties, Expression<Func<TItem, bool>> whereClause, string sortExpression, string sortDirection, int pageIndex, int pageSize) where TItem : class, new();
        
        System.Linq.IQueryable<TItem> Select<TItem>(string childCollectionPropertyName, Expression<Func<TItem, bool>> whereClause, Expression<Func<TItem, bool>> whereClauseForChildCollection, string sortExpression, string sortDirection, int pageIndex, int pageSize) where TItem : class, new();
        
        System.Linq.IQueryable<TItem> Select<TItem>(string[] childCollectionProperties, string sortExpression, string sortDirection, int pageIndex, int pageSize) where TItem : class, new();
        
        TItem Get<TItem>(object objectId, string[] childCollectionProperties) where TItem : class, new();
        
        TItem Get<TItem>(object objectId, string[] childCollectionProperties, Expression<Func<TItem, bool>> whereClause) where TItem : class, new();
        
        List<TItem> ExecuteProcedure<TItem>(string spName, System.Collections.Hashtable keyValuePair);
        
        TItem Insert<TItem>(TItem item) where TItem : class, new();
        
        TItem Update<TItem>(TItem item) where TItem : class, new();

        bool Delete<TItem>(TItem item) where TItem : class, new();
    }
}
