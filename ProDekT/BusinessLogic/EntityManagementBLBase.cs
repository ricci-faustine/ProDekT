#region Included Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using ProDekT.DataAccess;
using ProDekT.Domain;
#endregion

namespace ProDekT.BusinessLogic
{
	/// <summary>
	/// The base class for all Entity management business logic classes
	/// </summary>
	/// <typeparam name="DomainClass">The type of domain class</typeparam>
	/// <typeparam name="TContext">Type of Db Context</typeparam>
	public class EntityManagementBLBase<DomainClass> : BusinessLogicBase, IEntityManagementBLBase<DomainClass>
		where DomainClass : class, new()
	{
		#region Protected Properties

		#region EntityDAL
		/// <summary>
		/// EntityDAL
		/// </summary>
		protected virtual IDataAccessBase<DomainClass> EntityDAL { get; set; }
		#endregion 

		#endregion

		#region Constructors

		#region EntityManagementBLBase
		/// <summary>
		/// The constructor for EntityManagementBLBase
		/// </summary>
		/// <param name="gdbObject"></param>
		public EntityManagementBLBase()
		{
			this.EntityDAL = new DataAccessBase<DomainClass>();
		}
		#endregion

		#endregion

		#region Protected Methods

		#region MapViewModelToDomain
		/// <summary>
		/// MapViewModelToDomain
		/// </summary>
		/// <param name="viewModelObject"></param>
		/// <param name="domainObject"></param>
		/// <returns></returns>
		protected virtual DomainClass MapViewModelToDomain(ViewModelClass viewModelObject, DomainClass domainObject)
		{
			// Copy properties from view model object to domain object
			Mapper.CreateMap<ViewModelClass, DomainClass>();
			domainObject = Mapper.Map<ViewModelClass, DomainClass>(viewModelObject);

			return domainObject;
		}
		#endregion

		#region MapDomainToViewModel
		/// <summary>
		/// MapDomainToViewModel
		/// </summary>
		/// <param name="domainObject"></param>
		/// <param name="viewModelObject"></param>
		/// <returns></returns>
		protected virtual ViewModelClass MapDomainToViewModel(DomainClass domainObject, ViewModelClass viewModelObject)
		{
			// Copy properties from view model object to domain object
			Mapper.CreateMap<DomainClass, ViewModelClass>();
			viewModelObject = Mapper.Map<DomainClass, ViewModelClass>(domainObject);

			return viewModelObject;
		}
		#endregion

		#region SetDomainGlobalProperties<T>
		/// <summary>
		/// SetDomainGlobalProperties
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="domainObject"></param>
		/// <returns></returns>
		protected virtual T SetDomainGlobalProperties<T>(T domainObject)
		{
			domainObject =
				SetDomainGlobalProperty(domainObject, "CompanyID", this.BllDataBag.ProfileObject.CompanyID);

			if (typeof(DomainClass).FullName != "Zerone.ZeroERP.Domain.AcademicYear")
			{
				domainObject =
					SetDomainGlobalProperty(domainObject, "OperationYearID", this.BllDataBag.OperationYearID);
			}
			
			domainObject =
				SetDomainGlobalProperty(domainObject, "CreationTime", DateTime.Now);
			domainObject =
				SetDomainGlobalProperty(domainObject, "CreatedBy", this.BllDataBag.ProfileObject.UserID);
			domainObject =
				SetDomainGlobalProperty(domainObject, "LastModificationTime", DateTime.Now);
			domainObject =
				SetDomainGlobalProperty(domainObject, "LastModifiedBy", this.BllDataBag.ProfileObject.UserID);

			return domainObject;
		} 
		#endregion

		#region SetDomainGlobalProperty<T>
		/// <summary>
		/// SetDomainGlobalProperty
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="domainObject"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual T SetDomainGlobalProperty<T>(T domainObject, String propertyName, object value)
		{
			IList<PropertyInfo> propertyList = typeof(T).GetProperties().Where(
				item => item.Name == propertyName).ToList();

			if (propertyList != null)
			{
				if (propertyList.Count > 0)
				{
					PropertyInfo globalProperty = propertyList.First();

					globalProperty.SetValue(domainObject, value, null);
				}
			}

			return domainObject;
		} 
		#endregion

		#region GetTransactionScope
		/// <summary>
		/// GetTransactionScope
		/// </summary>
		/// <returns></returns>
		protected virtual TransactionScope GetTransactionScope()
		{
			return new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 15, 30));
		} 
		#endregion 

		#endregion

		#region Public Methods

		#region GetQueryableDomainObjectList - full list
		/// <summary>
		/// GetQueryableDomainObjectList - full list
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		public virtual IQueryable<DomainClass> GetQueryableDomainObjectList(String[] childCollectionProperties)
		{
			return EntityDAL.GetList(childCollectionProperties);
		} 
		#endregion

		#region GetQueryableDomainObjectList - filtered list
		/// <summary>
		/// GetQueryableDomainObjectList - filtered list
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public virtual IQueryable<DomainClass> GetQueryableDomainObjectList(String[] childCollectionProperties, 
			Expression<Func<DomainClass, bool>> whereClause)
		{
			return EntityDAL.GetList(childCollectionProperties, whereClause);
		} 
		#endregion

		#region GetQueryableViewModelObjectList - full list from list of domain objects
		/// <summary>
		/// GetQueryableViewModelObjectList - full list from list of domain objects
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="lstDomain"></param>
		/// <returns></returns>
		/// <remarks>Please make sure that the list of domain objects is small in size.
		/// Otherwise, this will result in performance issues and / or memory issues.</remarks>
		public virtual IQueryable<ViewModelClass> GetQueryableViewModelObjectList(
			String[] childCollectionProperties, IList<DomainClass> lstDomain)
		{
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel.AsQueryable();
		}
		#endregion

		#region GetQueryableViewModelObjectList - full list, paged sorted
		/// <summary>
		/// GetQueryableViewModelObjectList - full list, paged sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public virtual IQueryable<ViewModelClass> GetQueryableViewModelObjectList(
			String[] childCollectionProperties, string sortExpression, string sortDirection, 
			int pageIndex, int pageSize)
		{
			IList<DomainClass> lstDomain =
				EntityDAL.GetList(childCollectionProperties, sortExpression, sortDirection, 
				pageIndex, pageSize).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel.AsQueryable();
		}
		#endregion

		#region GetQueryableViewModelObjectList - filtered list, paged sorted
		/// <summary>
		/// GetQueryableViewModelObjectList - filtered list, paged sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public virtual IQueryable<ViewModelClass> GetQueryableViewModelObjectList(
			String[] childCollectionProperties, Expression<Func<DomainClass, bool>> whereClause, 
			string sortExpression, string sortDirection, int pageIndex, int pageSize)
		{
			IList<DomainClass> lstDomain =
				EntityDAL.GetList(childCollectionProperties, whereClause, sortExpression, sortDirection,
				pageIndex, pageSize).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel.AsQueryable();
		}
		#endregion

		#region GetViewModelObjectList - full list
		/// <summary>
		/// GetViewModelObjectList - full list
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		public virtual List<ViewModelClass> GetViewModelObjectList(String[] childCollectionProperties)
		{
			List<DomainClass> lstDomain = EntityDAL.GetList(childCollectionProperties).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel;
		} 
		#endregion

		#region GetViewModelObjectList - filtered list
		/// <summary>
		/// GetViewModelObjectList - filtered list
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public virtual List<ViewModelClass> GetViewModelObjectList(String[] childCollectionProperties, 
			Expression<Func<DomainClass, bool>> whereClause)
		{
			List<DomainClass> lstDomain = EntityDAL.GetList(childCollectionProperties, whereClause).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel;
		}
		#endregion

		#region GetViewModelObjectList - full list, paged sorted
		/// <summary>
		/// GetViewModelObjectList - full list, paged sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public virtual IList<ViewModelClass> GetViewModelObjectList(String[] childCollectionProperties, 
			string sortExpression, string sortDirection, int pageIndex, int pageSize)
		{
			List<DomainClass> lstDomain =
				EntityDAL.GetList(childCollectionProperties, sortExpression, sortDirection, 
				pageIndex, pageSize).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel;
		} 
		#endregion

		#region GetViewModelObjectList - filtered list, paged sorted
		/// <summary>
		/// GetViewModelObjectList - filtered list, paged sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public virtual IList<ViewModelClass> GetViewModelObjectList(String[] childCollectionProperties, 
			Expression<Func<DomainClass, bool>> whereClause, string sortExpression, string sortDirection,
			int pageIndex, int pageSize)
		{
			List<DomainClass> lstDomain =
				EntityDAL.GetList(childCollectionProperties, whereClause, sortExpression, sortDirection, 
				pageIndex, pageSize).ToList();
			List<ViewModelClass> lstViewModel = new List<ViewModelClass>();

			foreach (DomainClass domainObject in lstDomain)
			{
				ViewModelClass viewModelObject = new ViewModelClass();
				viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);
				lstViewModel.Add(viewModelObject);
			}

			return lstViewModel;
		} 
		#endregion

		#region GetCount
		/// <summary>
		/// GetCount
		/// </summary>
		/// <returns></returns>
		public virtual int GetCount()
		{
			return EntityDAL.GetCount<DomainClass>();
		} 
		#endregion

		#region GetCount - Linq expression as parameter
		/// <summary>
		/// GetCount
		/// </summary>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public virtual int GetCount(Expression<Func<DomainClass, bool>> whereClause)
		{
			return EntityDAL.GetCount<DomainClass>(whereClause);
		}
		#endregion

		#region SaveNew
		/// <summary>
		/// SaveNew
		/// </summary>
		/// <param name="newViewModelObject"></param>
		/// <returns></returns>
		public virtual Boolean SaveNew(ViewModelClass newViewModelObject)
		{
			using (TransactionScope trans = GetTransactionScope())
			{
				DomainClass newDomainObject = new DomainClass();

				newDomainObject = MapViewModelToDomain(newViewModelObject, newDomainObject);
				newDomainObject = SetDomainGlobalProperties<DomainClass>(newDomainObject);

				if (EntityDAL.SaveNew<DomainClass>(newDomainObject))
				{
					trans.Complete();

					return true;
				}
				else
				{
					return false;
				}
			}
		}
		#endregion

		#region GetById
		/// <summary>
		/// GetById
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		public virtual ViewModelClass GetById(int objectId, String[] childCollectionProperties)
		{
			DomainClass domainObject = EntityDAL.GetById<DomainClass>(objectId, childCollectionProperties);
			ViewModelClass viewModelObject = new ViewModelClass();
			viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);

			return viewModelObject;
		} 
		#endregion

		#region GetById - get with child collection
		/// <summary>
		/// GetById - get with child collection
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		public virtual ViewModelClass GetById(int objectId, String[] childCollectionProperties,
			Expression<Func<DomainClass, bool>> whereClause)
		{
			DomainClass domainObject = EntityDAL.GetById<DomainClass>(objectId, 
				childCollectionProperties, whereClause);
			ViewModelClass viewModelObject = new ViewModelClass();
			viewModelObject = MapDomainToViewModel(domainObject, viewModelObject);

			return viewModelObject;
		} 
		#endregion

		#region Update
		/// <summary>
		/// Update
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="modifiedViewModelObject"></param>
		/// <returns></returns>
		public virtual Boolean Update(int objectId, ViewModelClass modifiedViewModelObject)
		{
			using (TransactionScope trans = new TransactionScope())
			{
				DomainClass modifiedDomainObject = new DomainClass();

				modifiedDomainObject = MapViewModelToDomain(modifiedViewModelObject, modifiedDomainObject);
				modifiedDomainObject = SetDomainGlobalProperties(modifiedDomainObject);

				if (EntityDAL.Update<DomainClass>(modifiedDomainObject))
				{
					trans.Complete();

					return true;
				}
				else
				{
					return false;
				}
			}
		} 
		#endregion

		#region Delete - By Id
		/// <summary>
		/// Delete - By Id
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public virtual Boolean Delete(int objectId)
		{
			using (TransactionScope trans = new TransactionScope())
			{
				if (EntityDAL.Delete<DomainClass>(objectId))
				{
					trans.Complete();

					return true;
				}
				else
				{
					return false;
				}
			}
		} 
		#endregion

		#region Delete - By Object
		/// <summary>
		/// Delete - By Object
		/// </summary>
		/// <param name="viewModelObjectToBeDeleted"></param>
		/// <returns></returns>
		public virtual Boolean Delete(ViewModelClass viewModelObjectToBeDeleted)
		{
			using (TransactionScope trans = new TransactionScope())
			{
				DomainClass domainObjectToBeDeleted = new DomainClass();

				domainObjectToBeDeleted = MapViewModelToDomain(viewModelObjectToBeDeleted, domainObjectToBeDeleted);
				domainObjectToBeDeleted = SetDomainGlobalProperties(domainObjectToBeDeleted);

				if (EntityDAL.Delete<DomainClass>(domainObjectToBeDeleted))
				{
					trans.Complete();

					return true;
				}
				else
				{
					return false;
				}
			}
		} 
		#endregion

        #region ExecuteProcedure
        /// <summary>
        /// Execute Stored Procedure from database
        /// </summary>
        /// <typeparam name="DomainClass">DomainClass object</typeparam>
        /// <param name="spName"></param>
        /// <param name="keyValuePair"></param>
        /// <returns></returns>
        public virtual List<DomainClass> ExecuteProcedure(string spName, Hashtable keyValuePair)
        {
            return EntityDAL.ExecuteProcedure(spName, keyValuePair);
        }
        #endregion

		#endregion
	}
}
