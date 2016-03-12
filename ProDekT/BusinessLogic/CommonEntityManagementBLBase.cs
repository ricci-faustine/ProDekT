#region Included Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProDekT.DataAccess;
using ProDekT.Domain;
#endregion

namespace ProDekT.BusinessLogic
{
	/// <summary>
	/// The base business logic class for management of common entities like Student, Academic Year etc.
	/// </summary>
	/// <typeparam name="DomainClass">The type of Domain Class</typeparam>
	public class CommonEntityManagementBLBase<DomainClass> : EntityManagementBLBase<DomainClass>, IEntityManagementBLBase<DomainClass>
		where DomainClass : class, new()
	{
		#region Protected Properties

		#region EntityDAL
		/// <summary>
		/// EntityDAL
		/// </summary>
		protected override IDataAccessBase<DomainClass> EntityDAL { get; set; }
		#endregion

		#endregion

		#region Constructors

		#region CommonEntityManagementBLBase
		/// <summary>
		/// The constructor for CommonEntityManagementBLBase
		/// </summary>
		/// <param name="gdbObject"></param>
		public CommonEntityManagementBLBase() : base()
		{
			this.EntityDAL = new DataAccessBase<DomainClass>();
		}
		#endregion

		#endregion
	}
}
