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

        public async Task<ApiResponse<List<RejectReasonDto>>> RejectReasonsForInvoice()
        {
            var data = await _generalRepository.RejectReasonsForInvoice();
            var dto = _mapper.Map<List<RejectReasonDto>>(data);

            if (dto.Count > 0)
                return ApiResponse<List<RejectReasonDto>>.Success(dto, 200);
            return ApiResponse<List<RejectReasonDto>>.Fail("Data not found", 404);
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
            //{06.12.2023 15:25:38}
            //{06.12.2023 0:00:00}
            date = date.Date;
            var convDtoList = await _generalRepository.GetConvRateList(businessUnitId);
            var businessUnit = (await _businessUnitRepository.GetAllAsync())
                .SingleOrDefault(x => x.BusinessUnitId == businessUnitId);

            var singleResultBase = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date
                && x.EffToDateTime >= date
                && x.CurrCodeFrom == currency + "  "
                && x.CurrCodeTo == businessUnit.BaseCurrencyCode + "  "
            );

            var singleResultReport = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date
                && x.EffToDateTime >= date
                && x.CurrCodeFrom == businessUnit.BaseCurrencyCode + "  "
                && x.CurrCodeTo == businessUnit.ReportingCurrencyCode + "  "
            );

            if (singleResultBase is null || singleResultReport is null)
            {
                var dateStringFormatted = date.ToString("dd/MM/yyyy");
                string message =
                    $"There is no currency rate at date {dateStringFormatted}, please contact the finance department.";
                return ApiResponse<BaseAndReportCurrencyRate>.Fail(message, 444);
            }

            var result = new BaseAndReportCurrencyRate
            {
                BaseRate = singleResultBase.ConvRate,
                ReportRate = singleResultReport.ConvRate,
                BaseMultiplyOrDivide = singleResultBase.MultiplyDivide,
                ReportMultiplyOrDivide = singleResultReport.MultiplyDivide,
                IsReportEqualsDisCount = currency == businessUnit.ReportingCurrencyCode
            };

            return ApiResponse<BaseAndReportCurrencyRate>.Success(result);
        }
    }
}