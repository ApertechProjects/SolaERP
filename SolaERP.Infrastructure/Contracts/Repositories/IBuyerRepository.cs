using SolaERP.Infrastructure.Entities.Buyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IBuyerRepository : ICrudOperations<Buyer>
    {
        public Task<List<Buyer>> GetBuyerByUserTokenAsync(int userId);
    }
}
