using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.TaxDto;
using SolaERP.Application.Dtos.WithHoldingTax;
using SolaERP.Application.Entities.Attachment;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.Invoice;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;
using System.Xml.Linq;
using InvoiceRegisterDetails = SolaERP.Application.Entities.Invoice.InvoiceRegisterDetails;

namespace SolaERP.Persistence.Services
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IAttachmentService _attachmentService;
		private readonly ISupplierEvaluationRepository _supplierRepository;
		private readonly IBusinessUnitRepository _businessUnitRepository;
		private readonly IGeneralRepository _generalRepository;
		private readonly IGeneralService _generalService;
		private readonly IVendorService _vendorService;
		private readonly IUnitOfWork _unitOfWork;

		public InvoiceService(IUserRepository userRepository,
							  IMapper mapper,
							  IInvoiceRepository invoiceRepository,
							  IAttachmentService attachmentService,
							  ISupplierEvaluationRepository supplierRepository,
							  IBusinessUnitRepository businessUnitRepository,
							  IGeneralRepository generalRepository,
							  IGeneralService generalService,
							  IVendorService vendorService,
							  IUnitOfWork unitOfWork)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_invoiceRepository = invoiceRepository;
			_unitOfWork = unitOfWork;
			_attachmentService = attachmentService;
			_supplierRepository = supplierRepository;
			_generalRepository = generalRepository;
			_businessUnitRepository = businessUnitRepository;
			_vendorService = vendorService;
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
					model.InvoiceRegisterIds[i].Sequence, model.ApproveStatus, model.Comment, userId,
					model.RejectReasonId);

				if (model.InvoiceRegisterIds[i].InMaxSequence && model.InvoiceRegisterIds[i].InvoiceTypeId == 1)
				{
					var data = await _invoiceRepository.InvoiceIUD(model.BusinessUnitId,
						model.InvoiceRegisterIds[i].InvoiceRegisterId, userId);
				}

				var businessUnit = await _businessUnitRepository.GetByIdAsync(model.BusinessUnitId);

				if (businessUnit.UseOrderForInvoice == false)
				{
					await _invoiceRepository.InvoiceApproveIntegration(model.InvoiceRegisterIds[i].InvoiceRegisterId, userId, model.BusinessUnitId);
				}

				if (model.InvoiceRegisterIds[i].InMaxSequence && businessUnit.UseOrderForInvoice == false)
				{
					//var invoice = GetInvoiceRegisterLoad(model.InvoiceRegisterIds[i].InvoiceRegisterId, name);
					var invoice = await _invoiceRepository.GetInvoiceRegisterMainLoad(model.InvoiceRegisterIds[i].InvoiceRegisterId);

					CreateVendorRequest requestVendor = new CreateVendorRequest
					{
						VendorCode = invoice.VendorCode,
						UserId = userId,
						BusinessUnitId = businessUnit.BusinessUnitId,
					};
					await _vendorService.TransferToIntegration(requestVendor);
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

		public async Task<ApiResponse<RegisterMainLoadDto>> Info(int invoiceRegisterId, string name)
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

		public async Task<ApiResponse<int>> Save(List<InvoiceRegisterSaveModel> model, string name)
		{
			int data = 0;
			int userId = await _userRepository.ConvertIdentity(name);
			for (int i = 0; i < model.Count; i++)
			{
				var check = await _invoiceRepository.CheckInvoiceRegister(model[i].InvoiceRegisterId,
					model[i].BusinessUnitId, model[i].VendorCode, model[i].InvoiceNo);
				if (check)
					return ApiResponse<int>.Fail(
						"Vendor Code and Invoice No can not be duplicate for the same Business Unit - Invoice no: " +
						model[i].InvoiceNo, 400);

				if (model[i].InvoiceRegisterId < 0) model[i].InvoiceRegisterId = 0;
				if (string.IsNullOrEmpty(model[i].InvoiceNo))
					model[i].InvoiceNo = "";


				data = await _invoiceRepository.Save(model[i], userId);


				if (model[i].Details != null)
					await InvoiceRegisterDetailsSave(data, model[i].Details);

				if (model[i].Attachments != null)
					await _attachmentService.SaveAttachmentAsync(model[i].Attachments, SourceType.INV, data);
			}

			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<int>.Success(data, 200);
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

		public async Task<ApiResponse<SaveResultModel>> SaveInvoiceMatchingForService(SaveInvoiceMatchingModel model,
			string userName)
		{
			SaveResultModel resultModel = new SaveResultModel();
			int userId = await _userRepository.ConvertIdentity(userName);
			int mainId = model.Main.InvoiceMatchingMainId;
			var data = await _invoiceRepository.SaveInvoiceMatchingMain(model.Main, userId);
			if (data > 0)
				mainId = data;
			if (mainId > 0)
			{
				var advanceTable = model.AdvanceInvoicesMatchingTypeList.ConvertListOfCLassToDataTable();
				var advanceSave = await _invoiceRepository.SaveInvoiceMatchingAdvances(mainId, advanceTable);

				var detailsData = _mapper.Map<List<InvoicesMatchingDetailsType>>(model.Details);
				var dataTable = detailsData.ConvertListOfCLassToDataTable();

				var result = await _invoiceRepository.SaveInvoiceMatchingDetails(mainId, dataTable);

				await _unitOfWork.SaveChangesAsync();

				await _invoiceRepository.InvoiceIUDIntegration(model.Main.BusinessUnitId, mainId, userId);

				if (result)
				{
					resultModel.MainId = mainId;
					resultModel.DetailIds = await _invoiceRepository.GetDetailIds(mainId);
					return ApiResponse<SaveResultModel>.Success(resultModel, 200);
				}
				else
				{
					return ApiResponse<SaveResultModel>.Fail("Data can not be saved for details", 400);
				}
			}
			else
			{
				return ApiResponse<SaveResultModel>.Fail("Data can not be saved for main", 400);
			}
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
			await _invoiceRepository.SaveInvoiceMatchingAdvances(request.InvoiceMatchingMainid, dataTable);
			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(true);
		}

		public async Task<ApiResponse<InvoiceRegisterByOrderMainIdDto>> InvoiceRegisterList(int orderMainId)
		{
			var data = await _invoiceRepository.InvoiceRegisterList(orderMainId);
			var dto = _mapper.Map<InvoiceRegisterByOrderMainIdDto>(data);
			var tax = await _supplierRepository.TaxDatas();
			dto.TaxDatas = _mapper.Map<List<TaxDto>>(tax);
			var withHoldingTax = await _supplierRepository.WithHoldingTaxDatas();
			dto.WithHoldingTaxDatas = _mapper.Map<List<WithHoldingTaxDto>>(withHoldingTax);
			return ApiResponse<InvoiceRegisterByOrderMainIdDto>.Success(dto, 200);
		}

		public async Task<ApiResponse<List<InvoiceRegisterServiceDetailsLoadDto>>> InvoiceRegisterServiceDetailsLoad(
			InvoiceRegisterServiceLoadModel model)
		{
			var data = await _invoiceRepository.InvoiceRegisterDetailsLoad(model);
			var businessUnit = (await _businessUnitRepository.GetAllAsync())
			  .SingleOrDefault(x => x.BusinessUnitId == model.BusinessUnitId);
			var dto = _mapper.Map<List<InvoiceRegisterServiceDetailsLoadDto>>(data);
			foreach (var item in dto)
			{
				item.IsReportEqualsDisCountBase = item.BaseCurrency == businessUnit.BaseCurrencyCode;
				item.IsReportEqualsDisCountReport = item.ReportCurrency ==
				  businessUnit.ReportingCurrencyCode;

			}
			if (dto != null)
				return ApiResponse<List<InvoiceRegisterServiceDetailsLoadDto>>.Success(dto, 200);

			return ApiResponse<List<InvoiceRegisterServiceDetailsLoadDto>>.Fail("Please, enter valid parameters", 400);
		}

		public async Task<ApiResponse<SaveResultModel>> SaveInvoiceMatchingForGRN(SaveInvoiceMatchingGRNModel model,
			string userName)
		{
			SaveResultModel resultModel = new SaveResultModel();
			int userId = await _userRepository.ConvertIdentity(userName);
			int mainId = model.Main.InvoiceMatchingMainId;
			var data = await _invoiceRepository.SaveInvoiceMatchingMain(model.Main, userId);
			if (data > 0)
				mainId = data;
			if (mainId > 0)
			{
				var advanceTable = model.AdvanceInvoicesMatchingTypeList.ConvertListOfCLassToDataTable();
				var advanceSave = await _invoiceRepository.SaveInvoiceMatchingAdvances(mainId, advanceTable);

				var grnTable = model.RNEInvoicesMatchingTypeList.ConvertListOfCLassToDataTable();
				var grnSave = await _invoiceRepository.SaveInvoiceMatchingGRNs(mainId, grnTable);


				var dataTable = model.Details.ConvertListOfCLassToDataTable();
				var result = await _invoiceRepository.SaveInvoiceMatchingDetails(mainId, dataTable);

				await _unitOfWork.SaveChangesAsync();

				await _invoiceRepository.InvoiceIUDIntegration(model.Main.BusinessUnitId, mainId, userId);

				if (result)
				{
					resultModel.MainId = mainId;
					resultModel.DetailIds = await _invoiceRepository.GetDetailIds(mainId);
					return ApiResponse<SaveResultModel>.Success(resultModel, 200);
				}
				else
				{
					return ApiResponse<SaveResultModel>.Fail("Data can not be saved for details", 400);
				}
			}
			else
			{
				return ApiResponse<SaveResultModel>.Fail("Data can not be saved for main", 400);
			}
		}

		public async Task<ApiResponse<bool>> CheckInvoiceRegister(int invocieRegisterId, int businessUnitId,
			string vendorCode, string invoiceNo)
		{
			var data = await _invoiceRepository.CheckInvoiceRegister(invocieRegisterId, businessUnitId, vendorCode,
				invoiceNo);

			if (data)
				return ApiResponse<bool>.Success(true);
			return ApiResponse<bool>.Fail(false, 400);
		}

		public async Task<ApiResponse<List<RegisterDraftDto>>> RegisterDraft(InvoiceRegisterGetModel model, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var data = await _invoiceRepository.RegisterDraft(model, userId);
			var dto = _mapper.Map<List<RegisterDraftDto>>(data);
			for (int i = 0; i < dto.Count; i++)
			{
				var attachment =
					await _attachmentService.GetAttachmentsAsync(data[i].InvoiceRegisterId, SourceType.INV,
						Modules.Invoices);
				dto[i].Attachments = attachment;
			}

			return ApiResponse<List<RegisterDraftDto>>.Success(dto);
		}

		public async Task<ApiResponse<List<RegisterHeldDto>>> RegisterHeld(InvoiceRegisterGetModel model, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var data = await _invoiceRepository.RegisterHeld(model, userId);
			var dto = _mapper.Map<List<RegisterHeldDto>>(data);
			for (int i = 0; i < dto.Count; i++)
			{
				var attachment =
					await _attachmentService.GetAttachmentsAsync(data[i].InvoiceRegisterId, SourceType.INV,
						Modules.Invoices);
				dto[i].Attachments = attachment;
			}

			return ApiResponse<List<RegisterHeldDto>>.Success(dto);
		}

		public async Task<ApiResponse<List<ApprovalInfoDto>>> ApprovalInfos(int invoiceRegisterId, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var data = await _invoiceRepository.ApprovalInfos(invoiceRegisterId, userId);
			var dto = _mapper.Map<List<ApprovalInfoDto>>(data);

			return ApiResponse<List<ApprovalInfoDto>>.Success(dto);
		}

		public async Task<ApiResponse<List<InvoiceMatchingMainGRNDto>>> MatchingMainGRNList(
			InvoiceMatchingMainModel model)
		{
			var data = await _invoiceRepository.MatchingMainGRNList(model);
			var dto = _mapper.Map<List<InvoiceMatchingMainGRNDto>>(data);
			return ApiResponse<List<InvoiceMatchingMainGRNDto>>.Success(dto, 200);
		}

		public async Task<ApiResponse<InvoiceMatchResultModelDto>> GetInvoiceMatchData(int invoiceMatchingMainId, int businessUnitId)
		{
			var data = await _invoiceRepository.GetInvoiceMatchData(invoiceMatchingMainId, businessUnitId);
			InvoiceMatchResultModelDto resultModel = new InvoiceMatchResultModelDto();
			resultModel.InvoiceMatchMainData = _mapper.Map<InvoiceMatchMainDataDto>(data.InvoiceMatchMainData);
			resultModel.InvoiceMatchDetailDatas = _mapper.Map<List<InvoiceMatchDetailDataDto>>(data.InvoiceMatchDetailDatas);
			resultModel.InvoiceMatchAdvances = _mapper.Map<List<InvoiceMatchAdvanceDto>>(data.InvoiceMatchAdvances);
			resultModel.InvoiceMatchGRN = _mapper.Map<List<InvoiceMatchGRNDto>>(data.InvoiceMatchGRN);
			return ApiResponse<InvoiceMatchResultModelDto>.Success(resultModel);
		}

		public async Task<ApiResponse<bool>> InvoiceRegisterDetailsSave(int invoiceRegisterMainId, List<InvoiceRegisterDetails> details)
		{
			var dataTable = details.ConvertListOfCLassToDataTable();
			var result = await _invoiceRepository.InvoiceRegisterDetailsSave(invoiceRegisterMainId, dataTable);

			await _unitOfWork.SaveChangesAsync();
			return ApiResponse<bool>.Success(result);
		}

		public async Task<ApiResponse<InvoiceRegisterLoadDto>> GetInvoiceRegisterLoad(int invoiceRegisterId, string name)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var main = await _invoiceRepository.GetInvoiceRegisterMainLoad(invoiceRegisterId);
			var details = await _invoiceRepository.GetInvoiceRegisterDetailsLoad(invoiceRegisterId);

			var dtoMain = _mapper.Map<InvoiceRegisterLoadDto>(main);

			dtoMain.Attachments = await _attachmentService.GetAttachmentsAsync(invoiceRegisterId,
				SourceType.INV, Modules.Invoices);

			dtoMain.Details = _mapper.Map<List<InvoiceRegisterGetDetailsDto>>(details);

			var withHoldingTax = await _supplierRepository.WithHoldingTaxDatas();
			dtoMain.WithHoldingTaxDatas = _mapper.Map<List<WithHoldingTaxDto>>(withHoldingTax);
			var tax = await _supplierRepository.TaxDatas();
			dtoMain.TaxDatas = _mapper.Map<List<TaxDto>>(tax);
			var businessUnits = await _businessUnitRepository.GetBusinessUnitListByCurrentUser(userId);
			dtoMain.BusinessUnits = _mapper.Map<List<BaseBusinessUnitDto>>(businessUnits);
			return ApiResponse<InvoiceRegisterLoadDto>.Success(dtoMain, 200);
		}

		public async Task<ApiResponse<List<InvoiceRegisterPayablesTransactionsDto>>> GetInvoiceRegisterPayablesTransactions(int invoiceRegisterId)
		{
			var data = await _invoiceRepository.GetInvoiceRegisterPayablesTransactions(invoiceRegisterId);
			var dto = _mapper.Map<List<InvoiceRegisterPayablesTransactionsDto>>(data);
			return ApiResponse<List<InvoiceRegisterPayablesTransactionsDto>>.Success(dto, 200);
		}

		public async Task<ApiResponse<List<InvoicePeriodListDto>>> GetPeriodList(int businessUnitId)
		{
			var data = await _invoiceRepository.GetPeriod(businessUnitId);

			var periodFrom = Int32.Parse(data.periodFrom);
			var periodTo = Int32.Parse(data.periodTo);

			List<InvoicePeriodListDto> result = new List<InvoicePeriodListDto>();

			for (int i = periodFrom; i <= periodTo; i++)
			{
				if (i % 100 == 13)
				{
					var nextYear = ((i / 1000) + 1);
					// Novbeti il 2024012
					i = Int32.Parse(nextYear.ToString() + "000");
					continue;
				}

				InvoicePeriodListDto dto = new InvoicePeriodListDto();
				dto.name = i.ToString();
				result.Add(dto);
			}

			return ApiResponse<List<InvoicePeriodListDto>>.Success(result, 200);
		}
		public async Task<ApiResponse<List<InvoiceRegisterOrderDetailDto>>> GetRegisterOrderDetails(int orderMainId)
		{
			var data = await _invoiceRepository.GetRegisterOrderDetails(orderMainId);
			var dto = _mapper.Map<List<InvoiceRegisterOrderDetailDto>>(data);

			return ApiResponse<List<InvoiceRegisterOrderDetailDto>>.Success(dto, 200);
		}
	}
}