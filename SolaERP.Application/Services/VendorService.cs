using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Vendors;

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

        public async Task<int> GetVendorByTaxIdAsync(string taxId)
        {
            int entity = await _vendorRepository.GetVendorByTaxIdAsync(taxId);
            return entity;
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(Vendors model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<object>> WaitingForApprovals(int businessUnitId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
