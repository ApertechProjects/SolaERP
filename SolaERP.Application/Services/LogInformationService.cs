using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.LogInfo;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class LogInformationService : ILogInformationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogInformationRepository _logInfoRepository;

        public LogInformationService(IMapper mapper, IUnitOfWork unitOfWork, ILogInformationRepository logInfoRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logInfoRepository = logInfoRepository;
        }

        public async Task<ApiResponse<List<LogInfoDto>>> GetAllLogInformationAsync(LogInfoGetDto logGetparameters)
        {
            var entity = await _logInfoRepository.GetAllLogInformationAsync(logGetparameters.Id, logGetparameters.LogType);
            var dto = _mapper.Map<List<LogInfoDto>>(entity);

            return dto.Count > 0 ? ApiResponse<List<LogInfoDto>>.Success(dto, 200) :
                ApiResponse<List<LogInfoDto>>.Fail("Something went wrong", 400);

        }

        public async Task<ApiResponse<LogInfoDto>> GetSingleLogInformationAsync(LogInfoGetDto logGetparameters)
        {
            var entity = await _logInfoRepository.GetSingleLogInformationAsync(logGetparameters.Id, logGetparameters.LogType);
            var dto = _mapper.Map<LogInfoDto>(entity);

            return dto != null ? ApiResponse<LogInfoDto>.Success(dto, 200) :
                ApiResponse<LogInfoDto>.Fail("Something went wrong", 400);
        }
    }
}
