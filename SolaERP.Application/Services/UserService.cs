using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Entities.Auth;
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

        public ApiResponse<bool> Register(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            var result = _userRepository.Add(user);

            return ApiResponse<bool>.Success(200);
        }

        public ApiResponse<List<UserDto>> GetAll()
        {
            var users = _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }

        public ApiResponse<bool> UpdateUser(UserDto model)
        {
            User user = _userRepository.GetByUserName(model.UserName);

            var result = _mapper.Map<User>(user);
            _userRepository.Update(result);
            return ApiResponse<bool>.Success(200);
        }

        public ApiResponse<bool> RemoveUser(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            _userRepository.Remove(user);
            return ApiResponse<bool>.Success(200);
        }

    }
}
