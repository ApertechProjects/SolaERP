using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.UnitOfWork;
using System.Data.Common;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public VendorService(IVendorRepository vendorRepository, IUserRepository userRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _userRepository = userRepository;
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

        public async Task<VendorInfo> GetVendorByTaxAsync(string taxId)
        {
            return await _vendorRepository.GetVendorByTaxAsync(taxId);
        }

        public async Task<int> GetVendorByTaxIdAsync(string taxId)
        {
            VendorInfo entity = await _vendorRepository.GetVendorByTaxAsync(taxId);
            return entity.VendorId;
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(Vendors model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<VendorWFA>>> WaitingForApprovals(string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var data = await _vendorRepository.WaitingForApprovals(userId);
            var dto = _mapper.Map<List<VendorWFA>>(data);
            if (dto.Count > 0)
                return ApiResponse<List<VendorWFA>>.Success(dto);
            return ApiResponse<List<VendorWFA>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<VendorAll>>> All(string userName)
        {
            int userId = await _userRepository.ConvertIdentity(userName);
            var data = await _vendorRepository.All(userId);
            var dto = _mapper.Map<List<VendorAll>>(data);
            if (dto.Count > 0)
                return ApiResponse<List<VendorAll>>.Success(dto);
            return ApiResponse<List<VendorAll>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<VendorInfoDto>>> Vendors(int businessUnitId, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _vendorRepository.Get(businessUnitId, userId);
            var dto = _mapper.Map<List<VendorInfoDto>>(data);
            if (dto.Count > 0)
                return ApiResponse<List<VendorInfoDto>>.Success(dto);
            return ApiResponse<List<VendorInfoDto>>.Fail("Data not found", 404);
        }
    }
}
