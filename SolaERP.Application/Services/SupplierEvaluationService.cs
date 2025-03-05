using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.Country;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Extensions;
using SolaERP.Application.Helper;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Application.Validator;
using SolaERP.Infrastructure.ViewModels;
using SolaERP.Persistence.Exceptions;
using SolaERP.Persistence.Utils;
using System.Numerics;
using Language = SolaERP.Application.Enums.Language;
using PrequalificationGridData = SolaERP.Application.Entities.SupplierEvaluation.PrequalificationGridData;

namespace SolaERP.Persistence.Services
{
	public class SupplierEvaluationService : ISupplierEvaluationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IBusinessUnitRepository _buRepository;
		private readonly ISupplierEvaluationRepository _repository;
		private readonly IUserRepository _userRepository;
		private readonly IVendorRepository _vendorRepository;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IMailService _mailService;
		private readonly IUserService _userService;
		private readonly IFileUploadService _fileUploadService;
		private readonly IGeneralRepository _generalRepository;
		private readonly IAttachmentService _attachmentService;
		private readonly IBackgroundTaskQueue _taskQueue;

		public SupplierEvaluationService(ISupplierEvaluationRepository repository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IBusinessUnitRepository buRepository,
			IUserRepository userRepository,
			IVendorRepository vendorRepository,
			IEmailNotificationService emailNotificationService,
			IMailService mailService,
			IUserService userService,
			IFileUploadService fileUploadService,
			IGeneralRepository generalRepository,
			IAttachmentService attachmentService,
			IBackgroundTaskQueue taskQueue
			)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
			_buRepository = buRepository;
			_mapper = mapper;
			_userRepository = userRepository;
			_vendorRepository = vendorRepository;
			_emailNotificationService = emailNotificationService;
			_mailService = mailService;
			_userService = userService;
			_fileUploadService = fileUploadService;
			_generalRepository = generalRepository;
			_attachmentService = attachmentService;
			_taskQueue = taskQueue;
		}

		private void SetAttachmentIdToZeroWhenIsRevise(bool isRevise, List<AttachmentSaveModel> saveModels)
		{
			if (isRevise)
			{
				foreach (var item in saveModels)
				{
					item.AttachmentId = 0;
				}
			}
		}

		public async Task<ApiResponse<EvaluationResultModel>> AddAsync2(string useridentity,
			SupplierRegisterCommand2 command, bool isSubmitted = false)
		{
			try
			{
				var dueDiligence = command.DueDiligence
					.Where(x => x.Childs != null)         // Ensure the Childs list is not null
					.SelectMany(x => x.Childs)
					.ToList();

				var validatorDue = new DueDiligenceValidator(_repository);
				var validationDueResponse = await validatorDue.ValidateDueDiligenceAsync(dueDiligence);
				if (!string.IsNullOrEmpty(validationDueResponse))
				{
					throw new DueDiligenceException($"Please, fill all mandatory fields");
				}

				var validatorGridResponse = await validatorDue.ValidateGridDatas(dueDiligence);
				if (!validatorGridResponse)
				{
					throw new DueDiligenceException($"Fill grid datas for Financial Status");
				}

				
				User user = await _userRepository.GetByIdAsync(Convert.ToInt32(useridentity));
				command.CompanyInformation.VendorCode = command.CompanyInformation.VendorCode == ""
					? null
					: command.CompanyInformation.VendorCode;

				var processSelector = GetProcessSelector(command.CompanyInformation.VendorId, command.IsRevise);

				SetRevisionNumber(command, processSelector, isSubmitted);

				Vendor vendor = _mapper.Map<Vendor>(command?.CompanyInformation);
				if (isSubmitted && !processSelector.IsCreate)
				{
					vendor.ReviseDate = DateTime.UtcNow.AddHours(4);
				}

				string companyLogoFile = await _vendorRepository.GetCompanyLogoFileAsync(vendor.VendorId);


				int vendorId = await _vendorRepository.UpdateAsync(user.Id, vendor);



				vendor.RegistrationDate = vendor.RegistrationDate.ConvertDateToValidDate();

				#region Represented Company & Represented Products

				await _repository.DeleteRepresentedProductAsync(vendorId);
				await _repository.DeleteRepresentedCompanyAsync(vendorId);

				if (command?.CompanyInformation?.RepresentedCompanies != null)
					await _repository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany
					{
						VendorId = vendorId,
						RepresentedCompanyName = string.Join(",", command?.CompanyInformation?.RepresentedCompanies)
					});
				if (command?.CompanyInformation?.RepresentedProducts != null)
					await _repository.AddRepresentedProductAsync(new RepresentedProductData
					{
						VendorId = vendorId,
						RepresentedProductName = string.Join(",", command?.CompanyInformation?.RepresentedProducts)
					});

				#endregion

				#region BusinessCategory

				await _repository.DeleteVendorBusinessCategoryAsync(vendorId);
				foreach (var item in command.CompanyInformation.BusinessCategories)
				{
					await _repository.AddVendorBusinessCategoryAsync(new VendorBusinessCategoryData
					{
						VendorId = vendorId,
						BusinessCategoryId = item.Id
					});
				}

				#endregion

				#region BusinessSector

				//await _repository.DeleteVendorBusinessSectorAsync(vendorId);
				//foreach (var item in command.CompanyInformation.BusinessSectors)
				//{
				//await _repository.AddVendorBusinessSectorAsync(new VendorBusinessSectorData
				//    {
				//        VendorId = vendorId,
				//        BusinessSectorId = item.BusinessSectorId
				//    });
				//}

				#endregion

				#region ProductServices

				await _repository.DeleteProductServiceAsync(vendorId);
				foreach (var item in command.CompanyInformation.Services)
				{
					await _repository.AddProductServiceAsync(new ProductServiceData
					{
						VendorId = vendorId,
						ProductServiceId = item.Id
					});
				}

				#endregion

				#region PrequalificationCategory

				await _repository.DeletePrequalificationCategoryAsync(vendorId);
				foreach (var item in command.CompanyInformation.PrequalificationTypes)
				{
					await _repository.AddPrequalificationCategoryAsync(new PrequalificationCategoryData
					{
						VendorId = vendorId,
						PrequalificationCategoryId = item.Id
					});
				}

				#endregion

				#region Company Information Logo
				if (command?.CompanyInformation?.CompanyLogo != null)
				{
					SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, command?.CompanyInformation?.CompanyLogo);
					await _attachmentService.SaveAttachmentAsync(command?.CompanyInformation?.CompanyLogo,
						SourceType.VEN_LOGO, vendorId);
				}

				#endregion


				#region Company Information Attachments

				//SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, command?.CompanyInformation?.Attachments);
				//await _attachmentService.SaveAttachmentAsync2(command?.CompanyInformation?.Attachments,
				//    SourceType.VEN_OLET, vendorId);

				#endregion

				#region Setting Vendor Ids (COBC,NDA,Bank Accounts)

				command?.CodeOfBuConduct?.ForEach(x => x.VendorId = vendorId);
				command?.NonDisclosureAgreement?.ForEach(x => x.VendorId = vendorId);
				command?.BankAccounts?.ForEach(x => x.VendorId = vendorId);

				#endregion

				#region NDA

				if (command.NonDisclosureAgreement is not null)
				{
					if (command.NonDisclosureAgreement.Count > 0)
					{
						await _repository.DeleteNDAAsync(vendorId);
					}

					for (int i = 0; i < command.NonDisclosureAgreement.Count; i++)
					{
						command.NonDisclosureAgreement[i].VendorId = vendorId;
						var mappedNDA = _mapper.Map<VendorNDA>(command.NonDisclosureAgreement[i]);
						await _repository.AddNDAAsync(mappedNDA);
					}
				}

				#endregion

				#region COBC

				if (command.CodeOfBuConduct is not null)
				{
					if (command.CodeOfBuConduct.Count > 0)
					{
						await _repository.DeleteCOBCAsync(vendorId);
					}

					for (int i = 0; i < command.CodeOfBuConduct.Count; i++)
					{
						command.CodeOfBuConduct[i].VendorId = vendorId;
						await _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(command.CodeOfBuConduct[i]));
					}
				}

				#endregion

				#region Bank Accounts

				if (command.BankAccounts is not null)
				{
					for (var i = 0; i < command.BankAccounts.Count; i++)
					{
						var x = command.BankAccounts[i];

						if (x.Type == 2 && x.Id > 0)
						{
							await _vendorRepository.DeleteBankDetailsAsync(user.Id, x.Id);
						}

						else if (x.Type != 2)
						{
							x.VendorId = vendorId;

							var detaildId = await _vendorRepository.UpdateBankDetailsAsync(user.Id,
								_mapper.Map<VendorBankDetail>(x));

							if (x.AccountVerificationLetter != null)
							{
								SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, x.AccountVerificationLetter);
								await _attachmentService.SaveAttachmentAsync(x.AccountVerificationLetter,
									SourceType.VEN_BNK, detaildId);
							}
						}
					}
				}

				#endregion

				#region DueDiligence

				if (command.DueDiligence is not null)
				{
					foreach (var designSaveDto in command.DueDiligence)
					{
						foreach (var item in designSaveDto.Childs)
						{
							if (item.HasCheckBox == false)
							{
								item.CheckboxValue = false;
							}

							if (item.HasRadioBox == false)
							{
								item.RadioboxValue = false;
							}

							if (item.HasDateTime == false)
							{
								item.DateTimeValue = null;
							}

							if (item.TextareaValue == "null" || string.IsNullOrEmpty(item.TextareaValue))
							{
								item.TextareaValue = "";
							}

							if (item.TextboxValue == "null" || string.IsNullOrEmpty(item.TextboxValue))
							{
								item.TextboxValue = "";
							}

							var dueInputModel = _mapper.Map<VendorDueDiligenceModel>(item);
							dueInputModel.VendorId = vendorId;

							if (!string.IsNullOrEmpty(item.DateTimeValue) && item.DateTimeValue != "null")
							{
								try
								{
									dueInputModel.DateTimeValue = Convert.ToDateTime(item.DateTimeValue);
								}
								catch (Exception e)
								{
									dueInputModel.DateTimeValue = null;
								}
							}
							else
							{
								dueInputModel.DateTimeValue = null;
							}

							await _repository.UpdateDueAsync(dueInputModel);

							if (item?.HasDataGrid == true)
							{
								foreach (var gridData in item?.GridDatas)
								{
									if (gridData.Type == 2 && gridData.Id > 0)
									{
										await _repository.DeleteDueDesignGrid(gridData.Id);
										continue;
									}
									else if (gridData.Type != 2)
									{
										var gridDatas = _mapper.Map<DueDiligenceGridModel>(gridData);
										if (gridDatas.Id < 0)
											gridDatas.Id = 0;

										gridDatas.DueDesignId = item.DesignId;
										gridDatas.VendorId = vendorId;

										await _repository.UpdateDueDesignGrid(gridDatas);
									}
								}
							}

							if (item.Attachments != null)
							{
								item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
								SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, item.Attachments);
								await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_DUE,
									vendorId);
							}
						}
					}
				}

				#endregion

				#region Prequalification

				if (command.Prequalification is not null)
				{
					for (int i = 0; i < command.Prequalification.Count; i++)
					{
						for (int j = 0; j < command.Prequalification[i].Prequalifications.Count; j++)
						{
							foreach (var item in command?.Prequalification?[i].Prequalifications[j]?.Childs)
							{
								if (item.HasCheckbox == false)
								{
									item.CheckboxValue = false;
								}

								if (item.HasRadiobox == false)
								{
									item.RadioboxValue = false;
								}

								if (item.HasDateTime == false &&
									string.IsNullOrEmpty(item.DateTimeValue))
								{
									item.DateTimeValue = null;
								}

								if (item.TextareaValue == "null" ||
									string.IsNullOrEmpty(item.TextareaValue))
								{
									item.TextareaValue = "";
								}

								if (item.TextboxValue == "null" ||
									string.IsNullOrEmpty(item.TextboxValue))
								{
									item.TextboxValue = "";
								}

								var prequalificationValue =
									_mapper.Map<VendorPrequalificationValues>(item);
								if (!string.IsNullOrEmpty(item.DateTimeValue) &&
									item.DateTimeValue != "null")
								{
									try
									{
										prequalificationValue.DateTimeValue =
											Convert.ToDateTime(item.DateTimeValue);
									}
									catch (Exception e)
									{
										prequalificationValue.DateTimeValue = null;
									}
								}
								else
								{
									prequalificationValue.DateTimeValue = null;
								}

								prequalificationValue.VendorId = vendorId;


								await _repository.UpdatePrequalification(prequalificationValue);


								if (item?.Attachments is not null)
								{
									item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
									SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, item.Attachments);
									await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_PREQ,
										vendorId);
								}


								if (item.HasGrid == true)
								{
									if (item.GridDatas != null)
									{
										foreach (var gridData in item.GridDatas)
										{
											var gridDatas =
												_mapper.Map<PrequalificationGridData>(gridData);
											if (gridData.Type == 2 && gridData.Id > 0)
											{
												await _repository.DeletePreGridAsync(gridDatas
													.PreqqualificationGridDataId);
												continue;
											}
											else if (gridData.Type != 2)
											{
												if (gridDatas.PreqqualificationGridDataId < 0)
													gridDatas.PreqqualificationGridDataId = 0;
												gridDatas.PreqqualificationDesignId = item.DesignId;
												gridDatas.VendorId = vendorId;

												await _repository.UpdatePreGridAsync(gridDatas);
											}

										}
									}
								}
							}
						}
					}
				}

				#endregion

				if (processSelector.IsCreate && user.UserTypeId == 0)
				{
					user.VendorId = vendorId;
					await _userRepository.SaveUserAsync(user);
				}

				await _unitOfWork.SaveChangesAsync();
				EvaluationResultModel resultModel = new EvaluationResultModel()
				{
					VendorId = vendorId,
					BankDetails = await GetBankDetail(useridentity),
					CompanyInformation = await GetCompanyInformation(useridentity),
				};

				return ApiResponse<EvaluationResultModel>.Success(resultModel, 200);
			}
			catch (DueDiligenceException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		private async Task<bool> CompareVendorBusinessCategory(int oldVendorId, int currentVendorId)
		{
			var vendorOld = await _repository.GetVendorBuCategoriesAsync(oldVendorId);
			var vendorCurrent = await _repository.GetVendorBuCategoriesAsync(currentVendorId);
			List<int> oldCategories = vendorOld.Select(x => x.BusinessCategoryId).OrderBy(x => x).ToList();
			List<int> currentCategories = vendorCurrent.Select(x => x.BusinessCategoryId).OrderBy(x => x).ToList();
			string oldVersion = string.Join(",", oldCategories);
			string currentVersion = string.Join(",", currentCategories);
			if (oldVersion == currentVersion)
				return true;
			return false;

		}

		private async Task<bool> CompareVendorBusinessSector(int oldVendorId, int currentVendorId)
		{
			var vendorOld = await _repository.GetBusinessSectorAsync(oldVendorId);
			var vendorCurrent = await _repository.GetBusinessSectorAsync(currentVendorId);
			List<int> oldBusinessSector = vendorOld.Select(x => x.BusinessSectorId).OrderBy(x => x).ToList();
			List<int> currentBusinessSector = vendorCurrent.Select(x => x.BusinessSectorId).OrderBy(x => x).ToList();
			string oldVersion = string.Join(",", oldBusinessSector);
			string currentVersion = string.Join(",", currentBusinessSector);
			if (oldVersion == currentVersion)
				return true;
			return false;

		}

		private async Task<bool> CompareVendorRepresentedCompany(int oldVendorId, int currentVendorId)
		{
			var vendorOld = await _repository.GetRepresentedCompanyAsync(oldVendorId);
			var vendorCurrent = await _repository.GetRepresentedCompanyAsync(currentVendorId);
			string oldVersion = vendorOld?.RepresentedCompanyName;
			string currentVersion = vendorCurrent?.RepresentedCompanyName;
			if (oldVersion == currentVersion)
				return true;
			return false;

		}

		private async Task<bool> CompareVendorRepresentedProduct(int oldVendorId, int currentVendorId)
		{
			var vendorOld = await _repository.GetRepresentedProductAsync(oldVendorId);
			var vendorCurrent = await _repository.GetRepresentedProductAsync(currentVendorId);
			string oldVersion = vendorOld?.RepresentedProductName;
			string currentVersion = vendorCurrent?.RepresentedProductName;
			if (oldVersion == currentVersion)
				return true;
			return false;

		}

		public async Task<ApiResponse<EvaluationResultModel>> SubmitAsync2(string userIdentity,
		  SupplierRegisterCommand2 command)
		{
			try
			{
				var result = (await AddAsync2(userIdentity, command, true)).Data;

				await VendorSubmit(result.VendorId);

				User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

				var vendor = await _vendorRepository.GetHeader(result.VendorId);

				List<Task> emails = new List<Task>();

				await _mailService.SendRegistrationPendingMail(Convert.ToInt32(userIdentity));

				var changedFields = new List<string>();
				if (command.IsRevise)
				{
					var previousVendorId = await _vendorRepository.GetVendorPreviousVendorId(result.VendorId);
					var vendorOld = await _vendorRepository.GetHeader(previousVendorId);
					var vendorCurrent = await _vendorRepository.GetHeader(result.VendorId);
					changedFields = Compare.CompareRow(vendorOld, vendorCurrent);
					if (!await CompareVendorBusinessCategory(previousVendorId, result.VendorId))
						changedFields.Add("Business Category");
					if (!await CompareVendorBusinessSector(previousVendorId, result.VendorId))
						changedFields.Add("Business Sector");
					if (!await CompareVendorRepresentedCompany(previousVendorId, result.VendorId))
						changedFields.Add("Represented Company");
					if (!await CompareVendorRepresentedProduct(previousVendorId, result.VendorId))
						changedFields.Add("Represented Product");

				}

				var vendorUsers = await _userRepository.GetUserIdsByVendor(result.VendorId);
				if (vendorUsers.Count > 0)
				{
					var userApprovals = await _userRepository.CheckUserApproval(vendorUsers[0]);
					if (userApprovals)
						await _mailService.SendMailToAdminstrationForApproveRegistration(Convert.ToInt32(userIdentity), changedFields);
					else
						await _mailService.SendMailToAdminstrationForApproveRegistrationForAutoApprove(Convert.ToInt32(userIdentity), changedFields);
				}else
				{
					await _mailService.SendMailToAdminstrationForApproveRegistrationForAutoApprove(Convert.ToInt32(userIdentity), changedFields);
				}

				// New Register Vendor Send Mail / Yeni Qeydiyyatdan keçmiş vendora mail göndərilməsi
				List<string> approveGroupUsersEmails= await _userRepository.GetVendorApproveGroupUsersEmail();
				if (approveGroupUsersEmails.Count > 0)
				{
					_taskQueue.QueueBackgroundWorkItem(async token =>
					{
						_mailService.SendNewVendorApproveGroupEmail(approveGroupUsersEmails, vendor.VendorName);
					});	
				}

				return ApiResponse<EvaluationResultModel>.Success(result, 200);
			}
			catch (DueDiligenceException ex)
			{
				return ApiResponse<EvaluationResultModel>.Fail(ex.Message, 422);
			}
			catch (PrequalificationException ex)
			{
				return ApiResponse<EvaluationResultModel>.Fail(ex.Message, 422);
			}
			catch (Exception ex)
			{
				return ApiResponse<EvaluationResultModel>.Fail(ex.Message, 400);
			}
		}

		public async Task UpdateAsync(string userIdentity,
		  SupplierRegisterCommand2 command)
		{
			try
			{
				var result = (await UpdateVendorRevise(userIdentity, command, true)).Data;

				await VendorSubmit(result.VendorId);
				
				List<Task> emails = new List<Task>();

				await _mailService.SendRegistrationPendingMail(Convert.ToInt32(userIdentity));

				var changedFields = new List<string>();
				if (command.IsRevise)
				{
					var previousVendorId = await _vendorRepository.GetVendorPreviousVendorId(result.VendorId);
					var vendorOld = await _vendorRepository.GetHeader(previousVendorId);
					var vendorCurrent = await _vendorRepository.GetHeader(result.VendorId);
					changedFields = Compare.CompareRow(vendorOld, vendorCurrent);
					if (!await CompareVendorBusinessCategory(previousVendorId, result.VendorId))
						changedFields.Add("Business Category");
					if (!await CompareVendorBusinessSector(previousVendorId, result.VendorId))
						changedFields.Add("Business Sector");
					if (!await CompareVendorRepresentedCompany(previousVendorId, result.VendorId))
						changedFields.Add("Represented Company");
					if (!await CompareVendorRepresentedProduct(previousVendorId, result.VendorId))
						changedFields.Add("Represented Product");

				}

				var vendorUsers = await _userRepository.GetUserIdsByVendor(result.VendorId);
				if (vendorUsers.Count > 0)
				{
					var userApprovals = await _userRepository.CheckUserApproval(vendorUsers[0]);
					if (userApprovals)
						await _mailService.SendMailToAdminstrationForApproveRegistration(Convert.ToInt32(userIdentity),
							changedFields);
					else
						await _mailService.SendMailToAdminstrationForApproveRegistrationForAutoApprove(
							Convert.ToInt32(userIdentity), changedFields);
				}
				else
					await _mailService.SendMailToAdminstrationForApproveRegistrationForAutoApprove(
						Convert.ToInt32(userIdentity), changedFields);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
				
		public async Task<ApiResponse<EvaluationResultModel>> UpdateVendorRevise(string useridentity, SupplierRegisterCommand2 command, bool isSubmitted = false)
		{
			try
			{
				var dueDiligence = command.DueDiligence
					.Where(x => x.Childs != null)         // Ensure the Childs list is not null
					.SelectMany(x => x.Childs)
					.ToList();

				var validatorDue = new DueDiligenceValidator(_repository);
				var validationDueResponse = await validatorDue.ValidateDueDiligenceAsync(dueDiligence);
				if (!string.IsNullOrEmpty(validationDueResponse))
				{
					throw new DueDiligenceException($"Please, fill all mandatory fields");
				}

				var validatorGridResponse = await validatorDue.ValidateGridDatas(dueDiligence);
				if (!validatorGridResponse)
				{
					throw new DueDiligenceException($"Fill grid datas for Financial Status");
				}

				
				User user = await _userRepository.GetByIdAsync(Convert.ToInt32(useridentity));
				command.CompanyInformation.VendorCode = command.CompanyInformation.VendorCode == ""
					? null
					: command.CompanyInformation.VendorCode;

				var processSelector = GetProcessSelector(command.CompanyInformation.VendorId, command.IsRevise);

				SetRevisionNumber(command, processSelector, isSubmitted);

				Vendor vendor = _mapper.Map<Vendor>(command?.CompanyInformation);
				if (isSubmitted && !processSelector.IsCreate)
				{
					vendor.ReviseDate = DateTime.UtcNow.AddHours(4);
				}

				string companyLogoFile = await _vendorRepository.GetCompanyLogoFileAsync(vendor.VendorId);


				int vendorId = await _vendorRepository.UpdateAsync(user.Id, vendor);



				vendor.RegistrationDate = vendor.RegistrationDate.ConvertDateToValidDate();

				#region Represented Company & Represented Products

				await _repository.DeleteRepresentedProductAsync(vendorId);
				await _repository.DeleteRepresentedCompanyAsync(vendorId);

				if (command?.CompanyInformation?.RepresentedCompanies != null)
					await _repository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany
					{
						VendorId = vendorId,
						RepresentedCompanyName = string.Join(",", command?.CompanyInformation?.RepresentedCompanies)
					});
				if (command?.CompanyInformation?.RepresentedProducts != null)
					await _repository.AddRepresentedProductAsync(new RepresentedProductData
					{
						VendorId = vendorId,
						RepresentedProductName = string.Join(",", command?.CompanyInformation?.RepresentedProducts)
					});

				#endregion

				#region BusinessCategory

				await _repository.DeleteVendorBusinessCategoryAsync(vendorId);
				foreach (var item in command.CompanyInformation.BusinessCategories)
				{
					await _repository.AddVendorBusinessCategoryAsync(new VendorBusinessCategoryData
					{
						VendorId = vendorId,
						BusinessCategoryId = item.Id
					});
				}

				#endregion

				#region BusinessSector

				//await _repository.DeleteVendorBusinessSectorAsync(vendorId);
				//foreach (var item in command.CompanyInformation.BusinessSectors)
				//{
				//await _repository.AddVendorBusinessSectorAsync(new VendorBusinessSectorData
				//    {
				//        VendorId = vendorId,
				//        BusinessSectorId = item.BusinessSectorId
				//    });
				//}

				#endregion

				#region ProductServices

				await _repository.DeleteProductServiceAsync(vendorId);
				foreach (var item in command.CompanyInformation.Services)
				{
					await _repository.AddProductServiceAsync(new ProductServiceData
					{
						VendorId = vendorId,
						ProductServiceId = item.Id
					});
				}

				#endregion

				#region PrequalificationCategory

				await _repository.DeletePrequalificationCategoryAsync(vendorId);
				foreach (var item in command.CompanyInformation.PrequalificationTypes)
				{
					await _repository.AddPrequalificationCategoryAsync(new PrequalificationCategoryData
					{
						VendorId = vendorId,
						PrequalificationCategoryId = item.Id
					});
				}

				#endregion

				#region Company Information Logo
				if (command?.CompanyInformation?.CompanyLogo != null)
				{
					SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, command?.CompanyInformation?.CompanyLogo);
					await _attachmentService.SaveAttachmentAsync(command?.CompanyInformation?.CompanyLogo,
						SourceType.VEN_LOGO, vendorId);
				}

				#endregion


				#region Company Information Attachments

				//SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, command?.CompanyInformation?.Attachments);
				//await _attachmentService.SaveAttachmentAsync2(command?.CompanyInformation?.Attachments,
				//    SourceType.VEN_OLET, vendorId);

				#endregion

				#region Setting Vendor Ids (COBC,NDA,Bank Accounts)

				command?.CodeOfBuConduct?.ForEach(x => x.VendorId = vendorId);
				command?.NonDisclosureAgreement?.ForEach(x => x.VendorId = vendorId);
				command?.BankAccounts?.ForEach(x => x.VendorId = vendorId);

				#endregion

				#region NDA

				if (command.NonDisclosureAgreement is not null)
				{
					if (command.NonDisclosureAgreement.Count > 0)
					{
						await _repository.DeleteNDAAsync(vendorId);
					}

					for (int i = 0; i < command.NonDisclosureAgreement.Count; i++)
					{
						command.NonDisclosureAgreement[i].VendorId = vendorId;
						var mappedNDA = _mapper.Map<VendorNDA>(command.NonDisclosureAgreement[i]);
						await _repository.AddNDAAsync(mappedNDA);
					}
				}

				#endregion

				#region COBC

				if (command.CodeOfBuConduct is not null)
				{
					if (command.CodeOfBuConduct.Count > 0)
					{
						await _repository.DeleteCOBCAsync(vendorId);
					}

					for (int i = 0; i < command.CodeOfBuConduct.Count; i++)
					{
						command.CodeOfBuConduct[i].VendorId = vendorId;
						await _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(command.CodeOfBuConduct[i]));
					}
				}

				#endregion

				#region Bank Accounts

				if (command.BankAccounts is not null)
				{
					for (var i = 0; i < command.BankAccounts.Count; i++)
					{
						var x = command.BankAccounts[i];

						if (x.Type == 2 && x.Id > 0)
						{
							await _vendorRepository.DeleteBankDetailsAsync(user.Id, x.Id);
						}

						else if (x.Type != 2)
						{
							x.VendorId = vendorId;

							var detaildId = await _vendorRepository.UpdateBankDetailsAsync(user.Id,
								_mapper.Map<VendorBankDetail>(x));

							if (x.AccountVerificationLetter != null)
							{
								SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, x.AccountVerificationLetter);
								await _attachmentService.SaveAttachmentAsync(x.AccountVerificationLetter,
									SourceType.VEN_BNK, detaildId);
							}
						}
					}
				}

				#endregion

				#region DueDiligence

				if (command.DueDiligence is not null)
				{
					foreach (var designSaveDto in command.DueDiligence)
					{
						foreach (var item in designSaveDto.Childs)
						{
							if (item.HasCheckBox == false)
							{
								item.CheckboxValue = false;
							}

							if (item.HasRadioBox == false)
							{
								item.RadioboxValue = false;
							}

							if (item.HasDateTime == false)
							{
								item.DateTimeValue = null;
							}

							if (item.TextareaValue == "null" || string.IsNullOrEmpty(item.TextareaValue))
							{
								item.TextareaValue = "";
							}

							if (item.TextboxValue == "null" || string.IsNullOrEmpty(item.TextboxValue))
							{
								item.TextboxValue = "";
							}

							var dueInputModel = _mapper.Map<VendorDueDiligenceModel>(item);
							dueInputModel.VendorId = vendorId;

							if (!string.IsNullOrEmpty(item.DateTimeValue) && item.DateTimeValue != "null")
							{
								try
								{
									dueInputModel.DateTimeValue = Convert.ToDateTime(item.DateTimeValue);
								}
								catch (Exception e)
								{
									dueInputModel.DateTimeValue = null;
								}
							}
							else
							{
								dueInputModel.DateTimeValue = null;
							}

							await _repository.UpdateDueAsync(dueInputModel);

							if (item?.HasDataGrid == true)
							{
								foreach (var gridData in item?.GridDatas)
								{
									if (gridData.Type == 2 && gridData.Id > 0)
									{
										await _repository.DeleteDueDesignGrid(gridData.Id);
										continue;
									}
									else if (gridData.Type != 2)
									{
										var gridDatas = _mapper.Map<DueDiligenceGridModel>(gridData);
										if (gridDatas.Id < 0)
											gridDatas.Id = 0;

										gridDatas.DueDesignId = item.DesignId;
										gridDatas.VendorId = vendorId;

										await _repository.UpdateDueDesignGrid(gridDatas);
									}
								}
							}

							if (item.Attachments != null)
							{
								item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
								SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, item.Attachments);
								await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_DUE,
									vendorId);
							}
						}
					}
				}

				#endregion

				#region Prequalification

				if (command.Prequalification is not null)
				{
					for (int i = 0; i < command.Prequalification.Count; i++)
					{
						for (int j = 0; j < command.Prequalification[i].Prequalifications.Count; j++)
						{
							foreach (var item in command?.Prequalification?[i].Prequalifications[j]?.Childs)
							{
								if (item.HasCheckbox == false)
								{
									item.CheckboxValue = false;
								}

								if (item.HasRadiobox == false)
								{
									item.RadioboxValue = false;
								}

								if (item.HasDateTime == false &&
									string.IsNullOrEmpty(item.DateTimeValue))
								{
									item.DateTimeValue = null;
								}

								if (item.TextareaValue == "null" ||
									string.IsNullOrEmpty(item.TextareaValue))
								{
									item.TextareaValue = "";
								}

								if (item.TextboxValue == "null" ||
									string.IsNullOrEmpty(item.TextboxValue))
								{
									item.TextboxValue = "";
								}

								var prequalificationValue =
									_mapper.Map<VendorPrequalificationValues>(item);
								if (!string.IsNullOrEmpty(item.DateTimeValue) &&
									item.DateTimeValue != "null")
								{
									try
									{
										prequalificationValue.DateTimeValue =
											Convert.ToDateTime(item.DateTimeValue);
									}
									catch (Exception e)
									{
										prequalificationValue.DateTimeValue = null;
									}
								}
								else
								{
									prequalificationValue.DateTimeValue = null;
								}

								prequalificationValue.VendorId = vendorId;


								await _repository.UpdatePrequalification(prequalificationValue);


								if (item?.Attachments is not null)
								{
									item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
									SetAttachmentIdToZeroWhenIsRevise(command.IsRevise, item.Attachments);
									await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_PREQ,
										vendorId);
								}


								if (item.HasGrid == true)
								{
									if (item.GridDatas != null)
									{
										foreach (var gridData in item.GridDatas)
										{
											var gridDatas =
												_mapper.Map<PrequalificationGridData>(gridData);
											if (gridData.Type == 2 && gridData.Id > 0)
											{
												await _repository.DeletePreGridAsync(gridDatas
													.PreqqualificationGridDataId);
												continue;
											}
											else if (gridData.Type != 2)
											{
												if (gridDatas.PreqqualificationGridDataId < 0)
													gridDatas.PreqqualificationGridDataId = 0;
												gridDatas.PreqqualificationDesignId = item.DesignId;
												gridDatas.VendorId = vendorId;

												await _repository.UpdatePreGridAsync(gridDatas);
											}

										}
									}
								}
							}
						}
					}
				}

				#endregion

				if (processSelector.IsCreate && user.UserTypeId == 0)
				{
					user.VendorId = vendorId;
					await _userRepository.SaveUserAsync(user);
				}

				await _unitOfWork.SaveChangesAsync();
				EvaluationResultModel resultModel = new EvaluationResultModel()
				{
					VendorId = vendorId,
					BankDetails = await GetBankDetail(useridentity),
					CompanyInformation = await GetCompanyInformation(useridentity),
				};

				return ApiResponse<EvaluationResultModel>.Success(resultModel, 200);
			}
			catch (DueDiligenceException ex)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		private async Task VendorSubmit(int vendorId)
		{
			await _vendorRepository.VendorSubmit(vendorId);
		}

		#region old
		//public async Task<ApiResponse<int>> AddAsync(string userIdentity, string token, SupplierRegisterCommand command, bool isSubmitted = false, bool isRevise = false)
		//{
		//    try
		//    {
		//        User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
		//        command.CompanyInformation.VendorCode = command.CompanyInformation.VendorCode == ""
		//            ? null
		//            : command.CompanyInformation.VendorCode;

		//        var processSelector = GetProcessSelector(command.CompanyInformation.VendorId, isRevise);

		//        SetRevisionNumber(command, processSelector, isSubmitted);

		//        Vendor vendor = _mapper.Map<Vendor>(command?.CompanyInformation);
		//        if (isSubmitted && !processSelector.IsCreate)
		//        {
		//            vendor.ReviseDate = DateTime.UtcNow.AddHours(4);
		//        }

		//        int vendorId = await _vendorRepository.UpdateAsync(user.Id, vendor);

		//        vendor.RegistrationDate = vendor.RegistrationDate.ConvertDateToValidDate();

		//        #region Represented Company & Represented Products

		//        await _repository.DeleteRepresentedProductAsync(vendorId);
		//        await _repository.DeleteRepresentedCompanyAsync(vendorId);

		//        if (command?.CompanyInformation?.RepresentedCompanies != null)
		//            await _repository.AddRepresentedCompany(new Application.Models.VendorRepresentedCompany
		//            {
		//                VendorId = vendorId,
		//                RepresentedCompanyName = string.Join(",", command?.CompanyInformation?.RepresentedCompanies)
		//            });
		//        if (command?.CompanyInformation?.RepresentedProducts != null)
		//            await _repository.AddRepresentedProductAsync(new RepresentedProductData
		//            {
		//                VendorId = vendorId,
		//                RepresentedProductName = string.Join(",", command?.CompanyInformation?.RepresentedProducts)
		//            });

		//        #endregion

		//        #region BusinessCategory

		//        await _repository.DeleteVendorBusinessCategoryAsync(vendorId);
		//        foreach (var item in command.CompanyInformation.BusinessCategories)
		//        {
		//            await _repository.AddVendorBusinessCategoryAsync(new VendorBusinessCategoryData
		//            {
		//                VendorId = vendorId,
		//                BusinessCategoryId = item.Id
		//            });
		//        }

		//        #endregion

		//        #region BusinessSector

		//        //await _repository.DeleteVendorBusinessSectorAsync(vendorId);
		//        //foreach (var item in command.CompanyInformation.BusinessSectors)
		//        //{
		//        //    await _repository.AddVendorBusinessSectorAsync(new VendorBusinessSectorData
		//        //    {
		//        //        VendorId = vendorId,
		//        //        BusinessSectorId = item.BusinessSectorId
		//        //    });
		//        //}

		//        #endregion

		//        #region ProductServices

		//        await _repository.DeleteProductServiceAsync(vendorId);
		//        foreach (var item in command.CompanyInformation.Services)
		//        {
		//            await _repository.AddProductServiceAsync(new ProductServiceData
		//            {
		//                VendorId = vendorId,
		//                ProductServiceId = item.Id
		//            });
		//        }

		//        #endregion

		//        #region PrequalificationCategory

		//        await _repository.DeletePrequalificationCategoryAsync(vendorId);
		//        foreach (var item in command.CompanyInformation.PrequalificationTypes)
		//        {
		//            await _repository.AddPrequalificationCategoryAsync(new PrequalificationCategoryData
		//            {
		//                VendorId = vendorId,
		//                PrequalificationCategoryId = item.Id
		//            });
		//        }

		//        #endregion

		//        #region Company Information Logo

		//        await _attachmentService.SaveAttachmentAsync(command?.CompanyInformation?.CompanyLogo,
		//            SourceType.VEN_LOGO, vendorId);

		//        #endregion

		//        #region Company Information Attachments

		//        //await _attachmentService.SaveAttachmentAsync(command?.CompanyInformation?.Attachments,
		//        //    SourceType.VEN_OLET, vendorId);

		//        #endregion

		//        #region Setting Vendor Ids (COBC,NDA,Bank Accounts)

		//        command?.CodeOfBuConduct?.ForEach(x => x.VendorId = vendorId);
		//        command?.NonDisclosureAgreement?.ForEach(x => x.VendorId = vendorId);
		//        command?.BankAccounts?.ForEach(x => x.VendorId = vendorId);

		//        #endregion

		//        #region NDA

		//        if (command.NonDisclosureAgreement is not null)
		//        {
		//            if (command.NonDisclosureAgreement.Count > 0)
		//            {
		//                await _repository.DeleteNDAAsync(vendorId);
		//            }

		//            for (int i = 0; i < command.NonDisclosureAgreement.Count; i++)
		//            {
		//                command.NonDisclosureAgreement[i].VendorId = vendorId;

		//                if (command.NonDisclosureAgreement[i].Type != 2)
		//                {
		//                    var mappedNDA = _mapper.Map<VendorNDA>(command.NonDisclosureAgreement[i]);
		//                    await _repository.AddNDAAsync(mappedNDA);
		//                }
		//            }
		//        }

		//        #endregion

		//        #region COBC

		//        if (command.CodeOfBuConduct is not null)
		//        {
		//            if (command.CodeOfBuConduct.Count > 0)
		//            {
		//                await _repository.DeleteCOBCAsync(vendorId);
		//            }

		//            for (int i = 0; i < command.CodeOfBuConduct.Count; i++)
		//            {
		//                command.CodeOfBuConduct[i].VendorId = vendorId;

		//                if (command.CodeOfBuConduct[i].Type != 2)
		//                {
		//                    await _repository.AddCOBCAsync(_mapper.Map<VendorCOBC>(command.CodeOfBuConduct[i]));
		//                }
		//            }
		//        }

		//        #endregion

		//        #region Bank Accounts

		//        if (command.BankAccounts is not null)
		//        {
		//            for (var i = 0; i < command.BankAccounts.Count; i++)
		//            {
		//                var x = command.BankAccounts[i];

		//                if (x.Type == 2)
		//                {
		//                    await _vendorRepository.DeleteBankDetailsAsync(user.Id, x.Id);
		//                }

		//                else
		//                {
		//                    x.VendorId = vendorId;

		//                    var detaildId = await _vendorRepository.UpdateBankDetailsAsync(user.Id,
		//                        _mapper.Map<VendorBankDetail>(x));

		//                    if (x.AccountVerificationLetter != null)
		//                    {
		//                        await _attachmentService.SaveAttachmentAsync(x.AccountVerificationLetter,
		//                            SourceType.VEN_BNK, detaildId);
		//                    }
		//                }
		//            }
		//        }

		//        #endregion

		//        #region DueDiligence

		//        if (command.DueDiligence is not null)
		//        {
		//            foreach (var designSaveDto in command.DueDiligence)
		//            {
		//                foreach (var item in designSaveDto.Childs)
		//                {
		//                    if (item.HasCheckBox == false)
		//                    {
		//                        item.CheckboxValue = false;
		//                    }

		//                    if (item.HasRadioBox == false)
		//                    {
		//                        item.RadioboxValue = false;
		//                    }

		//                    if (item.HasDateTime == false)
		//                    {
		//                        item.DateTimeValue = null;
		//                    }

		//                    if (item.TextareaValue == "null" || string.IsNullOrEmpty(item.TextareaValue))
		//                    {
		//                        item.TextareaValue = "";
		//                    }

		//                    if (item.TextboxValue == "null" || string.IsNullOrEmpty(item.TextboxValue))
		//                    {
		//                        item.TextboxValue = "";
		//                    }

		//                    var dueInputModel = _mapper.Map<VendorDueDiligenceModel>(item);
		//                    dueInputModel.VendorId = vendorId;

		//                    if (!string.IsNullOrEmpty(item.DateTimeValue) && item.DateTimeValue != "null")
		//                    {
		//                        try
		//                        {
		//                            dueInputModel.DateTimeValue = Convert.ToDateTime(item.DateTimeValue);
		//                        }
		//                        catch (Exception e)
		//                        {
		//                            dueInputModel.DateTimeValue = null;
		//                        }
		//                    }
		//                    else
		//                    {
		//                        dueInputModel.DateTimeValue = null;
		//                    }

		//                    await _repository.UpdateDueAsync(dueInputModel);

		//                    if (item?.HasDataGrid == true)
		//                    {
		//                        foreach (var gridData in item?.GridDatas)
		//                        {
		//                            if (gridData.Type == 2)
		//                            {
		//                                await _repository.DeleteDueDesignGrid(gridData.Id);
		//                                continue;
		//                            }

		//                            var gridDatas = _mapper.Map<DueDiligenceGridModel>(gridData);
		//                            gridDatas.DueDesignId = item.DesignId;
		//                            gridDatas.VendorId = vendorId;

		//                            await _repository.UpdateDueDesignGrid(gridDatas);
		//                        }
		//                    }

		//                    if (item.Attachments != null)
		//                    {
		//                        item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
		//                        await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_DUE,
		//                            vendorId);
		//                    }
		//                }
		//            }
		//        }

		//        #endregion

		//        #region Prequalification

		//        if (command.Prequalification is not null)
		//        {
		//            for (int i = 0; i < command.Prequalification.Count; i++)
		//            {
		//                for (int j = 0; j < command.Prequalification[i].Prequalifications.Count; j++)
		//                {
		//                    foreach (var item in command?.Prequalification?[i].Prequalifications[j]?.Childs)
		//                    {
		//                        if (item.HasCheckbox == false)
		//                        {
		//                            item.CheckboxValue = false;
		//                        }

		//                        if (item.HasRadiobox == false)
		//                        {
		//                            item.RadioboxValue = false;
		//                        }

		//                        if (item.HasDateTime == false &&
		//                            string.IsNullOrEmpty(item.DateTimeValue))
		//                        {
		//                            item.DateTimeValue = null;
		//                        }

		//                        if (item.TextareaValue == "null" ||
		//                            string.IsNullOrEmpty(item.TextareaValue))
		//                        {
		//                            item.TextareaValue = "";
		//                        }

		//                        if (item.TextboxValue == "null" ||
		//                            string.IsNullOrEmpty(item.TextboxValue))
		//                        {
		//                            item.TextboxValue = "";
		//                        }

		//                        var prequalificationValue =
		//                            _mapper.Map<VendorPrequalificationValues>(item);
		//                        if (!string.IsNullOrEmpty(item.DateTimeValue) &&
		//                            item.DateTimeValue != "null")
		//                        {
		//                            try
		//                            {
		//                                prequalificationValue.DateTimeValue =
		//                                    Convert.ToDateTime(item.DateTimeValue);
		//                            }
		//                            catch (Exception e)
		//                            {
		//                                prequalificationValue.DateTimeValue = null;
		//                            }
		//                        }
		//                        else
		//                        {
		//                            prequalificationValue.DateTimeValue = null;
		//                        }

		//                        prequalificationValue.VendorId = vendorId;


		//                        await _repository.UpdatePrequalification(prequalificationValue);


		//                        if (item?.Attachments is not null)
		//                        {
		//                            item.Attachments.ForEach(x => x.AttachmentTypeId = item.DesignId);
		//                            await _attachmentService.SaveAttachmentAsync(item.Attachments, SourceType.VEN_PREQ,
		//                                vendorId);
		//                        }


		//                        if (item.HasGrid == true)
		//                        {
		//                            if (item.GridDatas != null)
		//                            {
		//                                foreach (var gridData in item.GridDatas)
		//                                {
		//                                    var gridDatas =
		//                                        _mapper.Map<PrequalificationGridData>(gridData);
		//                                    if (gridData.Type == 2)
		//                                    {
		//                                        await _repository.DeletePreGridAsync(gridDatas
		//                                            .PreqqualificationGridDataId);
		//                                        continue;
		//                                    }

		//                                    gridDatas.PreqqualificationDesignId = item.DesignId;
		//                                    gridDatas.VendorId = vendorId;

		//                                    await _repository.UpdatePreGridAsync(gridDatas);
		//                                }
		//                            }
		//                        }
		//                    }
		//                }
		//            }
		//        }

		//        #endregion

		//        if (processSelector.IsCreate && user.UserTypeId == 0)
		//        {
		//            user.VendorId = vendorId;
		//            await _userRepository.SaveUserAsync(user);
		//        }

		//        await _unitOfWork.SaveChangesAsync();

		//        return ApiResponse<int>.Success(vendorId, 200);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw;
		//    }
		//}

		//public async Task<ApiResponse<int>> SubmitAsync(string userIdentity, string token, SupplierRegisterCommand command, bool isRevise)
		//{
		//    try
		//    {
		//        var vendorId = (await AddAsync(userIdentity, token, command, true, isRevise)).Data;
		//        User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));

		//        var vendor = await _vendorRepository.GetHeader(vendorId);
		//        if (vendor.ReviseNo == 0)
		//        {
		//            await _vendorRepository.ChangeStatusAsync(vendorId, 0, 1, null, user.Id);
		//        }

		//        List<Task> emails = new List<Task>();
		//        Language language = "en".GetLanguageEnumValue();
		//        var companyName = await _emailNotificationService.GetCompanyName(user.Email);
		//        var templateDataForRegistrationPending =
		//            await _emailNotificationService.GetEmailTemplateData(language, EmailTemplateKey.RGA);
		//        VM_RegistrationPending registrationPending = new VM_RegistrationPending()
		//        {
		//            FullName = user.FullName,
		//            UserName = user.UserName,
		//            Header = templateDataForRegistrationPending.Header,
		//            Body = new HtmlString(string.Format(templateDataForRegistrationPending.Body, user.FullName)),
		//            Language = language,
		//            CompanyName = command.CompanyInformation.CompanyName,
		//        };

		//        Task VerEmail = _mailService.SendUsingTemplate(templateDataForRegistrationPending.Subject,
		//            registrationPending,
		//            registrationPending.TemplateName(),
		//            registrationPending.ImageName(),
		//            new List<string> { user.Email });
		//        emails.Add(VerEmail);

		//        var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.RP);
		//        foreach (var lang in Enum.GetValues<Language>())
		//        {
		//            var sendUserMails = await _userService.GetAdminUserMailsAsync(1, lang);
		//            if (sendUserMails.Count > 0)
		//            {
		//                var templateData = templates.First(x => x.Language == lang.ToString());
		//                VM_RegistrationIsPendingAdminApprove adminApprove = GetVM(command, user, templateData);
		//                Task RegEmail = _mailService.SendUsingTemplate(templateData.Subject, adminApprove,
		//                    adminApprove.TemplateName, adminApprove.ImageName, sendUserMails);
		//                emails.Add(RegEmail);
		//            }
		//        }

		//        await Task.Run(() => { Task.WhenAll(emails); });

		//        return ApiResponse<int>.Success(vendorId, 200);
		//    }
		//    catch (Exception ex)
		//    {
		//        return ApiResponse<int>.Fail(ex.Message, 400);
		//    }
		//}
		#endregion

		public async Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model)
		{
			CompanyInformation companyInformation = new()
			{
				BusinessCategories = await _generalRepository.BusinessCategories(),
				PaymentTerms = await _repository.GetPaymentTermsAsync(),
				Countries = _mapper.Map<List<CountryDto>>(await _repository.GetCountriesAsync()),
				PrequalificationTypes = await _repository.GetPrequalificationCategoriesAsync(),
				Services = await _repository.GetProductServicesAsync(),
			};

			var mapCurrency = _mapper.Map<List<CurrencyDto>>(await _repository.GetCurrenciesAsync());

			BankCodesDto bankCodes = new()
			{
				Currencies = mapCurrency,
				BankDetails = await _repository.GetVendorBankDetailsAsync(model.VendorId),
			};
			List<BusinessUnits> buUnits = await _buRepository.GetAllAsync();

			VM_GET_SupplierEvaluation viewModel = new()
			{
				CompanyInformation = companyInformation,
				BankCodes = bankCodes,
				BusinessUnits = _mapper.Map<List<BusinessUnitsDto>>(buUnits),
				DueDiligenceDesign = await GetDueDesignsAsync("", model.Language)
			};

			return ApiResponse<VM_GET_SupplierEvaluation>.Success(viewModel, 200);
		}

		public async Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity,
			int? revisedVendorId = null)
		{
			return ApiResponse<VM_GET_VendorBankDetails>.Success(await GetBankDetail(userIdentity, revisedVendorId));
		}

		private async Task<VM_GET_VendorBankDetails> GetBankDetail(string userIdentity,
			int? revisedVendorId = null)
		{
			var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;

			var currencyTask = await _repository.GetCurrenciesAsync();
			var mapCurrency = _mapper.Map<List<CurrencyDto>>(currencyTask);
			var bankDetailsTask = await _repository.GetVendorBankDetailsAsync(vendorId);

			var bankAccount = _mapper.Map<List<VendorBankDetailViewDto>>(bankDetailsTask);

			foreach (var item in bankAccount)
			{
				var attachment =
					await _attachmentService.GetAttachmentsAsync(item.Id, SourceType.VEN_BNK, Modules.Vendors);
				attachment = attachment.Count > 0 ? attachment : Enumerable.Empty<AttachmentDto>().ToList();
				item.AccountVerificationLetter = attachment;
			}


			VM_GET_VendorBankDetails bankDetails = new()
			{
				Currencies = mapCurrency,
				BankDetails = bankAccount,
			};

			return bankDetails;
		}

		public async Task<ApiResponse<List<CodeOfBuConduct>>> GetCOBCAsync(string userIdentity, int? revisedVendorId = null)
		{
			User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;
			List<VendorCOBC> cobc = await _repository.GetCOBCAsync(vendorId);

			var buUnits = await _buRepository.GetAllAsync();
			var matchingBuUnitsIds = cobc.Select(x => x.BusinessUnitId).ToList();

			var result = buUnits
				.Select(x => new CodeOfBuConduct
				{
					VendorFullName = user.FullName,
					CobcID = cobc.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorCOBCId ?? 0,
					VendorId = user.VendorId,
					BusinessUnitId = x.BusinessUnitId,
					BusinessUnitCode = x.BusinessUnitCode,
					BusinessUnitName = x.BusinessUnitName,
					TaxId = x.TaxId,
					Address = x.Address,
					CountryCode = x.CountryCode,
					FullName = x.FullName,
					Position = x.Position,
					IsAgreed = matchingBuUnitsIds.Contains(x.BusinessUnitId),
					Type = cobc.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorCOBCId == null
						? 2
						: 0
				})
				.ToList();


			return ApiResponse<List<CodeOfBuConduct>>.Success(result, 200);
		}

		public async Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(string userIdentity,
			string acceptLanguage, int? vendorId = null)
			=> ApiResponse<List<DueDiligenceDesignDto>>.Success(await GetDueDesignsAsync(userIdentity, Language.en,
				vendorId));


		public async Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity,
			int? revisedVendorId = null)
		{
			var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendor = revisedVendorId ?? user.VendorId;
			
			var vendorPrequalificationTask = await _repository.GetVendorPrequalificationAsync(vendor);
			var vendorRepresentedProduct = await _repository.GetRepresentedProductAsync(vendor);
			var vendorBusinessSector = await _repository.GetBusinessSectorAsync(vendor);
			var vendorBusinessCategoriesTask = await _repository.GetVendorBuCategoriesAsync(vendor);
			var vendorProductsTask = await _repository.GetVendorProductServices(vendor);
			var vendorRepresentedCompany = await _repository.GetRepresentedCompanyAsync(vendor);

			var companyInfoTask = await _repository.GetCompanyInfoAsync(vendor);
			companyInfoTask.CompanyRegistrationDate = vendor == 0 ? null : companyInfoTask.CompanyRegistrationDate;
			companyInfoTask.CreditDays = vendor == 0 ? 60 : companyInfoTask.CreditDays;
			companyInfoTask.AgreeWithDefaultDays = vendor == 0 ? 1 : companyInfoTask.AgreeWithDefaultDays;

			var prequalificationTypesTask = await _repository.GetPrequalificationCategoriesAsync();
			var businessCategoriesTask = await _generalRepository.BusinessCategories();
			var businessSectorTask = await _repository.GetBusinessSectorAsync();
			var productServices = await _repository.GetProductServicesAsync();
			var paymentTerms = await _repository.GetPaymentTermsAsync();

			var venLogoAttachmentTask =
				await _attachmentService.GetAttachmentsAsync(vendor, SourceType.VEN_LOGO, Modules.Vendors);
			var venOletAttachmentTask =
				await _attachmentService.GetAttachmentsAsync(vendor, SourceType.VEN_OLET, Modules.Vendors);
			var productServicesTask = await _repository.GetProductServicesAsync();
			var countries = await _repository.GetCountriesAsync();

			var matchedPrequalificationTypes = prequalificationTypesTask
				.Where(x => vendorPrequalificationTask.Select(y => y.PrequalificationCategoryId).Contains(x.Id))
				.ToList();

			var matchedBuCategories = businessCategoriesTask
				.Where(x => vendorBusinessCategoriesTask.Select(y => y.BusinessCategoryId).Contains(x.Id))
				.ToList();

			var matchedProductServices = productServicesTask
				.Where(x => vendorProductsTask.Select(y => y.ProductServiceId).Contains(x.Id))
				.ToList();

			var companyInfo = _mapper.Map<CompanyInfoViewDto>(companyInfoTask);
			companyInfo.PrequalificationTypes = matchedPrequalificationTypes;
			companyInfo.BusinessCategories = matchedBuCategories;
			companyInfo.CompanyLogo = venLogoAttachmentTask;
			companyInfo.Attachments = venOletAttachmentTask;
			companyInfo.City ??= "";
			companyInfo.RepresentedProducts = vendorRepresentedProduct?.RepresentedProductName?.Split(",");
			companyInfo.RepresentedCompanies = vendorRepresentedCompany?.RepresentedCompanyName?.Split(",");
			companyInfo.BusinessSectors = vendorBusinessSector;
			companyInfo.Services = matchedProductServices;
			companyInfo.PhoneNo = companyInfoTask.PhoneNo;
			companyInfo.ContactPerson = companyInfo.ContactPerson;
			companyInfo.Email = companyInfo.Email;
			companyInfo.CreditDays = companyInfoTask.CreditDays;

			VM_GET_InitalRegistration viewModel = new()
			{
				CompanyInformation = companyInfo,
				BusinessCategories = businessCategoriesTask,
				PaymentTerms = paymentTerms,
				PrequalificationTypes = prequalificationTypesTask,
				Services = productServices,
				Countries = countries,
				BusinessSectors = businessSectorTask
			};

			return ApiResponse<VM_GET_InitalRegistration>.Success(viewModel, 200);
		}

		public async Task<CompanyInfoViewDto> GetCompanyInformation(string userIdentity)
		{
			var user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendor = user.VendorId;

			var vendorPrequalificationTask = await _repository.GetVendorPrequalificationAsync(vendor);
			var vendorRepresentedProduct = await _repository.GetRepresentedProductAsync(vendor);
			var vendorBusinessSector = await _repository.GetBusinessSectorAsync(vendor);
			var vendorBusinessCategoriesTask = await _repository.GetVendorBuCategoriesAsync(vendor);
			var vendorProductsTask = await _repository.GetVendorProductServices(vendor);
			var vendorRepresentedCompany = await _repository.GetRepresentedCompanyAsync(vendor);

			var companyInfoTask = await _repository.GetCompanyInfoAsync(vendor);
			companyInfoTask.CompanyRegistrationDate = vendor == 0 ? null : companyInfoTask.CompanyRegistrationDate;
			companyInfoTask.CreditDays = vendor == 0 ? 60 : companyInfoTask.CreditDays;
			companyInfoTask.AgreeWithDefaultDays = vendor == 0 ? 1 : companyInfoTask.AgreeWithDefaultDays;

			var prequalificationTypesTask = await _repository.GetPrequalificationCategoriesAsync();
			var businessCategoriesTask = await _generalRepository.BusinessCategories();

			var venLogoAttachmentTask =
				await _attachmentService.GetAttachmentsAsync(vendor, SourceType.VEN_LOGO, Modules.Vendors);
			var venOletAttachmentTask =
				await _attachmentService.GetAttachmentsAsync(vendor, SourceType.VEN_OLET, Modules.Vendors);
			var productServicesTask = await _repository.GetProductServicesAsync();

			var matchedPrequalificationTypes = prequalificationTypesTask
				.Where(x => vendorPrequalificationTask.Select(y => y.PrequalificationCategoryId).Contains(x.Id))
				.ToList();

			var matchedBuCategories = businessCategoriesTask
				.Where(x => vendorBusinessCategoriesTask.Select(y => y.BusinessCategoryId).Contains(x.Id))
				.ToList();

			var matchedProductServices = productServicesTask
				.Where(x => vendorProductsTask.Select(y => y.ProductServiceId).Contains(x.Id))
				.ToList();

			var companyInfo = _mapper.Map<CompanyInfoViewDto>(companyInfoTask);
			companyInfo.PrequalificationTypes = matchedPrequalificationTypes;
			companyInfo.BusinessCategories = matchedBuCategories;
			companyInfo.CompanyLogo = venLogoAttachmentTask;
			companyInfo.Attachments = venOletAttachmentTask;
			companyInfo.City ??= "";
			companyInfo.RepresentedProducts = vendorRepresentedProduct?.RepresentedProductName?.Split(",");
			companyInfo.RepresentedCompanies = vendorRepresentedCompany?.RepresentedCompanyName?.Split(",");
			companyInfo.BusinessSectors = vendorBusinessSector;
			companyInfo.Services = matchedProductServices;
			companyInfo.PhoneNo = companyInfoTask.PhoneNo;
			companyInfo.ContactPerson = user.FullName;
			companyInfo.Email = user.Email;
			companyInfo.CreditDays = companyInfoTask.CreditDays;

			return companyInfo;
		}

		public async Task<ApiResponse<List<NonDisclosureAgreement>>> GetNDAAsync(string userIdentity,
			int? revisedVendorId = null)
		{
			User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;

			List<VendorNDA> nda = await _repository.GetNDAAsync(vendorId);
			var bus = await _buRepository.GetAllAsync();
			var buUnits = bus.Where(x => x.BusinessUnitCode == "GLT");
			var matchingBuUnitsIds = nda.Select(y => y.BusinessUnitId).ToList();

			var result = buUnits
				.Select(x => new NonDisclosureAgreement
				{
					VendorFullName = user.FullName,
					NdaID = nda.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorNDAId ?? 0,
					VendorId = vendorId,
					BusinessUnitId = x.BusinessUnitId,
					BusinessUnitCode = x.BusinessUnitCode,
					BusinessUnitName = x.BusinessUnitName,
					TaxId = x.TaxId,
					Address = x.Address,
					CountryCode = x.CountryCode,
					FullName = x.FullName,
					Position = x.Position,
					IsAgreed = matchingBuUnitsIds.Contains(x.BusinessUnitId),
					Type = nda.FirstOrDefault(y => y.BusinessUnitId == x.BusinessUnitId)?.VendorNDAId == null
						? 2
						: 0
				})
				.ToList();

			return ApiResponse<List<NonDisclosureAgreement>>.Success(result, 200);
		}

		public async Task<ApiResponse<List<PrequalificationWithCategoryDto>>> GetPrequalificationAsync(
			string userIdentity, List<int> categoryIds, string acceptLang, int? revisedVendorId = null)
		{
			User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;

			List<VendorPrequalificationValues> prequalificationValues =
				await _repository.GetPrequalificationValuesAsync(vendorId);
			var responseModel = new List<PrequalificationWithCategoryDto>();

			var gridDatas = await _repository.GetPrequalificationGridAsync(vendorId);

			foreach (var categoryId in categoryIds)
			{
				List<PrequalificationDesign> prequalificationDesigns =
					await _repository.GetPrequalificationDesignsAsync(categoryId, Language.en);
				var category = await _repository.GetPrequalificationCategoriesAsync();
				var matchedCategory = category.FirstOrDefault(x => x.Id == categoryId);

				var categoryDto = new PrequalificationWithCategoryDto
				{
					Id = categoryId,
					Name = matchedCategory?.Category ?? "Unknown",
					Prequalifications = new List<VM_GET_Prequalification>()
				};

				var titleGroups = prequalificationDesigns.GroupBy(x => x.Title);
				foreach (var titleGroup in titleGroups)
				{
					var prequalificationTasks = titleGroup.Select(async design =>
					{
						var attachments = _mapper.Map<List<AttachmentDto>>(
							await _attachmentService.GetAttachmentsAsync(vendorId, SourceType.VEN_PREQ,
								Modules.Vendors, design.PrequalificationDesignId));

						var correspondingValue = prequalificationValues.FirstOrDefault(v =>
							v.PrequalificationDesignId == design.PrequalificationDesignId);
						var calculationResult =
							CalculateScoring(correspondingValue, design, gridDatas, attachments?.Count > 0);


						return new PrequalificationDto
						{
							VendorPrequalificationId = correspondingValue?.VendorPrequalificationId ?? 0,
							DesignId = design.PrequalificationDesignId,
							LineNo = design.LineNo,
							Discipline = design.Discipline,
							Questions = design.Questions,
							HasTextbox = design.HasTextbox > 0,
							HasTextarea = design.HasTextarea > 0,
							HasCheckbox = design.HasCheckbox > 0,
							HasRadiobox = design.HasRadiobox > 0,
							HasInt = design.HasInt > 0,
							HasDecimal = design.HasDecimal > 0,
							HasDateTime = design.HasDateTime > 0,
							HasAttachment = design.HasAttachment > 0,
							Title = design.Title,
							HasGrid = design.HasGrid > 0,
							DataGridPoint = design.HasGrid,
							GridRowLimit = design.GridRowLimit,
							GridColumnCount = design.GridColumnCount,
							GridColumns = new[]
							{
								design.Column1Alias,
								design.Column2Alias,
								design.Column3Alias,
								design.Column4Alias,
								design.Column5Alias
							}.Where(col => col != null).ToArray(),
							TextboxPoint = design.HasTextbox,
							TextareaPoint = design.HasTextarea,
							CheckboxPoint = design.HasCheckbox,
							RadioboxPoint = design.HasRadiobox,
							IntPoint = design.HasInt,
							DecimalPoint = design.HasDecimal,
							DateTimePoint = design.HasDateTime,
							Attachmentpoint = design.HasAttachment,
							TextboxValue = correspondingValue?.TextboxValue,
							TextareaValue = correspondingValue?.TextareaValue,
							CheckboxValue = Convert.ToBoolean(correspondingValue?.CheckboxValue),
							RadioboxValue = Convert.ToBoolean(correspondingValue?.RadioboxValue),
							IntValue = Convert.ToInt32(correspondingValue?.IntValue),
							DecimalValue = Convert.ToDecimal(correspondingValue?.DecimalValue),
							DateTimeValue = correspondingValue?.DateTimeValue == DateTime.MinValue
								? null
								: correspondingValue?.DateTimeValue,
							Attachments = attachments,
							Weight = design.Weight,
							Outcome = calculationResult.Outcome,
							Scoring = calculationResult.Scoring,
							AllPoint = calculationResult.AllPoint,
							Disabled = design.Disabled,
						};
					});

					var prequalificationDtos = await Task.WhenAll(prequalificationTasks);
					var prequalificationDto = new VM_GET_Prequalification
					{
						Title = titleGroup.Key,
						Childs = prequalificationDtos.ToList()
					};

					categoryDto.Prequalifications.Add(prequalificationDto);
				}

				responseModel.Add(categoryDto);
			}

			var response = await SetGridDatasAsync(responseModel, gridDatas, vendorId);
			return ApiResponse<List<PrequalificationWithCategoryDto>>.Success(response, 200);
		}

		public async Task<ApiResponse<List<PrequalificationWithCategoryDto2>>> GetPrequalificationAsync2(
		 string userIdentity, int id, string acceptLang, int? revisedVendorId = null)
		{
			User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;

			List<VendorPrequalificationValues> prequalificationValues =
				await _repository.GetPrequalificationValuesAsync(vendorId);
			var responseModel = new List<PrequalificationWithCategoryDto2>();

			var gridDatas = await _repository.GetPrequalificationGridAsync(vendorId);


			List<PrequalificationDesign> prequalificationDesigns =
				await _repository.GetPrequalificationDesignsAsync(id, Language.en);
			var category = await _repository.GetPrequalificationCategoriesAsync();
			var matchedCategory = category.FirstOrDefault(x => x.Id == id);

			var categoryDto = new PrequalificationWithCategoryDto2
			{
				Id = id,
				Name = matchedCategory?.Category ?? "Unknown",
				Prequalifications = new List<VM_GET_Prequalification2>()
			};

			var titleGroups = prequalificationDesigns.GroupBy(x => x.Title);
			foreach (var titleGroup in titleGroups)
			{
				var prequalificationTasks = titleGroup.Select(async design =>
				{
					var attachments = _mapper.Map<List<AttachmentDto>>(
						await _attachmentService.GetAttachmentsAsync(vendorId, SourceType.VEN_PREQ,
							Modules.Vendors, design.PrequalificationDesignId));

					var correspondingValue = prequalificationValues.FirstOrDefault(v =>
						v.PrequalificationDesignId == design.PrequalificationDesignId);
					var calculationResult =
						CalculateScoring(correspondingValue, design, gridDatas, attachments?.Count > 0);


					return new PrequalificationDto2
					{
						VendorPrequalificationId = correspondingValue?.VendorPrequalificationId ?? 0,
						DesignId = design.PrequalificationDesignId,
						LineNo = design.LineNo,
						Discipline = design.Discipline,
						Question = design.Questions,
						HasTextBox = design.HasTextbox > 0,
						HasTexarea = design.HasTextarea > 0,
						HasCheckbox = design.HasCheckbox > 0,
						HasRadioBox = design.HasRadiobox > 0,
						HasInt = design.HasInt > 0,
						HasDecimal = design.HasDecimal > 0,
						HasDateTime = design.HasDateTime > 0,
						HasAttachment = design.HasAttachment > 0,
						Title = design.Title,
						HasGrid = design.HasGrid > 0,
						DataGridPoint = design.HasGrid,
						GridRowLimit = design.GridRowLimit,
						GridColumnCount = design.GridColumnCount,
						GridColumns = new[]
						{
								design.Column1Alias,
								design.Column2Alias,
								design.Column3Alias,
								design.Column4Alias,
								design.Column5Alias
						}.Where(col => col != null).ToArray(),
						TextboxPoint = design.HasTextbox,
						TextareaPoint = design.HasTextarea,
						CheckboxPoint = design.HasCheckbox,
						RadioboxPoint = design.HasRadiobox,
						IntPoint = design.HasInt,
						DecimalPoint = design.HasDecimal,
						DateTimePoint = design.HasDateTime,
						Attachmentpoint = design.HasAttachment,
						TextboxValue = correspondingValue?.TextboxValue,
						TextareaValue = correspondingValue?.TextareaValue,
						CheckboxValue = Convert.ToBoolean(correspondingValue?.CheckboxValue),
						RadioboxValue = Convert.ToBoolean(correspondingValue?.RadioboxValue),
						IntValue = Convert.ToInt32(correspondingValue?.IntValue),
						DecimalValue = Convert.ToDecimal(correspondingValue?.DecimalValue),
						DateTimeValue = correspondingValue?.DateTimeValue == DateTime.MinValue
							? null
							: correspondingValue?.DateTimeValue,
						Attachments = attachments,
						Weight = design.Weight,
						Outcome = calculationResult.Outcome,
						Scoring = calculationResult.Scoring,
						AllPoint = calculationResult.AllPoint,
						Disabled = design.Disabled,
					};
				});

				var prequalificationDtos = await Task.WhenAll(prequalificationTasks);
				var prequalificationDto = new VM_GET_Prequalification2
				{
					Title = titleGroup.Key,
					Childs = prequalificationDtos.ToList()
				};

				categoryDto.Prequalifications.Add(prequalificationDto);
			}

			responseModel.Add(categoryDto);

			var response = await SetGridDatasAsync2(responseModel, gridDatas, vendorId);
			return ApiResponse<List<PrequalificationWithCategoryDto2>>.Success(response, 200);
		}



		private static VM_RegistrationIsPendingAdminApprove GetVM(SupplierRegisterCommand command, User user,
			EmailTemplateData templateData)
		{
			return new((UserRegisterType)user.UserTypeId)
			{
				Body = new HtmlString(templateData.Body),
				CompanyName = command.CompanyInformation.CompanyName,
				Header = templateData.Header,
				UserName = user.UserName,
				CompanyOrVendorName = command.CompanyInformation.CompanyName,
				Language = templateData.Language.GetLanguageEnumValue(),
			};
		}


		private async Task<List<DueDiligenceDesignDto>> GetDueDesignsAsync(string userIdentity, Language language,
			int? revisedVendorId = null)
		{

			User user = await _userRepository.GetByIdAsync(Convert.ToInt32(userIdentity));
			int vendorId = revisedVendorId ?? user.VendorId;

			List<DueDiligenceDesign> dueDiligence = await _repository.GetDueDiligencesDesignAsync(user.Language);
			List<DueDiligenceValue> dueDiligenceValues = await _repository.GetVendorDuesAsync(vendorId);

			var groupedList = dueDiligence.GroupBy(x => x.Title).ToList();
			var responseModel = new List<DueDiligenceDesignDto>();

			foreach (var group in groupedList)
			{
				var dto = new DueDiligenceDesignDto { Title = group.Key, Childs = new List<DueDiligenceChildDto>() };
				foreach (var d in group)
				{
					var correspondingValue =
						dueDiligenceValues.FirstOrDefault(v => v.DueDiligenceDesignId == d.DesignId);
					List<AttachmentDto> attachments = Enumerable.Empty<AttachmentDto>().ToList();

					if (d.HasAttachment > 0)
					{
						attachments = _mapper.Map<List<AttachmentDto>>(
							await _attachmentService.GetAttachmentsAsync(vendorId, SourceType.VEN_DUE,
								Modules.Vendors,
								d.DesignId));
					}

					var calculationResult =
						await CalculateScoring(correspondingValue, d, vendorId, attachments?.Count > 0);
					var childDto = new DueDiligenceChildDto
					{
						VendorDueDiligenceId = correspondingValue?.VendorDueDiligenceId ?? 0,
						DesignId = d.DesignId,
						LineNo = d.LineNo,
						Question = d.Question,
						HasTextBox = d.HasTextBox > 0,
						TextboxValue = correspondingValue?.TextboxValue,
						CheckboxValue = d.HasCheckBox > 0, // ? correspondingValue?.CheckboxValue : null,
						RadioboxValue = correspondingValue?.RadioboxValue == true,
						IntValue = Convert.ToInt32(correspondingValue?.IntValue),
						DecimalValue = Convert.ToDecimal(correspondingValue?.DecimalValue),
						DateTimeValue = Convert.ToDateTime(correspondingValue?.DateTimeValue),
						TextareaValue = correspondingValue?.TextareaValue ?? "",
						AgreementValue = correspondingValue?.AgreementValue == true,
						AgreementText = d.HasAgreement ? d.AgreementText ?? "" : null,
						HasCheckBox = d.HasCheckBox > 0, // ? true : null,
						HasRadioBox = d.HasRadioBox > 0,
						HasInt = d.HasInt > 0,
						HasDecimal = d.HasDecimal > 0,
						HasDateTime = d.HasDateTime > 0,
						HasAttachment = d.HasAttachment > 0,
						Attachments = attachments,
						HasBankList = d.HasBankList > 0,
						HasTexArea = d.HasTexArea > 0,
						ParentCompanies = d.ParentCompanies,
						HasDataGrid = d.HasGrid > 0,
						GridRowLimit = d.GridRowLimit, //> 0 ? d.GridRowLimit : null,
						GridColumnCount = d.GridColumnCount, //> 0 ? d.GridColumnCount : null,
						HasAgreement = d.HasAgreement,
						GridColumns = /*d.HasGrid > 0 ?*/ new[]
						{
							d.Column1Alias,
							d.Column2Alias,
							d.Column3Alias,
							d.Column4Alias,
							d.Column5Alias
						}.Where(col => col != null).ToArray(), //: null,
						TextBoxPoint = d.HasTextBox, //> 0 ? d.HasTextBox : null,
						TextAreaPoint = d.HasTexArea, //> 0 ? d.HasTexArea : null,
						CheckBoxPoint = d.HasCheckBox, //> 0 ? d.HasCheckBox : null,
						RadioBoxPoint = d.HasRadioBox, //> 0 ? d.HasRadioBox : null,
						BankListPoint = d.HasBankList, //> 0 ? d.HasBankList : null,
						IntPoint = d.HasInt, //> 0 ? d.HasInt : null,
						DecimalPoint = d.HasDecimal, //> 0 ? d.HasDecimal : null,
						DateTimePoint = d.HasDateTime, //> 0 ? d.HasDateTime : null,
						AttachmentPoint = d.HasAttachment, //> 0 ? d.HasAttachment : null,
						DataGridPoint = d.HasGrid,
						Weight = d.Weight,
						BankListValue = correspondingValue?.BankListValue,
						AllPoint = calculationResult.AllPoint,
						Scoring = d.HasBankList > 0 && !string.IsNullOrEmpty(correspondingValue?.TextboxValue)
							? 100
							: calculationResult.Scoring,
						Outcome = calculationResult.Outcome,
						Disabled = d.Disabled,
						IsMandatory = d.IsMandatory
					};

					if (childDto.LineNo == 3 || childDto.LineNo == 4)
						childDto.RadioboxValue = true;

					dto.Childs.Add(childDto);
				}

				responseModel.Add(dto);
			}

			return await SetGridDatasAsync(responseModel, vendorId);
		}

		private (decimal Scoring, decimal AllPoint, decimal Outcome) CalculateScoring(ValueEntity inputValue,
			PrequalificationDesign d, List<PrequalificationGridData> allDesignGrid, bool hasAttachment)
		{
			List<PrequalificationGridData> correspondingDesignGrid = null;

			if (d.HasGrid > 0)
				correspondingDesignGrid = allDesignGrid
					.Where(x => x.PreqqualificationDesignId == d.PrequalificationDesignId).ToList();

			decimal scoringSum = (!string.IsNullOrWhiteSpace(inputValue?.TextboxValue) ? d.HasTextbox : 0) +
								 (!string.IsNullOrWhiteSpace(inputValue?.TextareaValue) ? d.HasTextarea : 0) +
								 (inputValue?.CheckboxValue == true ? d.HasCheckbox : 0) +
								 (inputValue?.RadioboxValue == true ? d.HasRadiobox : 0) +
								 (inputValue?.IntValue > 0 ? d.HasInt : 0) +
								 (inputValue?.DecimalValue > 0 || !string.IsNullOrWhiteSpace(inputValue?.TextboxValue)
									 ? d.HasDecimal
									 : 0) +
								 (inputValue?.DateTimeValue != null ? d.HasDateTime : 0) +
								 (hasAttachment ? d.HasAttachment : 0) +
								 (correspondingDesignGrid?.Count > 0 ? d.HasGrid : 0);

			decimal allPoint = d.HasAttachment +
							   d.HasCheckbox +
							   d.HasDateTime +
							   d.HasDecimal +
							   d.HasGrid +
							   d.HasInt +
							   d.HasRadiobox +
							   d.HasTextarea +
							   d.HasTextbox;

			decimal scoring = 0;

			if (allPoint > 0)
				scoring = (scoringSum / allPoint) * 100;

			decimal outcome = (scoring * d.Weight) / 100;

			return (scoring, allPoint, outcome);
		}

		private async Task<(decimal Scoring, decimal AllPoint, decimal Outcome)> CalculateScoring(
			ValueEntity inputValue, DueDiligenceDesign d, int vendorId, bool hasAttachment)
		{
			List<DueDiligenceGrid> dueGrid = null;
			if (d.HasGrid > 0)
				dueGrid = await _repository.GetDueDiligenceGridAsync(d.DesignId, vendorId);

			decimal scoringSum = (!string.IsNullOrWhiteSpace(inputValue?.TextboxValue) ? d.HasTextBox : 0) +
								 (!string.IsNullOrWhiteSpace(inputValue?.TextareaValue) ? d.HasTexArea : 0) +
								 (inputValue?.CheckboxValue == true ? d.HasCheckBox : 0) +
								 (inputValue?.RadioboxValue == true ? d.HasRadioBox : 0) +
								 (inputValue?.IntValue > 0 ? d.HasInt : 0) +
								 (inputValue?.DecimalValue > 0 ? d.HasDecimal : 0) +
								 (inputValue?.DateTimeValue != null ? d.HasDateTime : 0) +
								 (hasAttachment ? d.HasAttachment : 0) +
								 (dueGrid?.Count > 0 ? d.HasGrid : 0);

			decimal allPoint = d.HasTextBox +
							   d.HasTexArea +
							   d.HasCheckBox +
							   d.HasRadioBox +
							   d.HasBankList +
							   d.HasInt +
							   d.HasDecimal +
							   d.HasDateTime +
							   d.HasAttachment +
							   d.HasGrid;


			decimal scoring = (scoringSum / allPoint) * 100;
			decimal outcome = (scoring * d.Weight) / 100;

			return (scoring, allPoint, outcome);
		}

		private async Task<List<DueDiligenceDesignDto>> SetGridDatasAsync(List<DueDiligenceDesignDto> dueDesign,
			int vendorId)
		{
			for (int i = 0; i < dueDesign.Count; i++)
			{
				for (int j = 0; j < dueDesign[i].Childs.Count; j++)
				{
					if (Convert.ToBoolean(dueDesign[i].Childs[j].HasDataGrid))
					{
						int childDesignId = dueDesign[i].Childs[j].DesignId;
						var gridDatas = await _repository.GetDueDiligenceGridAsync(childDesignId, vendorId);
						dueDesign[i].Childs[j].GridDatas = gridDatas;

						if ((dueDesign[i].Childs[j].Question.Contains("3 fiscal years")
							|| dueDesign[i].Childs[j].Question.Contains("3 il üçün")
							|| dueDesign[i].Childs[j].Question.Contains("3 финансовых года")
							) && gridDatas.Count == 0) //there is no another way for this
						{
							List<DueDiligenceGrid> staticDatas = new List<DueDiligenceGrid>()
							{
								new DueDiligenceGrid {Column4 = DateTime.Now.AddYears(-1).Year.ToString(),Id = -1},
								new DueDiligenceGrid {Column4 = DateTime.Now.AddYears(-2).Year.ToString(),Id = -2},
								new DueDiligenceGrid {Column4 = DateTime.Now.AddYears(-3).Year.ToString(),Id = -3},
							};
							dueDesign[i].Childs[j].GridDatas = staticDatas;
						}
					}
				}
			}

			return dueDesign;
		}

		private async Task<List<PrequalificationWithCategoryDto>> SetGridDatasAsync(
			List<PrequalificationWithCategoryDto> preualification, List<PrequalificationGridData> allGridData,
			int vendorId)
		{
			//var allGridData = await _repository.GetPrequalificationGridAsync(vendorId);
			var mappedGridData =
				_mapper.Map<List<Application.Dtos.SupplierEvaluation.PrequalificationGridData>>(allGridData);


			Parallel.For(0, preualification.Count, i =>
			{
				Parallel.For(0, preualification[Convert.ToInt32(i)].Prequalifications.Count, j =>
				{
					Parallel.For(0,
						preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs.Count, k =>
						{
							if (preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)]
									.Childs[Convert.ToInt32(k)].HasGrid != null)
							{
								int childDesignId = preualification[Convert.ToInt32(i)]
									.Prequalifications[Convert.ToInt32(j)].Childs[k].DesignId;
								var gridDatas = mappedGridData.Where(x => x.DesignId == childDesignId).ToList();
								preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)]
									.Childs[Convert.ToInt32(k)].GridDatas = gridDatas;
							}
						});
				});
			});

			return preualification;
		}

		private async Task<List<PrequalificationWithCategoryDto2>> SetGridDatasAsync2(
		  List<PrequalificationWithCategoryDto2> preualification, List<PrequalificationGridData> allGridData,
		  int vendorId)
		{
			//var allGridData = await _repository.GetPrequalificationGridAsync(vendorId);
			var mappedGridData =
				_mapper.Map<List<Application.Dtos.SupplierEvaluation.PrequalificationGridData>>(allGridData);


			Parallel.For(0, preualification.Count, i =>
			{
				Parallel.For(0, preualification[Convert.ToInt32(i)].Prequalifications.Count, j =>
				{
					Parallel.For(0,
						preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)].Childs.Count, k =>
						{
							if (preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)]
									.Childs[Convert.ToInt32(k)].HasGrid != null)
							{
								int childDesignId = preualification[Convert.ToInt32(i)]
									.Prequalifications[Convert.ToInt32(j)].Childs[k].DesignId;
								var gridDatas = mappedGridData.Where(x => x.DesignId == childDesignId).ToList();
								preualification[Convert.ToInt32(i)].Prequalifications[Convert.ToInt32(j)]
									.Childs[Convert.ToInt32(k)].GridDatas = gridDatas;
							}
						});
				});
			});

			return preualification;
		}


		public async Task<ApiResponse<bool>> UpdateVendor(string name, string taxId)
		{
			int userId = await _userRepository.ConvertIdentity(name);
			var result = await _repository.UpdateVendor(userId, taxId);
			await _unitOfWork.SaveChangesAsync();
			if (result)
				return ApiResponse<bool>.Success(result);
			return ApiResponse<bool>.Fail("result", 400);
		}

		private static ProcessSelectorDto GetProcessSelector(int? vendorId, bool isRevise)
		{
			var processSelector = new ProcessSelectorDto();

			if (isRevise)
			{
				processSelector.IsRevise = true;
				return processSelector;
			}

			if (vendorId is null or 0)
			{
				processSelector.IsCreate = true;
				return processSelector;
			}

			processSelector.IsUpdate = true;
			return processSelector;
		}

		private void SetRevisionNumber(SupplierRegisterCommand command, ProcessSelectorDto processSelector,
			bool isSubmitted)
		{
			if (processSelector.IsCreate)
			{
				command.CompanyInformation.ReviseNo = 0;
				return;
			}

			if (!isSubmitted) return;

			if (!processSelector.IsRevise) return;

			var resviseNo = _vendorRepository
				.GetRevisionNumberByVendorCode(command.CompanyInformation.VendorCode).Result;
			command.CompanyInformation.ReviseNo = resviseNo + 1;
		}

		private void SetRevisionNumber(SupplierRegisterCommand2 command, ProcessSelectorDto processSelector,
			bool isSubmitted)
		{
			string vendorCode = command.CompanyInformation.VendorCode;
			
			if (vendorCode == null || vendorCode.Equals(""))
			{
				command.CompanyInformation.ReviseNo = 0;
				return;
			}

			// if (!isSubmitted) return;
			//
			// if (!processSelector.IsRevise) return;

			var resviseNo = _vendorRepository
				.GetRevisionNumberByVendorCode(command.CompanyInformation.VendorCode).Result;
			command.CompanyInformation.ReviseNo = resviseNo + 1;
		}



	}
}
