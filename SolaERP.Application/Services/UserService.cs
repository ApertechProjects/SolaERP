using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;

namespace SolaERP.Application.Services
{
    public class UserService : IBaseService<UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ApiResponse<UserDto> Register(UserDto model)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<List<UserDto>> GetAll()
        {
            var users = _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }
    }
}
