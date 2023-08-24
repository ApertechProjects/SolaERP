using AutoMapper;
using SolaERP.Application.Constants;
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
        private readonly IBusinessUnitRepository _bURepository;
        private readonly ISupplierEvaluationRepository _evaluationRepository;
        private readonly IGeneralRepository _generalRepository;

        public RfqService(IUnitOfWork unitOfWork,
                          IRfqRepository repository,
                          IMapper mapper,
                          ISupplierEvaluationRepository evaluationRepository,
                          IBusinessUnitRepository bURepository,
                          IGeneralRepository generalRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
            _bURepository = bURepository;
            _generalRepository = generalRepository;
        }

        public async Task<ApiResponse<RfqSaveCommandResponse>> SaveRfqAsync(RfqSaveCommandRequest request, string useridentity)
        {
            request.UserId = Convert.ToInt32(useridentity);
            RfqSaveCommandResponse response = null;

            if (request.Id <= 0) response = await _repository.AddMainAsync(request);
            else response = await _repository.UpdateMainAsync(request);

            bool result = await _repository.AddDetailsAsync(request.Details, response.Id);
            var saveDetailTasks = request.Details.Select(requestList => _repository.SaveRFqRequestDetailsAsync(requestList.RequestDetails));
            await Task.WhenAll(saveDetailTasks);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<RfqSaveCommandResponse>.Success(response, 200);
        }

        public async Task<ApiResponse<List<RfqAllDto>>> GetAllAsync(RfqAllFilter filter)
        {
            var rfqAlls = await _repository.GetAllAsync(filter);
            var dto = _mapper.Map<List<RfqAllDto>>(rfqAlls);

            return ApiResponse<List<RfqAllDto>>.Success(dto, 200);
        }



        public async Task<ApiResponse<List<BusinessCategory>>> GetBuCategoriesAsync()
        {
            var data = await _generalRepository.BusinessCategories();
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

        public async Task<ApiResponse<bool>> ChangeRFQStatusAsync(RfqChangeStatusModel model, string userIdentity)
        {
            var result = await _repository.ChangeRFQStatusAsync(model, Convert.ToInt32(userIdentity));
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
        }

        public async Task<ApiResponse<RFQMainDto>> GetRFQAsync(string userIdentity, int rfqMainId)
        {
            var mainRFQ = await _repository.GetRFQMainAsync(rfqMainId);
            if (mainRFQ is null)
                return ApiResponse<RFQMainDto>.Fail(ResultMessageConstants.ResourceNotFound, 404);

            var businessCategoriesTask = _generalRepository.BusinessCategories();
            var mainDetailsTask = _repository.GetRFQDetailsAsync(rfqMainId);
            var rfqRequestDetailsTask = _repository.GetRFQLineDeatilsAsync(rfqMainId);
            var rfqSingleReasonsTask = _repository.GetRFQSingeSourceReasons(rfqMainId);
            var businessUnitTask = _bURepository.GetBusinessUnitListByUserId(Convert.ToInt32(userIdentity));

            await Task.WhenAll(mainDetailsTask, rfqRequestDetailsTask, rfqSingleReasonsTask, businessCategoriesTask);

            var mainDetails = mainDetailsTask.Result;
            var rfqRequestDetails = rfqRequestDetailsTask.Result;
            var rfqSingleReasons = rfqSingleReasonsTask.Result;
            var matchedBuCategory = businessCategoriesTask.Result.FirstOrDefault(x => x.Id == mainRFQ.BusinessCategoryId);
            var matchedBU = businessUnitTask.Result.FirstOrDefault(x => x.BusinessUnitId == mainRFQ.BusinessUnitId);

            var mainRFQDto = _mapper.Map<RFQMainDto>(mainRFQ);
            var mainDetailsDto = _mapper.Map<List<RFQDetailDto>>(mainDetails);
            var rfqRequestDetailsDto = _mapper.Map<List<RFQRequestDetailDto>>(rfqRequestDetails);

            mainRFQDto.SingleSourceReasons = rfqSingleReasons;
            mainRFQDto.BusinessCategory = matchedBuCategory;
            mainRFQDto.BusinessUnit = matchedBU;

            var groupedRequestDetails = rfqRequestDetailsDto.GroupBy(requestLine => requestLine.GUID);
            mainDetailsDto.ForEach(detail =>
            {
                var guid = detail.GUID;
                var requestLines = groupedRequestDetails.FirstOrDefault(group => group.Key == guid)?.ToList();
                if (requestLines != null)
                {
                    detail.RequestDetails.AddRange(requestLines);
                }
            });

            mainRFQDto.Details = mainDetailsDto;
            return ApiResponse<RFQMainDto>.Success(mainRFQDto, 200);
        }

        public async Task<ApiResponse<List<RFQInProgressDto>>> GetInProgressAsync(RFQFilterBase filter)
        {
            var inProgressRFQS = await _repository.GetInProgressesAsync(filter);
            var dto = _mapper.Map<List<RFQInProgressDto>>(inProgressRFQS);
            return ApiResponse<List<RFQInProgressDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(List<int> rfqMainIds, string userIdentity)
        {
            var deleteTasks = rfqMainIds.Select(async rfqMainId =>
            {
                var response = await _repository.DeleteMainsync(rfqMainId, Convert.ToInt32(userIdentity));
                return response != null;
            }).ToList();

            await Task.WhenAll(deleteTasks);
            bool allDeleted = deleteTasks.All(task => task.Result);

            await _unitOfWork.SaveChangesAsync();
            return allDeleted ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Fail(false, 400);
        }

        public async Task<ApiResponse<List<UOMDto>>> GetPUOMAsync(int businessUnitId, string itemCodes)
        {
            var puomList = await _repository.GetPUOMAsync(businessUnitId, itemCodes);
            var dto = _mapper.Map<List<UOMDto>>(puomList);

            return ApiResponse<List<UOMDto>>.Success(dto, 200);
        }
    }
}
