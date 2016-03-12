#region Included Namespaces
using System;
using System.Collections.Generic;
#endregion

namespace ProDekT.BusinessLogic
{
	/// <summary>
	/// Interface for entity management BL base
	/// </summary>
	/// <typeparam name="DomainClass">Type of Domain class</typeparam>
	public interface IEntityManagementBLBase<DomainClass>
		where DomainClass : class, new()
	{
		#region Public Methods

		#region Delete - By Id
		/// <summary>
		/// Delete - By Id
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
		bool Delete(int objectId); 
		#endregion

		#region Delete - By Object
		/// <summary>
		/// Delete - By Object
		/// </summary>
		/// <param name="viewModelObjectToBeDeleted"></param>
		/// <returns></returns>
		bool Delete(ViewModelClass viewModelObjectToBeDeleted); 
		#endregion

		#region GetById
		/// <summary>
		/// GetById
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		ViewModelClass GetById(int objectId, string[] childCollectionProperties); 
		#endregion

		#region GetById - Get with child collections
		/// <summary>
		/// GetById - Get with child collections
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		ViewModelClass GetById(int objectId, string[] childCollectionProperties, System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause); 
		#endregion

		#region GetCount
		/// <summary>
		/// GetCount
		/// </summary>
		/// <returns></returns>
		int GetCount(); 
		#endregion

		#region GetCount - Filtered
		/// <summary>
		/// GetCount - Filtered
		/// </summary>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		int GetCount(System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause); 
		#endregion

		#region GetQueryableDomainObjectList - Full list
		/// <summary>
		/// GetQueryableDomainObjectList - Full list
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		System.Linq.IQueryable<DomainClass> GetQueryableDomainObjectList(string[] childCollectionProperties); 
		#endregion

		#region GetQueryableDomainObjectList - Filtered List
		/// <summary>
		/// GetQueryableDomainObjectList - Filtered List
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		System.Linq.IQueryable<DomainClass> GetQueryableDomainObjectList(string[] childCollectionProperties, System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause); 
		#endregion

		#region GetQueryableViewModelObjectList - full list from list of domain objects
		/// <summary>
		/// GetQueryableViewModelObjectList - full list from list of domain objects
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="lstDomain"></param>
		/// <returns></returns>
		System.Linq.IQueryable<ViewModelClass> GetQueryableViewModelObjectList(string[] childCollectionProperties, 
			IList<DomainClass> lstDomain);
		#endregion

		#region GetQueryableViewModelObjectList - Filtered, paged and sorted
		/// <summary>
		/// GetQueryableViewModelObjectList - Filtered, paged and sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		System.Linq.IQueryable<ViewModelClass> GetQueryableViewModelObjectList(string[] childCollectionProperties, System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause, string sortExpression, string sortDirection, int pageIndex, int pageSize); 
		#endregion

		#region GetQueryableViewModelObjectList - Pages and sorted
		/// <summary>
		/// GetQueryableViewModelObjectList - Pages and sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		System.Linq.IQueryable<ViewModelClass> GetQueryableViewModelObjectList(string[] childCollectionProperties, string sortExpression, string sortDirection, int pageIndex, int pageSize); 
		#endregion

		#region GetViewModelObjectList - Full List
		/// <summary>
		/// GetViewModelObjectList - Full List
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <returns></returns>
		System.Collections.Generic.List<ViewModelClass> GetViewModelObjectList(string[] childCollectionProperties); 
		#endregion

		#region GetViewModelObjectList - Filtered
		/// <summary>
		/// GetViewModelObjectList - Filtered
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <returns></returns>
		System.Collections.Generic.List<ViewModelClass> GetViewModelObjectList(string[] childCollectionProperties, System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause); 
		#endregion

		#region GetViewModelObjectList - Filtered, paged and sorted
		/// <summary>
		/// GetViewModelObjectList - Filtered, paged and sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="whereClause"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		System.Collections.Generic.IList<ViewModelClass> GetViewModelObjectList(string[] childCollectionProperties, System.Linq.Expressions.Expression<Func<DomainClass, bool>> whereClause, string sortExpression, string sortDirection, int pageIndex, int pageSize); 
		#endregion

		#region GetViewModelObjectList - Full list, paged and sorted
		/// <summary>
		/// GetViewModelObjectList - Full list, paged and sorted
		/// </summary>
		/// <param name="childCollectionProperties"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		System.Collections.Generic.IList<ViewModelClass> GetViewModelObjectList(string[] childCollectionProperties, string sortExpression, string sortDirection, int pageIndex, int pageSize); 
		#endregion

		#region SaveNew
		/// <summary>
		/// SaveNew
		/// </summary>
		/// <param name="newViewModelObject"></param>
		/// <returns></returns>
		bool SaveNew(ViewModelClass newViewModelObject); 
		#endregion

		#region Update
		/// <summary>
		/// Update
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="modifiedViewModelObject"></param>
		/// <returns></returns>
		bool Update(int objectId, ViewModelClass modifiedViewModelObject); 
		#endregion

		#endregion
	}
}
