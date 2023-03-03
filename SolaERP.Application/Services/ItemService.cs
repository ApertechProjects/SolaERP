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


        public async Task<ApiResponse<List<ItemCodeDto>>> GetAllAsync(string businessUnitCode)
        {
            var entity = await _itemCodeRepository.GetAllAsync(businessUnitCode);
            var dto = _mapper.Map<List<ItemCodeDto>>(entity);

            return ApiResponse<List<ItemCodeDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<ItemCodeWithImagesDto>> GetItemCodeByItemCodeAsync(string businessUnitCode, string itemCode)
        {
            var itemCodes = await _itemCodeRepository.GetItemCodeByItemCodeAsync(businessUnitCode, itemCode);
            var itemCodeResult = _mapper.Map<ItemCodeWithImagesDto>(itemCodes);

            return itemCodeResult != null ? ApiResponse<ItemCodeWithImagesDto>.Success(itemCodeResult, 200) :
                ApiResponse<ItemCodeWithImagesDto>.Fail("Bad request from GetItemCodesByItemCode", 404);
        }

        public async Task<ApiResponse<ItemCodeInfoDto>> GetItemCodeInfoByItemCodeAsync(string itemCode)
        {
            var itemCodes = await _itemCodeRepository.GetItemCodeInfoByItemCodeAsync(itemCode);
            var itemCodeResult = _mapper.Map<ItemCodeInfoDto>(itemCodes);

            return itemCodeResult != null ? ApiResponse<ItemCodeInfoDto>.Success(itemCodeResult, 200) :
                ApiResponse<ItemCodeInfoDto>.Fail("Bad request from GetItemCodesByItemCode", 404);
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
