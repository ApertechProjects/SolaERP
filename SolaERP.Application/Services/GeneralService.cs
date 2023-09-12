using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Persistence.Services
{
    public class GeneralService : IGeneralService
    {
        private readonly IGeneralRepository _generalRepository;
        private readonly IMapper _mapper;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public GeneralService(IGeneralRepository generalRepository, IMapper mapper,
            IBusinessUnitRepository businessUnitRepository)
        {
            _generalRepository = generalRepository;
            _mapper = mapper;
            _businessUnitRepository = businessUnitRepository;
        }

        public async Task<ApiResponse<List<BusinessCategory>>> BusinessCategories()
        {
            var status = await _generalRepository.BusinessCategories();
            var dto = _mapper.Map<List<BusinessCategory>>(status);
            return ApiResponse<List<BusinessCategory>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<StatusDto>>> GetStatus()
        {
            var status = await _generalRepository.GetStatus();
            var dto = _mapper.Map<List<StatusDto>>(status);
            return ApiResponse<List<StatusDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<RejectReasonDto>>> RejectReasons()
        {
            var data = await _generalRepository.RejectReasons();
            var dto = _mapper.Map<List<RejectReasonDto>>(data);

            if (dto.Count > 0)
                return ApiResponse<List<RejectReasonDto>>.Success(dto, 200);
            return ApiResponse<List<RejectReasonDto>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<BaseAndReportCurrencyRate>> GetBaseAndReportCurrencyRateAsync(DateTime date,
            string currency, int businessUnitId)
        {
            var convDtoList = await _generalRepository.GetConvRateList(businessUnitId);
            var businessUnit = (await _businessUnitRepository.GetAllAsync())
                .SingleOrDefault(x => x.BusinessUnitId == businessUnitId);

            var singleResultBase = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date
                && x.EffToDateTime >= date
                && x.CurrCodeFrom == businessUnit.BaseCurrencyCode
                && x.CurrCodeTo == currency
            );

            var singleResultReport = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date.Date
                && x.EffToDateTime >= date.Date
                && x.CurrCodeFrom == "AZN "
                && x.CurrCodeTo == currency + " "
            );

            var result = new BaseAndReportCurrencyRate
            {
                BaseRate = 1.7m,
                ReportRate = 2.3m,
                BaseMultiplyOrDivide = 1,
                ReportMultiplyOrDivide = 0
            };

            return ApiResponse<BaseAndReportCurrencyRate>.Success(result);
        }
    }
}