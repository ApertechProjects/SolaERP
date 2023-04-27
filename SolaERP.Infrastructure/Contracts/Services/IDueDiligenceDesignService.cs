using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.DueDiligenceDesign;

namespace SolaERP.Application.Contracts.Services
{
    public class IDueDiligenceDesignService : ICrudService<DueDiligenceDesign>
    {
        public Task AddAsync(DueDiligenceDesign model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<DueDiligenceDesign>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(DueDiligenceDesign model)
        {
            throw new NotImplementedException();
        }
    }
}
