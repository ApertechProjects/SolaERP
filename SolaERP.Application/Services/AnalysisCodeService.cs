using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class AnalysisCodeService : IAnalysisCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAnalysisCodeRepository _analysisCodeRepository;

        public AnalysisCodeService(IMapper mapper, IUnitOfWork unitOfWork, IAnalysisCodeRepository analysisCodeRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _analysisCodeRepository = analysisCodeRepository;
        }

        public async Task<ApiResponse<List<AnalysisCodeDto>>> GetAnalysisCodesAsync(AnalysisCodeGetModel getRequest)
        {
            var analysisCodes = await _analysisCodeRepository.GetAnalysisCodesAsync(getRequest.Businessunitid, getRequest.ProcedureName, getRequest.Sequence);
            var analysisCodeResult = _mapper.Map<List<AnalysisCodeDto>>(analysisCodes);

            return analysisCodeResult.Count > 0 ? ApiResponse<List<AnalysisCodeDto>>.Success(analysisCodeResult, 200) :
                 ApiResponse<List<AnalysisCodeDto>>.Success(new List<AnalysisCodeDto>(), 200);
        }
    }
}
