using AutoMapper;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using System.Data;

namespace SolaERP.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IAttachmentService _attachmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository, IFileUploadService fileUploadService, IAttachmentService attachmentService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _attachmentService = attachmentService;
            _fileUploadService = fileUploadService;
        }

        public async Task<ApiResponse<List<AllDto>>> All(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.All(userId, payment);
            var dto = _mapper.Map<List<AllDto>>(data);
            return ApiResponse<List<AllDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<ApprovedDto>>> Approved(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.Approved(userId, payment);
            var dto = _mapper.Map<List<ApprovedDto>>(data);
            return ApiResponse<List<ApprovedDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<AttachmentDto>>> Attachments(int paymentDocumentMainId)
        {
            var data = await _paymentRepository.InfoAttachments(paymentDocumentMainId);
            var dto = _mapper.Map<List<AttachmentDto>>(data);
            return ApiResponse<List<AttachmentDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<BankDto>>> Bank(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.Bank(userId, payment);
            var dto = _mapper.Map<List<BankDto>>(data);
            return ApiResponse<List<BankDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> ChangeStatus(string name, PaymentChangeStatusModel model)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            for (int i = 0; i < model.Payments.Count; i++)
            {
                var data = await _paymentRepository.ChangeStatus(userId, model.Payments[i].PaymentDocumentMainId, model.Payments[i].Sequence, model.ApproveStatus);
            }
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<List<CreateAdvanceDto>>> CreateAdvanceAsync(CreateAdvanceModel createAdvance)
        {
            var data = await _paymentRepository.CreateAdvanceAsync(createAdvance);
            var dto = _mapper.Map<List<CreateAdvanceDto>>(data);
            return ApiResponse<List<CreateAdvanceDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<CreateBalanceDto>>> CreateBalanceAsync(CreateBalanceModel createBalance)
        {
            var data = await _paymentRepository.CreateBalanceAsync(createBalance);
            var dto = _mapper.Map<List<CreateBalanceDto>>(data);
            return ApiResponse<List<CreateBalanceDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<CreateDocumentDto>>> CreateDocument(PaymentCreateDocumentModel model)
        {
            var data = await _paymentRepository.CreateDocument(model);
            var dto = _mapper.Map<List<CreateDocumentDto>>(data);
            return ApiResponse<List<CreateDocumentDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<CreateOrderDto>>> CreateOrderAsync(CreateOrderModel createOrder)
        {
            var data = await _paymentRepository.CreateOrderAsync(createOrder);
            var dto = _mapper.Map<List<CreateOrderDto>>(data);
            return ApiResponse<List<CreateOrderDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> Delete(int paymentDocumentMainId)
        {
            var result = await _paymentRepository.Delete(paymentDocumentMainId);
            if (result)
                return ApiResponse<bool>.Success(200);
            return ApiResponse<bool>.Fail("Data can not be deleted", 400);
        }

        public async Task<ApiResponse<List<DraftDto>>> Draft(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.Draft(userId, payment);
            var dto = _mapper.Map<List<DraftDto>>(data);
            return ApiResponse<List<DraftDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<HeldDto>>> Held(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.Held(userId, payment);
            var dto = _mapper.Map<List<HeldDto>>(data);
            return ApiResponse<List<HeldDto>>.Success(dto);
        }



        public async Task<ApiResponse<PaymentInfoModel>> Info(int paymentDocumentMainId)
        {
            var header = await _paymentRepository.InfoHeader(paymentDocumentMainId);
            var headerDto = _mapper.Map<InfoHeaderDto>(header);
            var details = await _paymentRepository.InfoDetail(paymentDocumentMainId);
            var detailsDto = _mapper.Map<List<InfoDetailDto>>(details);
            var approvalInformation = await _paymentRepository.InfoApproval(paymentDocumentMainId);
            var approvalDto = _mapper.Map<List<InfoApproval>>(approvalInformation);
            var documentTypes = DocumentTypes().Data;
            var attachmentTypes = await _paymentRepository.AttachmentTypes();
            var attachments = await _attachmentService.GetAttachmentsAsync(paymentDocumentMainId, SourceType.PYMDC, Modules.Payment);
            foreach (var item in approvalDto)
            {
                item.UserPhoto = _fileUploadService.GetFileLink(item.UserPhoto, Modules.Users);
                item.SignaturePhoto = _fileUploadService.GetFileLink(item.SignaturePhoto, Modules.Users);
            }

            var attachmentSubTypes = await _paymentRepository.InfoAttachments(paymentDocumentMainId);
            PaymentLink link = new PaymentLink();
            link.RFQLinks = await _paymentRepository.RFQLinks(paymentDocumentMainId);
            link.InvoiceLinks = await _paymentRepository.InvoiceLinks(paymentDocumentMainId);
            link.OrderLinks = await _paymentRepository.OrderLinks(paymentDocumentMainId);
            link.RequestLinks = await _paymentRepository.RequestLinks(paymentDocumentMainId);
            link.BidLinks = await _paymentRepository.BidLinks(paymentDocumentMainId);
            link.BidComparisonLinks = await _paymentRepository.BidComparisonLinks(paymentDocumentMainId);

            return ApiResponse<PaymentInfoModel>.Success(new PaymentInfoModel
            {
                Approval = approvalDto,
                Detail = detailsDto,
                Header = headerDto,
                DocumentTypes = documentTypes,
                AttachmentTypes = attachmentTypes,
                AttachmentSubTypes = attachmentSubTypes,
                PaymentLink = link,
                Attachments = attachments
            });
        }

        public ApiResponse<List<AttachmentTypes>> DocumentTypes()
        {
            var paymentAttachmentTypes = Enum.GetNames(typeof(PaymentAttachmentTypes));
            List<AttachmentTypes> types = new List<AttachmentTypes>();
            int i = 0;
            foreach (var item in paymentAttachmentTypes)
            {
                types.Add(new AttachmentTypes { TypeId = i, TypeName = item.ToString() });
                i++;
            }
            return ApiResponse<List<AttachmentTypes>>.Success(types, 200);
        }

        public async Task<ApiResponse<List<RejectedDto>>> Rejected(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.Rejected(userId, payment);
            var dto = _mapper.Map<List<RejectedDto>>(data);
            return ApiResponse<List<RejectedDto>>.Success(dto);
        }

        public async Task<ApiResponse<PaymentDocumentSaveResultModel>> Save(string name, PaymentDocumentSaveModel model)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var mainResult = await _paymentRepository.MainSave(userId, model.Main);
            if (mainResult != null)
            {
                foreach (var item in model.Details)
                    item.PaymentDocumentMainId = mainResult.PaymentDocumentMainId;

                DataTable data = model.Details.ConvertListOfCLassToDataTable();
                var detailResult = await _paymentRepository.DetailSave(data);
            }

            model.Attachments.ForEach(attachment =>
            {
                if (attachment.Type == 2)
                {
                    if (attachment.AttachmentId > 0)
                    {
                        _attachmentService.DeleteAttachmentAsync(attachment.AttachmentId).Wait();
                    }
                }
                else
                {
                    if (attachment.AttachmentId > 0) return;
                    attachment.SourceId = mainResult.PaymentDocumentMainId;
                    attachment.SourceType = SourceType.PYMDC.ToString();
                    _attachmentService.SaveAttachmentAsync(attachment).Wait();
                }
            });

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<PaymentDocumentSaveResultModel>.Success(new PaymentDocumentSaveResultModel
            {
                PaymentDocumentMainId = mainResult.PaymentDocumentMainId,
                PaymentRequestNo = mainResult.PaymentRequestNo
            });
        }

        public async Task<ApiResponse<bool>> SendToApprove(string name, int paymentDocumentMainId)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var result = await _paymentRepository.SendToApprove(userId, paymentDocumentMainId);
            return result ? ApiResponse<bool>.Success(200) : ApiResponse<bool>.Fail("Problem detected. Payment can not be send to approvals", 400);
        }

        public async Task<ApiResponse<decimal>> VendorBalance(int businessUnitId, string vendorCode)
        {
            var data = await _paymentRepository.VendorBalance(businessUnitId, vendorCode);
            return ApiResponse<decimal>.Success(data);
        }

        public async Task<ApiResponse<List<WaitingForApprovalDto>>> WaitingForApproval(string name, PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.WaitingForApproval(userId, payment);
            var dto = _mapper.Map<List<WaitingForApprovalDto>>(data);
            return ApiResponse<List<WaitingForApprovalDto>>.Success(dto);
        }
    }
}
