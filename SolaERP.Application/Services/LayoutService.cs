using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Layout;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Layout;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;

        public LayoutService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, ILayoutRepository layoutRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _layoutRepository = layoutRepository;
        }

        public async Task<ApiResponse<bool>> DeleteLayoutAsync(string finderToken, LayoutDeleteModel layout)
        {
            var entity = _mapper.Map<Layout>(layout);
            entity.UserId = await _userRepository.GetUserIdByTokenAsync(finderToken);

            var response = await _layoutRepository.DeleteLayoutAsync(entity);
            return response ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail("Failed to delete user layout", 400);
        }

        public async Task<ApiResponse<LayoutDto>> GetUserLayoutAsync(string finderToken, string layoutKey)
        {
            var entity = await _layoutRepository.GetUserLayoutAsync(await
                 _userRepository.GetUserIdByTokenAsync(finderToken), layoutKey);

            var responseContent = _mapper.Map<LayoutDto>(entity);
            return responseContent is not null ? ApiResponse<LayoutDto>.Success(responseContent, 200) : ApiResponse<LayoutDto>.Fail("Failed to load user layout", 400);
        }

        public async Task<ApiResponse<bool>> SaveLayoutAsync(string finderToken, LayoutDto layout)
        {
            var entity = _mapper.Map<Layout>(layout);
            entity.UserId = await _userRepository.GetUserIdByTokenAsync(finderToken);

            var response = await _layoutRepository.SaveLayoutAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return response ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail("Failed to save user layout", 400);
        }
    }
}
