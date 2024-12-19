using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
	public interface IUpdate
	{
		Task UpdateRFQStatusToClose();
	}
}
