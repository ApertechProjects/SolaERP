using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.LogInfo;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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

        public async Task<ApiResponse<List<LogInfoDto>>> GetAllLogInformationAsync(LogInfoGetModel logGetparameters)
        {
            var entity = await _logInfoRepository.GetAllLogInformationAsync(logGetparameters.Id, logGetparameters.LogType);
            var dto = _mapper.Map<List<LogInfoDto>>(entity);

            return dto.Count > 0 ? ApiResponse<List<LogInfoDto>>.Success(dto, 200) :
                ApiResponse<List<LogInfoDto>>.Fail("No log information found with the specified parameters.", 404);
        }
    }
}
