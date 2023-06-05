using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BuyerService(IBuyerRepository buyerRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _buyerRepository = buyerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task AddAsync(BuyerDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<BuyerDto>>> GetAllAsync()
        {
            var status = await _buyerRepository.GetAllAsync();
            var dto = _mapper.Map<List<BuyerDto>>(status);
            return ApiResponse<List<BuyerDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BuyerDto>>> GetBuyersAsync(string name, string businessUnitCode)
        {
            var buyer = await _buyerRepository.GetBuyersAsync(await _userRepository.ConvertIdentity(name), businessUnitCode);
            var buyerDto = _mapper.Map<List<BuyerDto>>(buyer);

            if (buyerDto != null)
                return ApiResponse<List<BuyerDto>>.Success(buyerDto, 200);

            return ApiResponse<List<BuyerDto>>.Fail("buyer", "Buyer is empty", 400, true);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(BuyerDto model)
        {
            throw new NotImplementedException();
        }
    }
}
