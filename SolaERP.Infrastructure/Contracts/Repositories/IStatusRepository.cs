using SolaERP.Infrastructure.Entities.Status;
using SolaERP.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Status = SolaERP.Infrastructure.Entities.Status.Status;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IStatusRepository:ICrudOperations<Status>
    {
    }
}
