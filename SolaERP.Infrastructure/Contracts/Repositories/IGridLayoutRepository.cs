using SolaERP.Application.Entities.GridLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGridLayoutRepository : ICrudOperations<GridLayout>
    {
        public Task<GridLayout> GetAsync(int userId, string layoutName);

    }
}
