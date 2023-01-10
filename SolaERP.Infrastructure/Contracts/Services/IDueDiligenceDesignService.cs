using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.DueDiligenceDesign;

namespace SolaERP.Infrastructure.Contracts.Services
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
