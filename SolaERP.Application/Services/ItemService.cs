using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Item_Code;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemCodeRepository _itemCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IItemCodeRepository itemCodeRepository, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _itemCodeRepository = itemCodeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ItemCodeDto>>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC)
        {
            var ttt = await _itemCodeRepository.ExecQueryWithReplace(sqlElement, paramListsR, paramListsC);
            var dto = _mapper.Map<List<ItemCodeDto>>(ttt);
            return ApiResponse<List<ItemCodeDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ItemCodeDto>>> GetAllAsync()
        {
            var entity = await _itemCodeRepository.GetAllAsync();
            var dto = _mapper.Map<List<ItemCodeDto>>(entity);

            return ApiResponse<List<ItemCodeDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<ItemCodeDto>> GetItemCodeByItemCodeAsync(string itemCode)
        {
            var itemCodes = await _itemCodeRepository.GetItemCodeByItemCodeAsync(itemCode);
            var itemCodeResult = _mapper.Map<ItemCodeDto>(itemCodes);

            return itemCodeResult != null ? ApiResponse<ItemCodeDto>.Success(itemCodeResult, 200) :
                ApiResponse<ItemCodeDto>.Fail("Bad request from GetItemCodesByItemCode", 404);
        }

        public async Task<ApiResponse<List<ItemCodeWithImagesDto>>> GetItemCodesWithImagesAsync()
        {
            var itemCodes = await _itemCodeRepository.GetItemCodesWithImagesAsync();
            var itemCodeResult = _mapper.Map<List<ItemCodeWithImagesDto>>(itemCodes);

            return itemCodeResult.Count > 0 ? ApiResponse<List<ItemCodeWithImagesDto>>.Success(itemCodeResult, 200) :
                ApiResponse<List<ItemCodeWithImagesDto>>.Fail("Bad request from GetItemCodesWithImages", 404);
        }
    }
}
