using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Repositories
{
    public interface IGroupRepository : ICrudOperations<Groups>
    {
    }
}
