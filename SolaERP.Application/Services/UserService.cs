using AutoMapper;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;

namespace SolaERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IDbConnection _connection;
        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
        }

        public async Task<ApiResponse<Token>> AddAsync(UserDto model)
        {
            model.PasswordHash = Utils.SecurityUtil.ComputeSha256Hash(model.PasswordHash);
            var user = _mapper.Map<User>(model);
            Guid guid = Guid.NewGuid();
            user.UserToken = guid;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            var result = _userRepository.Add(user);

            _unitOfWork.SaveChanges();

            return ApiResponse<Token>.Success(await _tokenHandler.GenerateJwtTokenAsync(1), 200);
        }

        public ApiResponse<List<UserDto>> GetAll()
        {
            var users = _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> UpdateUser(UserDto model)
        {
            User user = await _userRepository.GetByUserNameAsync(model.UserName);

            var result = _mapper.Map<User>(user);
            _userRepository.Update(result);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public ApiResponse<bool> RemoveUser(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            _userRepository.Remove(user);

            _unitOfWork.SaveChanges();
            return ApiResponse<bool>.Success(200);
        }

    }
}
