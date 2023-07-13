using SolaERP.Application.Dtos.GridLayout;
using SolaERP.Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGridLayoutService : ICrudService<GridLayoutDto>
    {
        public Task<ApiResponse<GridLayoutDto>> GetAsync(int userId, string layoutName);
    }
}
