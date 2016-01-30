using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProDekT.Extensions;

namespace ProDekT.DataAccess
{
    public class DBOperations<TContext> : IDisposable, IDBOperations
        where TContext : DbContext, IObjectContextAdapter, new()
    {

        #region Private Fields
        private TContext _context; 
        #endregion

        #region Constructors
        public DBOperations(string nameOfConnectionString)
        {
            _context = new TContext();
            _context.Database.Connection.ConnectionString =
                ConfigurationManager.ConnectionStrings[nameOfConnectionString].ConnectionString; ;
        } 
        #endregion

        private PropertyInfo GetDbSet(Type itemType)
        {
            var properties = typeof(TContext).GetProperties().Where(
                item => item.PropertyType.Equals(typeof(DbSet<>).MakeGenericType(itemType)));

            return properties.First();
        }

        private static DbSet<TItem> AttachChildCollection<TItem>(String[] childCollectionProperties,
            DbSet<TItem> dbSet) where TItem : class, new()
        {
            IQueryable<TItem> qryGet = dbSet.AsNoTracking();

            if (childCollectionProperties != null)
            {
                foreach (String childCollectionPropertyName in childCollectionProperties)
                {
                    dbSet.Include(childCollectionPropertyName);
                }
            }

            return dbSet;
        }

        private static IQueryable<TItem> AttachChildCollection<TItem>(String[] childCollectionProperties,
            IQueryable<TItem> qryGet) where TItem : class, new()
        {
            if (childCollectionProperties != null)
            {
                foreach (String childCollectionPropertyName in childCollectionProperties)
                {
                    qryGet = qryGet.Include(childCollectionPropertyName).AsNoTracking();
                }
            }

            return qryGet;
        }

        private static IQueryable<TItem> AttachChildCollection<TItem>(String childCollectionPropertyName,
            IQueryable<TItem> qryGet, Expression<Func<TItem, bool>> whereClauseForChildCollection) where TItem : class, new()
        {
            if (childCollectionPropertyName != null)
            {
                qryGet = qryGet.Include(childCollectionPropertyName).Where(whereClauseForChildCollection).AsNoTracking();
            }

            return qryGet;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        public int Count<TItem>()
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;

            return dbSet.Count<TItem>();
        }

        public int Count<TItem>(Expression<Func<TItem, bool>> whereClause)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;

            return dbSet.Where(whereClause).Count<TItem>();
        }

        public IQueryable<TItem> Select<TItem>(String[] childCollectionProperties)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;
            IQueryable<TItem> qryGet = AttachChildCollection<TItem>(childCollectionProperties, dbSet);

            return qryGet.AsNoTracking();
        }

        public IQueryable<TItem> Select<TItem>(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;
            IQueryable<TItem> qryGet = dbSet.Where(whereClause);
            qryGet = AttachChildCollection<TItem>(childCollectionProperties, qryGet);

            return qryGet.AsNoTracking();
        }

        public IQueryable<TItem> Select<TItem>(String[] childCollectionProperties, string sortExpression,
            string sortDirection, int pageIndex, int pageSize)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;

            if (sortDirection.ToLower() == "desc")
            {
                sortExpression += " DESC";
            }

            IQueryable<TItem> qryGet = dbSet.Skip(pageIndex * pageSize).Take(pageSize).OrderBy(sortExpression);
            qryGet = AttachChildCollection<TItem>(childCollectionProperties, qryGet);

            return qryGet.AsNoTracking();
        }

        public IQueryable<TItem> Select<TItem>(String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause, string sortExpression, string sortDirection,
            int pageIndex, int pageSize)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;

            if (sortDirection.ToLower() == "desc")
            {
                sortExpression += " DESC";
            }

            IQueryable<TItem> qryGet = dbSet.Where(whereClause).Skip(pageIndex * pageSize).Take(pageSize).OrderBy(sortExpression);
            qryGet = AttachChildCollection<TItem>(childCollectionProperties, qryGet);

            return qryGet.AsNoTracking();
        }

        public IQueryable<TItem> Select<TItem>(String childCollectionPropertyName,
            Expression<Func<TItem, bool>> whereClause, Expression<Func<TItem, bool>> whereClauseForChildCollection,
            string sortExpression, string sortDirection, int pageIndex, int pageSize)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;

            if (sortDirection.ToLower() == "desc")
            {
                sortExpression += " DESC";
            }

            IQueryable<TItem> qryGet = null;

            if (whereClause != null)
            {
                qryGet = dbSet.Where(whereClause).Skip(pageIndex * pageSize).Take(pageSize).OrderBy(sortExpression);
            }
            else
            {
                qryGet = dbSet.Skip(pageIndex * pageSize).Take(pageSize).OrderBy(sortExpression);
            }

            qryGet = AttachChildCollection<TItem>(childCollectionPropertyName, qryGet, whereClauseForChildCollection);

            return qryGet.AsNoTracking();
        }

        public TItem Get<TItem>(object objectId, String[] childCollectionProperties)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;
            dbSet = AttachChildCollection(childCollectionProperties, dbSet);

            return dbSet.Find(objectId);
        }

        public TItem Get<TItem>(object objectId, String[] childCollectionProperties,
            Expression<Func<TItem, bool>> whereClause)
           where TItem : class, new()
        {
            PropertyInfo property = GetDbSet(typeof(TItem));
            DbSet<TItem> dbSet = property.GetValue(_context, null) as DbSet<TItem>;
            IQueryable<TItem> qryGet = AttachChildCollection<TItem>(childCollectionProperties, dbSet);
            List<TItem> lstObject = qryGet.Where(whereClause).AsNoTracking().ToList();

            return lstObject[0];
        }

        public TItem Insert<TItem>(TItem item)
            where TItem : class, new()
        {
            DbSet<TItem> dbSet = GetDbSet(typeof(TItem)).GetValue(_context, null) as DbSet<TItem>;
            dbSet.Add(item);
            _context.SaveChanges();

            return item;
        }

        public TItem Update<TItem>(TItem item)
            where TItem : class, new()
        {
            DbSet<TItem> dbSet = GetDbSet(typeof(TItem)).GetValue(_context, null) as DbSet<TItem>;
            dbSet.Attach(item);
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return item;
        }

        public Boolean Delete<TItem>(TItem item)
           where TItem : class, new()
        {
            try
            {
                DbSet<TItem> set = GetDbSet(typeof(TItem)).GetValue(_context, null) as DbSet<TItem>;
                var entry = _context.Entry(item);

                if (entry != null)
                {
                    entry.State = System.Data.Entity.EntityState.Deleted;
                }
                else
                {
                    set.Attach(item);
                }

                _context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<TItem> ExecuteProcedure<TItem>(string spName, Hashtable keyValuePair)
        {
            object[] array = new SqlParameter[keyValuePair.Count];
            int iLoop = 0;

            foreach (string key in keyValuePair.Keys)
            {
                array[iLoop] = new SqlParameter(parameterName: key, value: keyValuePair[key]);
                iLoop++;
            }

            List<TItem> objItem = _context.ObjectContext.ExecuteStoreQuery<TItem>(spName, array).ToList();

            return objItem;
        }
    }
}
