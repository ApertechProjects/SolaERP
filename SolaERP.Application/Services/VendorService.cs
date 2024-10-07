using AutoMapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BusinessCategory;
using SolaERP.Application.Dtos.Country;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.DeliveryTerm;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.PaymentTerm;
using SolaERP.Application.Dtos.Score;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Shipment;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.TaxDto;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Dtos.WithHoldingTax;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.ViewModels;
using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace SolaERP.Persistence.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierEvaluationRepository _supplierRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IGeneralRepository _generalRepository;
        private readonly IApproveStageService _approveStageService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMailService _mailService;

        public VendorService(IVendorRepository vendorRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ISupplierEvaluationRepository supplierRepository,
            IUnitOfWork unitOfWork,
            IFileUploadService fileUploadService,
            IGeneralRepository generalRepository,
            IAttachmentService attachmentService,
            IVendorRepository _vendorRepository,
            IApproveStageService approveStageService,
            IEmailNotificationService emailNotificationService,
            IMailService mailService)
        {
            _repository = vendorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _generalRepository = generalRepository;
            _attachmentService = attachmentService;
            _approveStageService = approveStageService;
            _emailNotificationService = emailNotificationService;
            _mailService = mailService;
        }

        public async Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model, HttpResponse response)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

            model.UserId = user.Id;
            var result = await _repository.ApproveAsync(model);
            await _mailService.CheckLastApproveStageAndSendMailToVendor(model.VendorId, model.Sequence, model.ApproveStatusId, response);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(result, 200);
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(VendorStatusModel taxModel, string userIdentity, HttpResponse response)
        {
            var userId = await _userRepository.ConvertIdentity(userIdentity);
            int counter = 0;
            for (int i = 0; i < taxModel.VendorIds.Count; i++)
            {
                var result = await _repository.ChangeStatusAsync(taxModel.VendorIds[i], taxModel.Status, taxModel.Sequence, taxModel.Comment, userId);
                if (result)
                    await _mailService.CheckLastApproveStageAndSendMailToVendor(taxModel.VendorIds[i], taxModel.Sequence, taxModel.Status, response);

            }


            if (counter == taxModel.VendorIds.Count)
                return ApiResponse<bool>.Success(200);
            else
                return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        //private async Task CheckLastApproveStageAndSendMail(int vendorId, int sequence, int approveStatus, HttpResponse response)
        //{
        //    var stageCount = await _approveStageService.GetStageCountAsync(Procedures.Vendors);
        //    if (stageCount == sequence && approveStatus == 1)
        //    {
        //        var vendorUser = await _userRepository.GetUserByVendor(vendorId);
        //        var companyName = await _emailNotificationService.GetCompanyName("");

        //        VM_VendorApprove vendorApprove = new VM_VendorApprove(vendorUser.Language.ToString())
        //        {
        //            CompanyName = companyName,
        //            Language = (Language)Enum.Parse(typeof(Language), vendorUser.Language.ToString())
        //        };


        //        response.OnCompleted(async () =>
        //        {
        //            await _mailService.SendUsingTemplate(GetSubject(vendorUser.Language.ToString()), vendorApprove,
        //            vendorApprove.TemplateName(), null,
        //            new List<string> { vendorUser.Email });
        //        });

        //    }

        //}

        //private string GetSubject(string language)
        //{
        //    return language switch
        //    {
        //        "az" => $@"Vendor təsdiqi",
        //        "en" => $@"Vendor Approve"
        //    };
        //}

        public async Task<ApiResponse<bool>> DeleteAsync(string userIdentity, VendorDeleteModel model)
        {
            int counter = 0;
            for (int i = 0; i < model.Ids.Count; i++)
            {
                var operationResult = await _repository.DeleteAsync(Convert.ToInt32(userIdentity), model.Ids[0]);
                bool isSuccessfull = operationResult != 0;
                if (isSuccessfull)
                    counter++;
            }

            await _unitOfWork.SaveChangesAsync();
            if (counter == model.Ids.Count)
                return ApiResponse<bool>.Success(true, 200);
            else
                return ApiResponse<bool>.Success("some datas can not be deleted");
        }

        public async Task<ApiResponse<bool>> HasVendorName(string vendorName, string userIdentity)
        {
            var userId = Convert.ToInt32(userIdentity);
            return ApiResponse<bool>.Success(await _repository.HasVendorName(vendorName, userId));
        }

        public async Task<ApiResponse<List<VendorRFQListResponseDto>>> GetVendorRFQList(string vendorCode,
            string userIdentity)
        {
            var userId = Convert.ToInt32(userIdentity);
            var result = await _repository.GetVendorRFQList(vendorCode, userId);
            var list = new List<VendorRFQListResponseDto>();

            foreach (var item in result)
            {
                list.Add(new VendorRFQListResponseDto
                {
                    BidMainId = item.BidMainId,
                    RFQMainId = item.RFQMainId,
                    CreatedDate = item.CreatedDate,
                    EnteredBy = item.EnteredBy,
                    LineNo = item.LineNo,
                    ParticipationStatus = item.ParticipationStatus,
                    RespondedDate = item.RespondedDate,
                    SentDate = item.SentDate,
                    DesiredDeliveryDate = item.DesiredDeliveryDate,
                    RFQDate = item.RFQDate,
                    RFQDeadline = item.RFQDeadline,
                    RFQNo = item.RFQNo,
                    RFQType = item.RFQType == 1 ? "Purchasing" : "Information",
                    BusinessCategoryId = GetBusinessCategoryById(item.BusinessCategoryId),
                    Emergency = GetEmergencyById(item.Emergency),
                    RFQStatus = GetRFQStatusById(item.RFQStatus),
                    BiddingType = item.BiddingType
                });
            }

            return ApiResponse<List<VendorRFQListResponseDto>>.Success(list);
        }

        public async Task<ApiResponse<bool>> RFQVendorResponseChangeStatus(int rfqMainId, int status, string vendorCode)
        {
            return ApiResponse<bool>.Success(
                await _repository.RFQVendorResponseChangeStatus(rfqMainId, status, vendorCode)
            );
        }

        public async Task<ApiResponse<bool>> TransferToIntegration(CreateVendorRequest request)
        {
            return ApiResponse<bool>.Success(
                await _repository.TransferToIntegration(request)
            );
        }

        public async Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity,
            VendorAllCommandRequest request)
        {
            List<VendorAll> allVendors = await _repository.GetAll(Convert.ToInt32(userIdentity), request);
            allVendors = allVendors.GetDataByFilter(request.Text);

            List<VendorAllDto> dto = _mapper.Map<List<VendorAllDto>>(allVendors);
            if (dto.Count > 0)
                return ApiResponse<List<VendorAllDto>>.Success(dto, 200);
            return ApiResponse<List<VendorAllDto>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<VendorApprovedDto>>> GetApprovedAsync(string userIdentity, string text)
        {
            var approvedByCurrentUser = await _repository.GetApprovedAsync(Convert.ToInt32(userIdentity));
            approvedByCurrentUser = approvedByCurrentUser.GetDataByFilter(text);

            var dto = _mapper.Map<List<VendorApprovedDto>>(approvedByCurrentUser);
            if (dto.Count > 0)
                return ApiResponse<List<VendorApprovedDto>>.Success(dto, 200);
            return ApiResponse<List<VendorApprovedDto>>.Fail("Data not found", 404);
        }

        public async Task<VendorInfo> GetByTaxAsync(string taxId)
            => await _repository.GetByTaxAsync(taxId);

        public async Task<int> GetByTaxIdAsync(string taxId)
        {
            VendorInfo entity = await _repository.GetByTaxAsync(taxId);
            return entity.VendorId;
        }

        public async Task<ApiResponse<List<VendorDraft>>> GetDraftAsync(string userIdentity, VendorFilter filter)
        {
            var data = await _repository.GetDraftAsync(Convert.ToInt32(userIdentity), filter);
            data = data.GetDataByFilter(filter.Text);
            if (data.Count > 0)
                return ApiResponse<List<VendorDraft>>.Success(data, 200);
            return ApiResponse<List<VendorDraft>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<VM_GetVendorFilters>> GetFiltersAsync()
        {
            var prequalificationTypesTask = _supplierRepository.GetPrequalificationCategoriesAsync();
            var businessCategoriesTask = _generalRepository.BusinessCategories();
            var productServicesTask = _supplierRepository.GetProductServicesAsync();

            await Task.WhenAll(prequalificationTypesTask, businessCategoriesTask, productServicesTask);
            VM_GetVendorFilters viewModel = new()
            {
                PrequalificationCategories = prequalificationTypesTask.Result,
                BusinessCategories = businessCategoriesTask.Result,
                ProductServices = productServicesTask.Result,
            };

            return ApiResponse<VM_GetVendorFilters>.Success(viewModel, 200);
        }

        public async Task<ApiResponse<List<VendorWFADto>>> GetHeldAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetHeldAsync(Convert.ToInt32(userIdentity), filter);
            vendorWFAs = vendorWFAs.GetDataByFilter(filter.Text);

            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);
            if (vendorAllDtos.Count > 0)
                return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);
            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);
        }

        public async Task<ApiResponse<List<VendorWFADto>>> GetRejectedAsync(string userIdentity, VendorFilter filter)
        {
            var rejectedByCurrentUser = await _repository.GetRejectedAsync(Convert.ToInt32(userIdentity), filter);
            rejectedByCurrentUser = rejectedByCurrentUser.GetDataByFilter(filter.Text);
            var rejectedByCurrentUserDto = _mapper.Map<List<VendorWFADto>>(rejectedByCurrentUser);
            if (rejectedByCurrentUserDto.Count > 0)
                return ApiResponse<List<VendorWFADto>>.Success(rejectedByCurrentUserDto, 200);

            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);
        }

        public async Task<VendorLoadDto> GetVendorHeader(int vendorId)
        {
            var header = _mapper.Map<VendorLoadDto>(await _repository.GetHeader(vendorId));
            if (vendorId == 0) header.CompanyRegistrationDate = null;

            header.RevisionVendorId = await _repository.GetRevisionVendorIdByVendorCode(header.VendorCode);
            header.WithHoldingTaxId = header.WithHoldingTaxId == 0 ? null : header.WithHoldingTaxId;
            header.ShipVia = header.ShipVia == 0 ? null : header.ShipVia;
            header.DeliveryTerms = header.DeliveryTerms == 0 ? null : header.DeliveryTerms;
            header.Tax = header.Tax == 0 ? null : header.Tax;

            var logo = await _attachmentService.GetAttachmentsAsync(header.VendorId,
                SourceType.VEN_LOGO, Modules.Vendors);
            if (logo is { Count: > 0 })
            {
                header.Logo = logo[0].FileLink;
                header.AttachmentLogo = logo[0];
            }

            return header;
        }

        public async Task<ApiResponse<VM_VendorCard>> GetAsync(int vendorId)
        {
            var previousVendorId = await GetVendorPreviousVendorId(vendorId);
            var headerPrevious = new VendorLoadDto();
            var bankDetailsPrevious = new List<VendorBankDetail>();
            var vendorBuCategoriesPrevious = new List<VendorBuCategory>();
            var usersPrevious = new List<VendorUser>();
            var scorePrevious = new List<Score>();
            var matchedBuCategoriesPrevious = new List<BusinessCategory>();


            var header = await GetVendorHeader(vendorId);

            var paymentTerms = _supplierRepository.GetPaymentTermsAsync();
            var deliveryTerms = _supplierRepository.GetDeliveryTermsAsync();
            var currency = _supplierRepository.GetCurrenciesAsync();

            var bankDetails = _supplierRepository.GetVendorBankDetailsAsync(vendorId);

            var vendorBuCategories = _supplierRepository.GetVendorBuCategoriesAsync(vendorId);

            var buCategories = _generalRepository.BusinessCategories();
            var users = _supplierRepository.GetVendorUsers(vendorId);
            var score = _supplierRepository.Scores(vendorId);
            var shipment = _supplierRepository.Shipments();
            var withHoldingTax = _supplierRepository.WithHoldingTaxDatas();
            var tax = _supplierRepository.TaxDatas();
            var itemCategories = _generalRepository.BusinessCategories();
            var countries = _supplierRepository.GetCountriesAsync();

            if (previousVendorId > 0)
            {
                headerPrevious = await GetVendorHeader(previousVendorId);
                bankDetailsPrevious = await _supplierRepository.GetVendorBankDetailsAsync(previousVendorId);
                vendorBuCategoriesPrevious = await _supplierRepository.GetVendorBuCategoriesAsync(previousVendorId);
                usersPrevious = await _supplierRepository.GetVendorUsers(previousVendorId);
                scorePrevious = await _supplierRepository.Scores(previousVendorId);
                matchedBuCategoriesPrevious = buCategories.Result
                .Where(x => vendorBuCategoriesPrevious.Select(y => y.BusinessCategoryId).Contains(x.Id))
                .ToList();
            }

            await Task.WhenAll
            (
                paymentTerms,
                deliveryTerms,
                users,
                shipment,
                withHoldingTax,
                bankDetails,
                tax,
                users,
                currency,
                buCategories,
                countries
            );


            var matchedBuCategories = buCategories.Result
                .Where(x => vendorBuCategories.Result.Select(y => y.BusinessCategoryId).Contains(x.Id))
                .ToList();




            VM_VendorCard vendorModel = new VM_VendorCard()
            {
                Header = header,
                HeaderPrevious = headerPrevious,
                ScorePrevious = _mapper.Map<List<ScoreDto>>(scorePrevious),
                VendorBankDetailsPrevious = _mapper.Map<List<VendorBankDetailViewDto>>(bankDetailsPrevious),
                VendorUsersPrevious = _mapper.Map<List<VendorUserDto>>(usersPrevious),
                Currencies = _mapper.Map<List<CurrencyDto>>(currency.Result),
                PaymentTerms = _mapper.Map<List<PaymentTermDto>>(paymentTerms.Result),
                DeliveryTerms = _mapper.Map<List<DeliveryTermDto>>(deliveryTerms.Result),
                VendorBankDetails = _mapper.Map<List<VendorBankDetailViewDto>>(bankDetails.Result),
                VendorUsers = _mapper.Map<List<VendorUserDto>>(users.Result),
                ItemCategories = _mapper.Map<List<BusinessCategoryDto>>(matchedBuCategories),
                ItemCategoriesPrevious = _mapper.Map<List<BusinessCategoryDto>>(matchedBuCategoriesPrevious),
                Score = _mapper.Map<List<ScoreDto>>(score.Result),
                Shipments = _mapper.Map<List<ShipmentDto>>(shipment.Result),
                WithHoldingTaxDatas = _mapper.Map<List<WithHoldingTaxDto>>(withHoldingTax.Result),
                TaxDatas = _mapper.Map<List<TaxDto>>(tax.Result),
                Countries = _mapper.Map<List<CountryDto>>(countries.Result),
            };

            for (int i = 0; i < vendorModel.VendorBankDetails.Count; i++)
            {
                vendorModel.VendorBankDetails[i].AccountVerificationLetter = await _attachmentService.GetAttachmentsAsync(vendorModel.VendorBankDetails[i].Id,
                SourceType.VEN_BNK, Modules.Vendors);
            }


            return ApiResponse<VM_VendorCard>.Success(vendorModel);
        }

        private async Task<int> GetVendorPreviousVendorId(int vendorId)
        {
            var id = await _repository.GetVendorPreviousVendorId(vendorId);
            return id;
        }

        public async Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter)
        {
            List<VendorWFA> vendorWFAs = await _repository.GetWFAAsync(Convert.ToInt32(userIdentity), filter);

            if (!string.IsNullOrEmpty(filter.Text?.ToString())) vendorWFAs = vendorWFAs.GetDataByFilter(filter.Text);

            List<VendorWFADto> vendorAllDtos = _mapper.Map<List<VendorWFADto>>(vendorWFAs);

            if (vendorWFAs.Count > 0) return ApiResponse<List<VendorWFADto>>.Success(vendorAllDtos, 200);

            return ApiResponse<List<VendorWFADto>>.Fail("Data not found", 404);
        }


        public async Task<ApiResponse<bool>> SaveAsync(string userIdentity, VendorCardDto vendorDto)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<Task<bool>> tasks = new();
            Vendor vendor = _mapper.Map<Vendor>(vendorDto);

            vendor.ShipmentId = vendorDto.ShipVia == "null" ? null : Convert.ToInt32(vendorDto.ShipVia);
            vendor.DeliveryTermId = vendorDto.DeliveryTerms == "null" ? null : Convert.ToInt32(vendorDto.DeliveryTerms);
            vendor.WithHoldingTaxId = vendorDto.WithHoldingTaxId == "null"
                ? null
                : Convert.ToInt32(vendorDto.WithHoldingTaxId);
            vendor.TaxesId = vendorDto.Tax == "null" ? null : Convert.ToInt32(vendorDto.Tax);
            vendor.OtherProducts = vendorDto.OtherProducts == "null" ? null : vendorDto.OtherProducts;

            CheckNullForFormData(vendor);

            int userId = Convert.ToInt32(userIdentity);
            int vendorId = 0;

            vendorId = vendor.IsNewVendor()
                ? await _repository.AddAsync(userId, vendor)
                : await _repository.UpdateAsync(userId, vendor);

            await _supplierRepository.DeleteRepresentedProductAsync(vendorId);
            await _supplierRepository.DeleteRepresentedCompanyAsync(vendorId);

            await _supplierRepository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany
            { VendorId = vendorId, RepresentedCompanyName = string.Join(",", vendor?.RepresentedCompanies) });
            await _supplierRepository.AddRepresentedProductAsync(new RepresentedProductData
            { VendorId = vendorId, RepresentedProductName = string.Join(",", vendor?.RepresentedProducts) });

            string vendorLogo = await _repository.GetVendorLogo(vendorId);
            vendor.Logo = await _fileUploadService.GetLinkForEntity(vendorDto.Logo, Modules.Vendors,
                vendorDto.CheckLogoIsDeleted, vendorLogo);


            #region Bank Accounts

            if (vendorDto.BankAccounts is not null)
            {
                for (var i = 0; i < vendorDto.BankAccounts.Count; i++)
                {
                    var x = vendorDto.BankAccounts[i];

                    if (x.Type == 2)
                    {
                        await _repository.DeleteBankDetailsAsync(user.Id, x.Id);
                    }

                    else
                    {
                        x.VendorId = vendorId;

                        var detaildId = await _repository.UpdateBankDetailsAsync(user.Id,
                            _mapper.Map<VendorBankDetail>(x));

                        if (x.AccountVerificationLetter != null)
                        {
                            await _attachmentService.SaveAttachmentAsync(x.AccountVerificationLetter,
                                SourceType.VEN_BNK, detaildId);
                        }
                    }
                }
            }

            #endregion

            await _attachmentService.DeleteAttachmentAsync(vendorId, SourceType.VEN_LOGO);

            if (!vendorDto.CheckLogoIsDeleted)
            {
                AttachmentSaveModel attachmentSaveModel = new AttachmentSaveModel
                {
                    FileLink = vendor.Logo
                };
                await _attachmentService.SaveAttachmentAsync(attachmentSaveModel, SourceType.VEN_LOGO, vendorId);
            }

            await Task.WhenAll(tasks);

            await _unitOfWork.SaveChangesAsync();
            return vendorId > 0 ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Success(false, 400);
        }

        public async Task<ApiResponse<bool>> SaveAsync2(string userIdentity, VendorCardDto2 vendorDto)
        {
            User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
            List<Task<bool>> tasks = new();
            Vendor vendor = _mapper.Map<Vendor>(vendorDto);

            vendor.ShipmentId = vendorDto.ShipVia;
            vendor.DeliveryTermId = vendorDto.DeliveryTerms;
            vendor.WithHoldingTaxId = vendorDto.WithHoldingTaxId;
            vendor.TaxesId = vendorDto.Tax;
            vendor.OtherProducts = vendorDto.OtherProducts == "null" ? null : vendorDto.OtherProducts;

            CheckNullForFormData(vendor);

            int userId = Convert.ToInt32(userIdentity);
            int vendorId = 0;

            vendorId = vendor.IsNewVendor()
                ? await _repository.AddAsync(userId, vendor)
                : await _repository.UpdateAsync(userId, vendor);

            await _supplierRepository.DeleteRepresentedProductAsync(vendorId);
            await _supplierRepository.DeleteRepresentedCompanyAsync(vendorId);

            await _supplierRepository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany
            { VendorId = vendorId, RepresentedCompanyName = string.Join(",", vendor?.RepresentedCompanies) });
            await _supplierRepository.AddRepresentedProductAsync(new RepresentedProductData
            { VendorId = vendorId, RepresentedProductName = string.Join(",", vendor?.RepresentedProducts) });

            #region Company Information Logo
            if (vendorDto.AttachmentLogo != null)
            {
                await _attachmentService.DeleteAttachmentAsync(vendorId, SourceType.VEN_LOGO);

                List<AttachmentSaveModel> attachmentSaveModels = new List<AttachmentSaveModel>();
                attachmentSaveModels.Add(vendorDto.AttachmentLogo);

                await _attachmentService.SaveAttachmentAsync(attachmentSaveModels,
                    SourceType.VEN_LOGO, vendorId);
            }

            #endregion

            #region Bank Accounts

            if (vendorDto.BankAccounts is not null)
            {
                for (var i = 0; i < vendorDto.BankAccounts.Count; i++)
                {
                    var x = vendorDto.BankAccounts[i];

                    if (x.Type == 2)
                    {
                        await _repository.DeleteBankDetailsAsync(user.Id, x.Id);
                    }

                    else
                    {
                        x.VendorId = vendorId;

                        var detaildId = await _repository.UpdateBankDetailsAsync(user.Id,
                            _mapper.Map<VendorBankDetail>(x));

                        if (x.AccountVerificationLetter != null)
                        {
                            await _attachmentService.SaveAttachmentAsync(x.AccountVerificationLetter,
                                SourceType.VEN_BNK, detaildId);
                        }
                    }
                }
            }

            #endregion


            await Task.WhenAll(tasks);

            await _unitOfWork.SaveChangesAsync();
            return vendorId > 0 ? ApiResponse<bool>.Success(true, 200) : ApiResponse<bool>.Success(false, 400);
        }
        public async Task<ApiResponse<bool>> SendToApproveAsync(VendorSendToApproveRequest request)
        {
            bool isSuccessFull = await _repository.SendToApprove(request);
            await _unitOfWork.SaveChangesAsync();

            return isSuccessFull
                ? ApiResponse<bool>.Success(isSuccessFull, 200)
                : ApiResponse<bool>.Success(isSuccessFull, 200);
        }

        public async Task<ApiResponse<List<VendorInfoDto>>> Vendors(string userIdentity)
        {
            int userId = await _userRepository.ConvertIdentity(userIdentity);
            var data = await _repository.Vendors(userId);
            var dto = _mapper.Map<List<VendorInfoDto>>(data);
            return ApiResponse<List<VendorInfoDto>>.Success(dto, 200);
        }

        private static string GetBusinessCategoryById(int id)
        {
            var businessCategories = new List<ItemIdValueDto>
            {
                new() { Id = 1, Value = "IT, Telecom and Utilities" },
                new() { Id = 2, Value = "Transportation and Logistics" },
                new() { Id = 3, Value = "HSE and Well-being, Catering, Safety, PPE" },
                new() { Id = 4, Value = "Production (Drilling, Workover, Geology, Geophysics, Special Software)" },
                new() { Id = 5, Value = "MRO (Maintenance, Repair, Inspection and Construction)" },
                new() { Id = 6, Value = "General (Stationary, Consulting, Courier, Audit)" }
            };

            return businessCategories.SingleOrDefault(x => x.Id == id).Value;
        }

        private static string GetEmergencyById(int id)
        {
            var emergencies = new List<ItemIdValueDto>
            {
                new() { Id = 1, Value = "Low" },
                new() { Id = 2, Value = "Medium" },
                new() { Id = 3, Value = "High" }
            };

            return emergencies.SingleOrDefault(x => x.Id == id).Value;
        }

        private static string GetRFQStatusById(int id)
        {
            var statuses = new List<ItemIdValueDto>
            {
                new() { Id = 0, Value = "Draft" },
                new() { Id = 1, Value = "Open" },
                new() { Id = 2, Value = "Closed" },
                new() { Id = 3, Value = "Canceled" }
            };

            return statuses.SingleOrDefault(x => x.Id == id).Value;
        }

        private void CheckNullForFormData(Vendor vendor)
        {
            vendor.BlackListDescription = string.IsNullOrEmpty(vendor.BlackListDescription) ||
                                          vendor.BlackListDescription == "null"
                ? ""
                : vendor.BlackListDescription;

            vendor.Email = string.IsNullOrEmpty(vendor.Email) || vendor.Email == "null"
                ? ""
                : vendor.Email;

            vendor.Postal = string.IsNullOrEmpty(vendor.Postal) || vendor.Postal == "null"
                ? ""
                : vendor.Postal;

            vendor.Address2 = string.IsNullOrEmpty(vendor.Address2) || vendor.Address2 == "null"
                ? ""
                : vendor.Address2;

            vendor.ContactPerson = string.IsNullOrEmpty(vendor.ContactPerson) || vendor.ContactPerson == "null"
                ? ""
                : vendor.ContactPerson;

            vendor.Description = string.IsNullOrEmpty(vendor.Description) || vendor.Description == "null"
                ? ""
                : vendor.Description;
        }

        public async Task<VendorLoadDto> GetVendorPreviousHeader(int vendorId)
        {
            var data = await _repository.GetVendorPreviousHeader(vendorId);
            var map = _mapper.Map<VendorLoadDto>(data);
            return map;
        }

        public async Task<ApiResponse<List<string>>> CompareVendor(int oldVendorId, int currentVendorId)
        {
            var header = await GetVendorHeader(oldVendorId);
            var headerPrevious = await GetVendorHeader(currentVendorId);
            var data = Compare.CompareRow(header, headerPrevious);
            if (data == null)
            {
                return ApiResponse<List<string>>.Success(data);
            }
            return ApiResponse<List<string>>.Fail(data, 404);
        }
    }
}