using AutoMapper;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Payment;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Enums;
using SolaERP.Persistence.Utils;
using System.Data;
using System.Text;
using System.Text.Json;

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
        private readonly IConfiguration _configuration;
        private readonly IBusinessUnitService _businessUnitService;

        public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository,
            IFileUploadService fileUploadService, IAttachmentService attachmentService, IUnitOfWork unitOfWork,
            IMapper mapper, IConfiguration configuration, IBusinessUnitService businessUnitService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _attachmentService = attachmentService;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
            _businessUnitService = businessUnitService;
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
                var data = await _paymentRepository.ChangeStatus(userId, model.Payments[i].PaymentDocumentMainId,
                    model.Payments[i].Sequence, model.ApproveStatus, model.Comment);
            }

            await _unitOfWork.SaveChangesAsync();
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

        public async Task<ApiResponse<List<CreateOrderDto>>> CreateOrderAsync(CreateOrderModel createOrder)
        {
            var data = await _paymentRepository.CreateOrderAsync(createOrder);
            var dto = _mapper.Map<List<CreateOrderDto>>(data);
            return ApiResponse<List<CreateOrderDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> Delete(PaymentDocumentDeleteModel model, string name)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            for (int i = 0; i < model.paymentDocumentMainIds.Count; i++)
            {
                var result = await _paymentRepository.Delete(model.paymentDocumentMainIds[i], userId);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
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
            var attachments =
                await _attachmentService.GetAttachmentsAsync(paymentDocumentMainId, SourceType.PYMDC, Modules.Payment);
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

        public async Task<ApiResponse<bool>> SendToApprove(string name, List<int> paymentDocumentMainIds)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            foreach (var item in paymentDocumentMainIds)
            {
                var status = await _paymentRepository.SendToApprove(userId, item);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<decimal>> VendorBalance(int businessUnitId, string vendorCode)
        {
            var data = await _paymentRepository.VendorBalance(businessUnitId, vendorCode);
            return ApiResponse<decimal>.Success(data);
        }

        public async Task<ApiResponse<List<WaitingForApprovalDto>>> WaitingForApproval(string name,
            PaymentGetModel payment)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _paymentRepository.WaitingForApproval(userId, payment);
            var dto = _mapper.Map<List<WaitingForApprovalDto>>(data);
            return ApiResponse<List<WaitingForApprovalDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<PaymentRequestDto>>> PaymentRequest(PaymentRequestGetModel model)
        {
            var data = await _paymentRepository.PaymentRequest(model);
            var dto = _mapper.Map<List<PaymentRequestDto>>(data);
            return ApiResponse<List<PaymentRequestDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> PaymentOperation(string name, PaymentOperationModel model,
            PaymentOperations operation)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var table = model.PaymentDocumentMainIds.ConvertListToDataTable();
            var detailResult = await _paymentRepository.PaymentOperation(userId, table, operation);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(detailResult);
        }

        public async Task<ApiResponse<PaymentOrderLoadModel>> PaymentOrderLoad(PaymentOrderParamModel model)
        {
            var dataMain = await _paymentRepository.PaymentOrderMainLoad(model);
            var dtoMain = _mapper.Map<PaymentOrderMainDto>(dataMain);
            dtoMain.EntryDate = dtoMain.EntryDate.ConvertDateToValidDate();
            dtoMain.PaymentDate = dtoMain.PaymentDate.ConvertDateToValidDate();
            var dataDetail = await _paymentRepository.PaymentOrderDetailLoad(model);
            var dtoDetail = _mapper.Map<List<PaymentOrderDetailDto>>(dataDetail);
            foreach (var item in dtoDetail)
            {
                item.PaymentRequestDate = item.PaymentRequestDate.ConvertDateToValidDate();
                item.SentDate = item.SentDate.ConvertDateToValidDate();
            }

            PaymentOrderLoadModel loadModel = new PaymentOrderLoadModel()
            {
                Main = dtoMain,
                Details = dtoDetail
            };
            return ApiResponse<PaymentOrderLoadModel>.Success(loadModel);
        }

        public async Task<ApiResponse<List<PaymentOrderTransactionDto>>> PaymentOrderTransaction(
            PaymentOrderTransactionModel model)
        {
            var table = model.PaymentDocuments.ConvertListOfCLassToDataTable();
            var dataMain = await _paymentRepository.PaymentOrderTransaction(table, model.PaymentOrderMainId,
                model.PaymentDate, model.BankAccount, model.BankCharge);

            var dto = _mapper.Map<List<PaymentOrderTransactionDto>>(dataMain);

            return ApiResponse<List<PaymentOrderTransactionDto>>.Success(dto);
        }

        public async Task<ApiResponse<List<BankAccountListDto>>> BankAccountList(int businessUnitId)
        {
            var data = await _paymentRepository.BankAccountList(businessUnitId);
            var dto = _mapper.Map<List<BankAccountListDto>>(data);
            return ApiResponse<List<BankAccountListDto>>.Success(dto);
        }

        public async Task<ApiResponse<bool>> DeleteAndUpdateAlloc(PaymentOrderPostAudit model, string businessUnitCode)
        {
            using HttpClient client = new HttpClient();
            string json = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await client.PostAsync(
                    _configuration[$"BusinessUnits:{businessUnitCode}"] + "api/v1/a-salfldg/delete-and-update-alloc",
                    content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Problem detected in Step 1!");

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SaveAsalfldgAndPstgAudit(PaymentOrderPostAudit model,
            string businessUnitCode)
        {
            using HttpClient client = new HttpClient();
            string json = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await client.PostAsync(
                    _configuration[$"BusinessUnits:{businessUnitCode}"] +
                    "api/v1/a-salfldg/save-asalfdg-and-pstg-audit",
                    content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Problem detected in Step 2!");

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> SaveAllocations(PaymentOrderPostAudit model, string businessUnitCode)
        {
            using HttpClient client = new HttpClient();
            string json = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await client.PostAsync(
                    _configuration[$"BusinessUnits:{businessUnitCode}"] + "api/v1/a-salfldg/sava-all-locations",
                    content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Problem detected in Step 1!");

            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<PaymentOrderPostDataResult>> PaymentOrderPostData(PaymentOrderPostModel model,
            string name)
        {
            try
            {
                var businessUnits = await _businessUnitService.GetBusinessUnitListConnections();
                var currentBusinessUnitCode =
                    businessUnits.FirstOrDefault(x => x.BusinessUnitId == model.BusinessUnitId);

                var userId = await _userRepository.ConvertIdentity(name);
                DataTable detailData = model.PaymentOrderDetails.ConvertListOfCLassToDataTable();
                var checkNonAllocated = await _paymentRepository.PaymentOrderDetailsCheckNonAllocated(detailData);

                if (checkNonAllocated)
                    return ApiResponse<PaymentOrderPostDataResult>.Success(
                        "Post to SS already created for this data");

                #region Save

                var paymentOrderSaveMain = await _paymentRepository.PaymentOrderPostSaveMain(model.PaymentOrderMain,
                    model.AllocationReference, model.JournalNo, userId);

                var paymentOrderSaveDetail =
                    await _paymentRepository.PaymentOrderPostDetailSave(paymentOrderSaveMain.PaymentOrderMainId,
                        detailData);

                var paymentOrderTransaction = _mapper.Map<List<PaymentTransaction>>(model.PaymentDocumentPosts);

                DataTable transactionData = paymentOrderTransaction.ConvertListOfCLassToDataTable();
                var paymentOrderSaveTransaction =
                    await _paymentRepository.PaymentOrderPostTransactionSave(
                        paymentOrderSaveMain.PaymentOrderMainId,
                        transactionData);

                #endregion

                var auditModel = new PaymentOrderPostAudit()
                {
                    AllocationReference = model.AllocationReference,
                    JournalNo = model.JournalNo
                };

                //API process: 1-2-3
                await DeleteAndUpdateAlloc(auditModel, currentBusinessUnitCode.BusinessUnitCode);

                //API process: 4-5-6-7
                var table = model.PaymentDocumentPosts.ConvertListOfCLassToDataTable();
                var data = await _paymentRepository.PaymentOrderPostData(table,
                    paymentOrderSaveMain.PaymentOrderMainId,
                    model.AllocationReference, model.JournalNo,
                    userId, model.BusinessUnitId);

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<List<ASalfldgDto>>(data.Item1);
                var aSaldldgLadList = new List<ASalfldgLadDto>();

                foreach (var a in data.Item1)
                {
                    aSaldldgLadList.Add(new ASalfldgLadDto
                    {
                        ACCNT_CODE = a.ACCNT_CODE,
                        PERIOD = a.PERIOD,
                        TRANS_DATETIME = a.TRANS_DATETIME,
                        JRNAL_NO = a.JRNAL_NO,
                        JRNAL_LINE = a.JRNAL_LINE,
                        GNRL_DESCR_24 = a.InvoiceNo,
                        GNRL_DESCR_25 = a.Reference,
                        UPDATE_COUNT = 1,
                        LAST_CHANGE_USER_ID = a.LAST_CHANGE_USER_ID,
                        USER_NAME = a.JRNAL_SRCE,
                        INTCO_TYPE = 0,
                        INTCO_PSTG_STATUS = 0,
                        LAST_CHANGE_DATETIME = a.LAST_CHANGE_DATETIME
                    });
                }

                auditModel.ASalfldgs = dto;
                auditModel.AllocationReference = model.AllocationReference;
                auditModel.ASalfldgLads = aSaldldgLadList;
                auditModel.CurrentPeriod = dto[0].ENTRY_PRD ?? 0;
                auditModel.SunUser = dto[0].JRNAL_SRCE ?? null;
                await SaveAsalfldgAndPstgAudit(auditModel, currentBusinessUnitCode.BusinessUnitCode);

                //API process: 8-9
                var allocationData =
                    await _paymentRepository.PaymentOrderAllocationData(paymentOrderSaveMain.PaymentOrderMainId,
                        userId);
                var allocationDataDto = _mapper.Map<List<AllocationDataDto>>(allocationData);

                var allocationLadList = new List<ASalfldgLadDto>();
                foreach (var a in allocationData)
                {
                    if (a.Action == 2)
                    {
                        allocationLadList.Add(new ASalfldgLadDto
                        {
                            ACCNT_CODE = a.ACCNT_CODE,
                            PERIOD = a.PERIOD,
                            TRANS_DATETIME = a.TRANS_DATETIME,
                            JRNAL_NO = a.JRNAL_NO,
                            JRNAL_LINE = a.JRNAL_LINE,
                            GNRL_DESCR_24 = a.GNRL_DESCR_24,
                            GNRL_DESCR_25 = a.GNRL_DESCR_25,
                            UPDATE_COUNT = 1,
                            LAST_CHANGE_USER_ID = auditModel.SunUser,
                            USER_NAME = a.JRNAL_SRCE,
                            INTCO_TYPE = 0,
                            INTCO_PSTG_STATUS = 0,
                            LAST_CHANGE_DATETIME = a.LAST_CHANGE_DATETIME
                        });
                    }
                }

                auditModel.AllocationDatas = allocationDataDto;
                auditModel.AllocationLads = allocationLadList;
                await SaveAllocations(auditModel, currentBusinessUnitCode.BusinessUnitCode);

                return ApiResponse<PaymentOrderPostDataResult>.Success(new PaymentOrderPostDataResult
                {
                    JournalNo = data.Item2,
                    PaymentOrderNo = paymentOrderSaveMain.PaymentOrderNo,
                    PaymentOrderMainId = paymentOrderSaveMain.PaymentOrderMainId,
                }, 200);
            }

            catch (Exception ex)
            {
                return ApiResponse<PaymentOrderPostDataResult>.Fail(ex.Message, 400);
            }
        }

        public async Task<ApiResponse<List<PaymentOrderDto>>> PaymentOrders(PaymentOrderGetModel payment)
        {
            var data = await _paymentRepository.PaymentOrders(payment);
            var dto = _mapper.Map<List<PaymentOrderDto>>(data);
            foreach (var paymentOrderDto in dto)
            {
                paymentOrderDto.Base = Convert.ToDecimal(paymentOrderDto.Base.ToString("0.00"));
                paymentOrderDto.Reporting = Convert.ToDecimal(paymentOrderDto.Reporting.ToString("0.00"));
            }

            return ApiResponse<List<PaymentOrderDto>>.Success(dto);
        }
    }
}