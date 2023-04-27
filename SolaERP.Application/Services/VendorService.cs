using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Venndors;
using SolaERP.Infrastructure.Entities.Vendors;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public VendorService(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public Task AddAsync(Vendors model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<Vendors>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<VendorInfoDto>> GetVendorByTaxIdAsync(string taxId)
        {
            var entity = await _vendorRepository.GetVendorByTaxIdAsync(taxId);
            var dto = _mapper.Map<VendorInfoDto>(entity);
            return ApiResponse<VendorInfoDto>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(Vendors model)
        {
            throw new NotImplementedException();
        }
    }
}
