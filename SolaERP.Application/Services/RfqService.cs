using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SolaERP.Persistence.Services
{
    public class RfqService : IRfqService
    {
        private readonly IRfqRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISupplierEvaluationRepository _evaluationRepository;

        public RfqService(IUnitOfWork unitOfWork, IRfqRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter)
        {
            var rfqDrafts = await _repository.GetDraftsAsync(filter);
            var dto = _mapper.Map<List<RfqDraftDto>>(rfqDrafts);

            return ApiResponse<List<RfqDraftDto>>.Success(dto, 200);
        }
    }
}
