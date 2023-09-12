using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Entities.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class GeneralService : IGeneralService
    {
        private readonly IGeneralRepository _generalRepository;
        private IMapper _mapper;

        public GeneralService(IGeneralRepository generalRepository, IMapper mapper)
        {
            _generalRepository = generalRepository;
            _mapper = mapper;
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
            return ApiResponse<BaseAndReportCurrencyRate>.Success(
                new BaseAndReportCurrencyRate() { BaseRate = 1.7m, ReportRate = 2.3m });
        }
    }
}