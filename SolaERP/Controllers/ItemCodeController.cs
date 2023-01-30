using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCodeController : CustomBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IItemCodeRepository _itemCodeRepository;
        private readonly IMapper _mapper;

        public ItemCodeController(IUnitOfWork unitOfWork, IItemCodeRepository itemCodeRepository, IMapper mapper)
        {
            _itemCodeRepository = itemCodeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemCodesAsync()
        {
            var entity = await _itemCodeRepository.GetAllAsync();
            var dto = _mapper.Map<List<ItemCodeDto>>(entity);

            return CreateActionResult(ApiResponse<List<ItemCodeDto>>.Success(dto, 200));
        }

    }
}
