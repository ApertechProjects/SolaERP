using SolaERP.Infrastructure.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILocationRepository : ICrudOperations<Location>
    {
    }
}
