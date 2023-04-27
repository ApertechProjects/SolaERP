using SolaERP.Application.Entities.Status;
using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Status = SolaERP.Application.Entities.Status.Status;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IStatusRepository:ICrudOperations<Status>
    {
    }
}
