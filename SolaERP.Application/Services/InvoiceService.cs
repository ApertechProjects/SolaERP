using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;

namespace SolaERP.Persistence.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUserRepository userRepository, IMapper mapper, IInvoiceRepository invoiceRepository,
            IAttachmentService attachmentService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }

        public async Task<ApiResponse<List<RegisterAllDto>>> RegisterAll(InvoiceRegisterGetModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _invoiceRepository.RegisterAll(model, userId);
            var dto = _mapper.Map<List<RegisterAllDto>>(data);
            return ApiResponse<List<RegisterAllDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> RegisterChangeStatus(InvoiceRegisterApproveModel model, string name)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            bool res = false;
            for (int i = 0; i < model.InvoiceRegisterIds.Count; i++)
            {
                await _invoiceRepository.ChangeStatus(model.InvoiceRegisterIds[i].InvoiceRegisterId,
                    model.InvoiceRegisterIds[i].Sequence, model.ApproveStatus, model.Comment, userId);

                if (model.InvoiceRegisterIds[i].InMaxSequence && model.InvoiceRegisterIds[i].InvoiceTypeId == 2)
                {
                    var data = await _invoiceRepository.InvoiceIUD(model.BusinessUnitId, model.InvoiceRegisterIds[i].InvoiceRegisterId, userId);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(res);
        }

        public async Task<ApiResponse<List<RegisterListByOrderDto>>> RegisterListByOrder(int orderMainId)
        {
            var data = await _invoiceRepository.RegisterListByOrder(orderMainId);
            var dto = _mapper.Map<List<RegisterListByOrderDto>>(data);
            return ApiResponse<List<RegisterListByOrderDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<RegisterLoadGRNDto>>> RegisterLoadGRN(int orderMainId)
        {
            var data = await _invoiceRepository.RegisterLoadGRN(orderMainId);
            var dto = _mapper.Map<List<RegisterLoadGRNDto>>(data);
            return ApiResponse<List<RegisterLoadGRNDto>>.Success(dto);
        }

        public async Task<ApiResponse<RegisterMainLoadDto>> Info(int invoiceRegisterId)
        {
            var dataMain = await _invoiceRepository.RegisterMainLoad(invoiceRegisterId);

            var dto = _mapper.Map<RegisterMainLoadDto>(dataMain);
            return ApiResponse<RegisterMainLoadDto>.Success(dto);
        }

        public async Task<ApiResponse<bool>> RegisterSendToApprove(InvoiceSendToApproveModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            bool result = false;
            for (int i = 0; i < model.InvoiceRegisterIds.Count; i++)
            {
                result = await _invoiceRepository.RegisterSendToApprove(model.InvoiceRegisterIds[i], userId);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result);
        }

        public async Task<ApiResponse<List<RegisterWFADto>>> RegisterWFA(InvoiceRegisterGetModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _invoiceRepository.RegisterWFA(model, userId);
            var dto = _mapper.Map<List<RegisterWFADto>>(data);
            return ApiResponse<List<RegisterWFADto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> Save(InvoiceRegisterSaveModel model, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _invoiceRepository.Save(model, userId);

            await _attachmentService.SaveAttachmentAsync(model.Attachments, SourceType.INV, data);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<List<OrderListApprovedDto>>> GetOrderListApproved(int businessUnitId,
            string vendorCode)
        {
            var data = await _invoiceRepository.GetOrderListApproved(businessUnitId, vendorCode);
            var dto = _mapper.Map<List<OrderListApprovedDto>>(data);
            return ApiResponse<List<OrderListApprovedDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<ProblematicInvoiceReasonDto>>> GetProblematicInvoiceReasonList()
        {
            var data = await _invoiceRepository.GetProblematicInvoiceReasonList();
            var dto = _mapper.Map<List<ProblematicInvoiceReasonDto>>(data);
            return ApiResponse<List<ProblematicInvoiceReasonDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> Delete(List<int> ids, string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            for (int i = 0; i < ids.Count; i++)
            {
                var data = await _invoiceRepository.Delete(ids[i], userId);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<List<MatchingMainGRNDto>>> MatchingMainGRN(InvoiceMatchingGRNModel model)
        {
            var data = await _invoiceRepository.MatchingMainGRN(model);
            var dto = _mapper.Map<List<MatchingMainGRNDto>>(data);
            return ApiResponse<List<MatchingMainGRNDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<MatchingMainServiceDto>>> MatchingMainService(InvoiceMatchingGRNModel model)
        {
            var data = await _invoiceRepository.MatchingMainService(model);
            var dto = _mapper.Map<List<MatchingMainServiceDto>>(data);
            return ApiResponse<List<MatchingMainServiceDto>>.Success(dto);
        }

        public async Task<ApiResponse<MatchingMainDto>> GetMatchingMain(int orderMainId)
        {
            var data = await _invoiceRepository.GetMatchingMain(orderMainId);
            var dto = _mapper.Map<MatchingMainDto>(data);
            return ApiResponse<MatchingMainDto>.Success(dto);
        }

        public async Task<ApiResponse<List<InvoiceRegisterDetailForPODto>>> GetDetailsForPO(
            InvoiceGetDetailsModel model)
        {
            var data = await _invoiceRepository.GetDetailsForPO(model);
            var dto = _mapper.Map<List<InvoiceRegisterDetailForPODto>>(data);
            return ApiResponse<List<InvoiceRegisterDetailForPODto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<InvoiceRegisterDetailForOtherDto>>> GetDetailsForOtherOrderTypes(
            InvoiceGetDetailsModel model)
        {
            var data = await _invoiceRepository.GetDetailsForOther(model);
            var dto = _mapper.Map<List<InvoiceRegisterDetailForOtherDto>>(data);
            return ApiResponse<List<InvoiceRegisterDetailForOtherDto>>.Success(dto, 200);
        }

        public async Task<string> GetKeyCode(int orderMainId)
        {
            var data = await _invoiceRepository.GetKeyKode(orderMainId);
            return data;
        }

        public async Task<ApiResponse<List<string>>> GetTransactionReferenceList(int businessUnitId)
        {
            return ApiResponse<List<string>>.Success(
                await _invoiceRepository.GetTransactionReferenceList(businessUnitId));
        }

        public async Task<ApiResponse<List<string>>> GetReferenceList(int businessUnitId)
        {
            return ApiResponse<List<string>>.Success(await _invoiceRepository.GetReferenceList(businessUnitId));
        }

        public async Task<ApiResponse<List<string>>> GetInvoiceList(int businessUnitId)
        {
            return ApiResponse<List<string>>.Success(await _invoiceRepository.GetInvoiceList(businessUnitId));
        }

        public async Task<ApiResponse<List<AdvanceInvoiceDto>>> GetAdvanceInvoicesList(int orderMainId)
        {
            var data = await _invoiceRepository.GetAdvanceInvoicesList(orderMainId);
            var dto = _mapper.Map<List<AdvanceInvoiceDto>>(data);
            return ApiResponse<List<AdvanceInvoiceDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<int>> SaveInvoiceMatchingMain(InvoiceMathcingMain request, int userId)
        {
            var data = await _invoiceRepository.SaveInvoiceMatchingMain(request, userId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<int>.Success(data, 200);
        }

        public async Task<ApiResponse<bool>> SaveInvoiceMatchingGRNs(InvoiceMatchingGRNs request)
        {
            var dataTable = request.RNEInvoicesMatchingTypeList.ConvertListToDataTable();
            await _invoiceRepository.SaveInvoiceMatchingGRNs(request.InvoiceMatchingMainId, dataTable);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SaveInvoiceMatchingAdvances(InvoiceMatchingAdvance request)
        {
            var dataTable = request.AdvanceInvoicesMatchingTypeList.ConvertListToDataTable();
            await _invoiceRepository.SaveInvoiceMatchingAdvances(request.InvoiceRegisterId,
                request.InvoiceMatchingMainid, dataTable);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SaveInvoiceMatchingDetails(InvoiceMatchingDetail request)
        {
            var dataTable = request.InvoicesMatchingDetailsTypeList.ConvertListToDataTable();
            await _invoiceRepository.SaveInvoiceMatchingDetails(request.InvoiceMatchingMainid, dataTable);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }
    }
}