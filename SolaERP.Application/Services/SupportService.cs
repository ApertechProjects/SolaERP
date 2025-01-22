using AutoMapper;
using MediatR;
using SolaERP.Application.Constants;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.UOM;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using System.Reflection.Metadata.Ecma335;
using SolaERP.Application.Enums;
using SolaERP.Application.Dtos.Bid;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Dtos.Support;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Entities.Auth;

namespace SolaERP.Persistence.Services
{
	public class SupportService : ISupportService
	{
		private readonly ISupportRepository _repository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IGeneralRepository _generalRepository;
		private readonly IAttachmentService _attachmentService;
		private readonly IUserRepository _userRepository;
		private readonly IMailService _mailService;

		public SupportService(IUnitOfWork unitOfWork,
			ISupportRepository repository,
			IMapper mapper,
			IGeneralRepository generalRepository,
			IAttachmentService attachmentService,
			IMailService mailService,
			IUserRepository userRepository)
		{
			_unitOfWork = unitOfWork;
			_repository = repository;
			_mapper = mapper;
			_generalRepository = generalRepository;
			_attachmentService = attachmentService;
			_userRepository = userRepository;
			_mailService = mailService;
		}


		public async Task<ApiResponse<int>> Save(SupportSaveDto dto, string useridentity)
		{
			dto.UserId = Convert.ToInt32(useridentity);

			var newId = await _repository.Save(dto);

			if (newId < 1)
			{
				return ApiResponse<int>.Fail(0, 400);
			}

			if (dto.Attachments != null)
				await _attachmentService.SaveAttachmentAsync(dto.Attachments, SourceType.SUPPORT, newId);
			await _unitOfWork.SaveChangesAsync();

			var user = await _userRepository.GetCurrentUserInfo(dto.UserId);
			var email = user.Email;

			await _mailService.SendSupportMail(dto.UserId, dto.subject, dto.description, dto.Attachments);

			return ApiResponse<int>.Success(1, 200);
		}

	}
}