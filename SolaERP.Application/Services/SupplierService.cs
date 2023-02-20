using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Supplier;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<SupplierCodeDto>>> GetSupplierCodesAsync()
        {
            var suppCodes = await _supplierRepository.GetSupplierCodesAsync();
            var resultList = _mapper.Map<List<SupplierCodeDto>>(suppCodes);

            return resultList.Count > 0 ? ApiResponse<List<SupplierCodeDto>>.Success(resultList, 200) : ApiResponse<List<SupplierCodeDto>>.Success(new(), 200);
        }
    }
}
