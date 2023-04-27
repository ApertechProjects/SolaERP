using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBuyerRepository : ICrudOperations<Buyer>
    {
        public Task<List<Buyer>> GetBuyersAsync(int userId, string businessUnitCode);
       
    }
}
