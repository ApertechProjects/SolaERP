using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Layout;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Layout;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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

        public async Task<ApiResponse<bool>> DeleteLayoutAsync(string name, string key)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);

            var response = await _layoutRepository.DeleteLayoutAsync(userId, key);
            _unitOfWork.SaveChangesAsync();
            return response ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail("Failed to delete user layout", 400);
        }

        public async Task<ApiResponse<LayoutDto>> GetUserLayoutAsync(string name, string layoutKey)
        {
            var entity = await _layoutRepository.GetUserLayoutAsync(await
                 _userRepository.GetIdentityNameAsIntAsync(name), layoutKey);

            var responseContent = _mapper.Map<LayoutDto>(entity);
            return responseContent is not null ? ApiResponse<LayoutDto>.Success(responseContent, 200) : ApiResponse<LayoutDto>.Fail("Failed to load user layout", 400);
        }

        public async Task<ApiResponse<bool>> SaveLayoutAsync(string name, LayoutDto layout)
        {
            var entity = _mapper.Map<Layout>(layout);
            entity.UserId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var response = await _layoutRepository.SaveLayoutAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return response ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail("Failed to save user layout", 400);
        }
    }
}
