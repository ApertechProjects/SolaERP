using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
	public class Update : IUpdate
	{
		private readonly IUnitOfWork _unitOfWork;
		public Update(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task UpdateRFQStatusToClose()
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @$"set nocount off update Procurement.RFQMain set Status = 2 where RFQDeadline<getDate() and RFqMAinId = 2453";

				var res = command.ExecuteNonQuery() > 0;

			}
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
