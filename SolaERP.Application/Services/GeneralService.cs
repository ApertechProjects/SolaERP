using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Persistence.Utils;
using System.Collections.Generic;

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
            dto.ForEach(x => { x.Code = x.Code.Trim(); });
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

            var singleResultBaseResult = await CurrencyOps.GetConvRateDtoAsync(convDtoList, date, currency, businessUnit);

            var singleResultReport = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date
                && x.EffToDateTime >= date
                && x.CurrCodeFrom == businessUnit.BaseCurrencyCode + "  "
                && x.CurrCodeTo == businessUnit.ReportingCurrencyCode + "  "
            );

            if (singleResultBaseResult is null || singleResultReport is null)
            {
                var dateStringFormatted = date.ToString("dd/MM/yyyy");
                string message =
                    $"There is no currency rate at date {dateStringFormatted}, please contact the finance department.";
                return ApiResponse<BaseAndReportCurrencyRate>.Fail(message, 444);
            }

            var result = new BaseAndReportCurrencyRate
            {
                BaseRate = singleResultBaseResult.ConvRate,
                ReportRate = singleResultReport.ConvRate,
                BaseMultiplyOrDivide = singleResultBaseResult.MultiplyDivide,
                ReportMultiplyOrDivide = singleResultReport.MultiplyDivide,
                IsReportEqualsDisCount = currency == businessUnit.ReportingCurrencyCode
            };

            return ApiResponse<BaseAndReportCurrencyRate>.Success(result);
        }

        public async Task<ApiResponse<List<BaseAndReportCurrencyRates>>> GetBaseAndReportCurrencyRatesAsync(DateTime date, int businessUnitId)
        {
            //{06.12.2023 15:25:38}
            //{06.12.2023 0:00:00}
            date = date.Date;
            var convDtoList = await _generalRepository.GetConvRateList(businessUnitId);
            var businessUnit = (await _businessUnitRepository.GetAllAsync())
                .SingleOrDefault(x => x.BusinessUnitId == businessUnitId);

            var resultBaseResult = await CurrencyOps.GetConvRateDtoAsync(convDtoList, date, businessUnit);

            var resultReport = convDtoList.SingleOrDefault(x =>
                x.EffFromDateTime <= date
                && x.EffToDateTime >= date
                && x.CurrCodeFrom == businessUnit.BaseCurrencyCode + "  "
                && x.CurrCodeTo == businessUnit.ReportingCurrencyCode + "  "
            );

            if (resultBaseResult is null || resultReport is null)
            {
                var dateStringFormatted = date.ToString("dd/MM/yyyy");
                string message =
                    $"There is no currency rate at date {dateStringFormatted}, please contact the finance department.";
                return ApiResponse<List<BaseAndReportCurrencyRates>>.Fail(message, 444);
            }

            List<BaseAndReportCurrencyRates> list = new List<BaseAndReportCurrencyRates>();
            foreach (var item in resultBaseResult)
            {
                list.Add(new BaseAndReportCurrencyRates
                {
                    BaseRate = item.ConvRate,
                    ReportRate = resultReport.ConvRate,
                    BaseMultiplyOrDivide = item.MultiplyDivide,
                    ReportMultiplyOrDivide = item.MultiplyDivide,
                    CurrCodeFrom = item.CurrCodeFrom,
                    CurrCodeTo = item.CurrCodeTo,
                    IsReportEqualsDisCount = item.CurrCodeFrom.Trim() == businessUnit.ReportingCurrencyCode
                });
            }

            return ApiResponse<List<BaseAndReportCurrencyRates>>.Success(list);
        }

        public async Task<bool> DailyCurrencyIsExist(DateTime date, string currency, int businessUnitId)
        {
            var convDtoList = await _generalRepository.GetConvRateList(businessUnitId);

            var businessUnit = (await _businessUnitRepository.GetAllAsync())
                .SingleOrDefault(x => x.BusinessUnitId == businessUnitId);

            var result = await CurrencyOps.GetConvRateDtoAsync(convDtoList, date, currency, businessUnit);

            if (result is null)
                return false;

            return true;
        }


        public async Task<RejectReasonDto> GetRejectReasonByCode(string code)
        {
            var rejectReason = await _generalRepository.GetRejectReasonByCode(code);
            var dto = _mapper.Map<RejectReasonDto>(rejectReason);
            return dto;
        }


        public async Task<string> ReasonCode(int rejectReasonId)
        {
            var rejectReason = await _generalRepository.ReasonCode(rejectReasonId);
            return rejectReason;
        }
    }
}