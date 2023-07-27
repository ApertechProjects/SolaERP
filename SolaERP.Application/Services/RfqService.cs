using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;


namespace SolaERP.Persistence.Services
{
    public class RfqService : IRfqService
    {
        private readonly IRfqRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISupplierEvaluationRepository _evaluationRepository;

        public RfqService(IUnitOfWork unitOfWork, IRfqRepository repository, IMapper mapper, ISupplierEvaluationRepository evaluationRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
        }

        public async Task<ApiResponse<int>> SaveRfqAsync(RfqSaveCommandRequest request, string useridentity)
        {
            request.UserId = Convert.ToInt32(useridentity);
            int newMainID = await _repository.AddMainAsync(request);

            bool result = await _repository.AddDetailsAsync(request.RfqDetails, newMainID);
            foreach (var requestList in request.RfqDetails)
            {
                await _repository.SaveRFqRequestDetailsAsync(requestList.LineDetails);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<int>.Success(newMainID, 200);
        }

        public async Task<ApiResponse<List<RfqAllDto>>> GetAllAsync(RfqAllFilter filter)
        {
            var rfqAlls = await _repository.GetAllAsync(filter);
            var dto = _mapper.Map<List<RfqAllDto>>(rfqAlls);

            return ApiResponse<List<RfqAllDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<BusinessCategory>>> GetBuCategoriesAsync()
        {
            var data = await _evaluationRepository.GetBusinessCategoriesAsync();
            return ApiResponse<List<BusinessCategory>>.Success(data, 200);
        }

        public async Task<ApiResponse<List<RfqDraftDto>>> GetDraftsAsync(RfqFilter filter)
        {
            var rfqDrafts = await _repository.GetDraftsAsync(filter);
            var dto = _mapper.Map<List<RfqDraftDto>>(rfqDrafts);

            return ApiResponse<List<RfqDraftDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<RequestRfqDto>>> GetRequestsForRFQ(string userIdentity, RFQRequestModel model)
        {
            model.UserId = Convert.ToInt32(userIdentity);
            var rfqRequests = await _repository.GetRequestsForRfq(model);
            var dto = _mapper.Map<List<RequestRfqDto>>(rfqRequests);
            return ApiResponse<List<RequestRfqDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<SingleSourceReasonModel>>> GetSingleSourceReasonsAsync()
            => ApiResponse<List<SingleSourceReasonModel>>.Success(await _repository.GetSingleSourceReasonsAsync(), 200);

        public async Task<ApiResponse<List<RfqVendor>>> GetRFQVendorsAsync(int buCategoryId)
        {
            var rfqVendors = await _repository.GetVendorsForRfqAync(buCategoryId);
            return ApiResponse<List<RfqVendor>>.Success(rfqVendors, 200);
        }
    }
}
