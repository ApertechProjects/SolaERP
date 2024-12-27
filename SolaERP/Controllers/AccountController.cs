using AutoMapper;
using FluentEmail.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RazorEngine.Templating;
using SolaERP.API.Extensions;
using SolaERP.API.Methods;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.Email;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace SolaERP.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AccountController : CustomBaseController
	{
		private readonly UserManager<Application.Entities.Auth.User> _userManager;
		private readonly SignInManager<Application.Entities.Auth.User> _signInManager;
		private readonly IUserService _userService;
		private readonly IVendorService _vendorService;
		private readonly ITokenHandler _tokenHandler;
		private readonly IMapper _mapper;
		private readonly IMailService _mailService;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IUserApprovalService _userApprovalService;
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IVendorRepository _vendorRepository;
		private readonly IConfiguration _configuration;

		public AccountController(UserManager<Application.Entities.Auth.User> userManager,
			SignInManager<Application.Entities.Auth.User> signInManager,
			IUserService userService,
			IVendorService vendorService,
			ITokenHandler handler,
			IMapper mapper,
			IMailService mailService,
			IEmailNotificationService emailNotificationService,
			IUserApprovalService userApprovalService,
			IUserRepository userRepository,
			IUnitOfWork unitOfWork,
			IVendorRepository vendorRepository,
			IConfiguration configuration
			)
		{
			_userService = userService;
			_signInManager = signInManager;
			_userManager = userManager;
			_vendorService = vendorService;
			_tokenHandler = handler;
			_mapper = mapper;
			_mailService = mailService;
			_emailNotificationService = emailNotificationService;
			_userApprovalService = userApprovalService;
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_vendorRepository = vendorRepository;
			_configuration = configuration;
		}


		[HttpPost]
		public async Task<IActionResult> SendResetPasswordEmail(string email)
		{
			var result = await _userService.SendResetPasswordEmail(email);
			if (result.StatusCode == 200)
			{
				Response.OnCompleted(async () =>
				{
					await _mailService.SendPasswordResetMailAsync(email, result.Data);
				});

				return CreateActionResult(ApiResponse<bool>.Success(true));
			}
			return CreateActionResult(result);
		}


		[RateLimit(3, 60)]
		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestModel dto)
		{

			var user = await _userManager.FindByNameAsync(dto.Email);
			if (user == null)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("email", $"email or password is incorrect", 422));
			}
			var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);


			if (!signInResult.Succeeded)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("email", $"email or password is incorrect", 422));
			}

			if (user.IsDeleted)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "This user was deleted.", 422));

			var userdto = _mapper.Map<UserRegisterModel>(user);

			var emailVerified = await _userService.CheckEmailIsVerified(dto.Email);
			if (!emailVerified)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Please, verify your account", 422));

			var isApproved = await _userApprovalService.CheckIsUserApproved(user.Id);
			if (!isApproved)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Please, wait for User approval", 422));

			if (user.InActive)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Your user is inactive", 422));
			}

			if (emailVerified)
			{
				await _userService.UpdateSessionAsync(user.Id, 1);
				await _userService.UpdateUserLastActivity(user.Id);
				var token = await _tokenHandler.GenerateJwtTokenAsync(1, userdto);
				await _userService.UpdateUserIdentifierAsync(user.Id, token.RefreshToken, token.Expiration, 5);

				var result = await _userService.CheckUserVerifyByVendor(dto.Email);
				if (!result)
					return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
						new AccountResponseDto { Token = token, IsEvaluation = true }, 200));


				return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
					new AccountResponseDto { Token = token }, 200));
			}


			return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
		}

		[RateLimit(8, 60)]
		[HttpPost]
		public async Task<IActionResult> LoginMs(LoginMsRequestModel dto)
		{
			Dictionary<string, bool> accessTokenData = await GetAccessTokenAsync(dto.Code);

			if (accessTokenData.First().Value == false)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("code", $"Token Not verified", 422));
			}

			Dictionary<string, bool> emailData = await GetEmailByAccessTokenAsync(accessTokenData.First().Key);

			if (emailData.First().Value == false)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("code", $"Token Not verified 2", 422));
			}

			var email = emailData.First().Key;

			var user = await _userManager.FindByNameAsync(email);
			if (user == null)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("email", $"email or password is incorrect", 422));
			}

			if (user.IsDeleted)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "This user was deleted.", 422));

			var userdto = _mapper.Map<UserRegisterModel>(user);

			var emailVerified = await _userService.CheckEmailIsVerified(email);
			if (!emailVerified)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Please, verify your account", 422));

			var isApproved = await _userApprovalService.CheckIsUserApproved(user.Id);
			if (!isApproved)
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Please, wait for User approval", 422));

			if (user.InActive)
			{
				return CreateActionResult(ApiResponse<bool>.Fail("email", "Your user is inactive", 422));
			}

			if (emailVerified)
			{
				await _userService.UpdateSessionAsync(user.Id, 1);
				await _userService.UpdateUserLastActivity(user.Id);
				var token = await _tokenHandler.GenerateJwtTokenAsync(1, userdto);
				await _userService.UpdateUserIdentifierAsync(user.Id, token.RefreshToken, token.Expiration, 5);

				var result = await _userService.CheckUserVerifyByVendor(email);
				if (!result)
					return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
						new AccountResponseDto { Token = token, IsEvaluation = true }, 200));


				return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
					new AccountResponseDto { Token = token }, 200));
			}


			return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
		}

		[NonAction]
		public async Task<Dictionary<string, bool>> GetEmailByAccessTokenAsync(string accessToken)
		{
			string Url = "https://graph.microsoft.com/v1.0/me";

			using var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var tokenNotVerifedResponse = new Dictionary<string, bool>() { { "Token not verified", false } };

			// Make GET request
			HttpResponseMessage response;
			try
			{
				response = await httpClient.GetAsync(Url);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Request failed: {ex.Message}");
				return tokenNotVerifedResponse;
			}

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Request failed with status: {response.StatusCode}");
				return tokenNotVerifedResponse;
			}

			// Parse the response JSON
			var content = await response.Content.ReadAsStringAsync();
			using var document = JsonDocument.Parse(content);

			// Check if "mail" exists in the response
			if (!document.RootElement.TryGetProperty("mail", out var mailElement))
			{
				Console.WriteLine("Mail property not found in response.");
				return tokenNotVerifedResponse;
			}

			return new Dictionary<string, bool>() { { mailElement.GetString()!, true } };
		}

		[NonAction]
		public async Task<Dictionary<string, bool>> GetAccessTokenAsync(string code)
		{
			const string url = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

			var tokenNotVerifedResponse = new Dictionary<string, bool>() { { "Token not verified", false } };

			using var httpClient = new HttpClient();

			var clientId = _configuration["MS:ClientId"];
			var secretId = _configuration["MS:SecretId"];
			var redirectUri = _configuration["MS:RedirectUri"];

			// Prepare form data
			var formData = new Dictionary<string, string>
			{
				{ "client_id", clientId },
				{ "client_secret", secretId },
				{ "grant_type", "authorization_code" },
				{ "code", code },
				{ "redirect_uri", redirectUri }
			};

			// Prepare request content
			var requestContent = new FormUrlEncodedContent(formData);

			// Set headers
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

			HttpResponseMessage response;
			try
			{
				response = await httpClient.PostAsync(url, requestContent);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Request failed: {ex.Message}");
				return tokenNotVerifedResponse;
			}

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Request failed with status: {response.StatusCode}");
				return tokenNotVerifedResponse;
			}

			// Parse the response JSON
			var content = await response.Content.ReadAsStringAsync();
			using var document = JsonDocument.Parse(content);

			// Check if "access_token" exists in the response
			if (!document.RootElement.TryGetProperty("access_token", out var accessTokenElement))
			{
				Console.WriteLine("Access token not found in response.");
				return tokenNotVerifedResponse;
			}

			return new Dictionary<string, bool>() { { accessTokenElement.GetString()!, true } };
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string verifyToken)
		{
			return CreateActionResult(await _userService.ConfirmEmail(verifyToken));
		}


		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterModel dto)
		{
			var user = await _userManager.FindByNameAsync(dto.Email);
			if (user is not null)
				return CreateActionResult(ApiResponse<bool>.Fail("email", $"This mail is already in use", 422));

			dto.VerifyToken = Helper.GetVerifyToken(_tokenHandler.CreateRefreshToken());

			dto.VendorId = await _vendorService.GetByTaxIdAsync(dto.TaxId);
			var respons = await _userService.UserRegisterAsync(dto, Response);
			if (dto.VendorId > 0 || dto.UserTypeId == 1)
			{
				await _userRepository.UserSendToApprove(respons.Data);
				await _unitOfWork.SaveChangesAsync();

			}
			AccountResponseDto account = new();
			if (respons.Data > 0)
			{
				account.UserId = respons.Data;
				await _mailService.SendEmailVerification(Response, account.UserId);
				return CreateActionResult(ApiResponse<AccountResponseDto>.Success(account, 200));
			}

			return CreateActionResult(ApiResponse<AccountResponseDto>.Fail(respons.Errors, 400));
		}


		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			var userId = await _userService.GetIdentityNameAsIntAsync(User.Identity.Name);
			await _userService.UpdateSessionAsync(Convert.ToInt32(User.Identity.Name), -1);
			return CreateActionResult(ApiResponse<bool>.Success(true, 200));
		}


		[HttpPost]
		public async Task<IActionResult> UpdatePasswordAsync(ResetPasswordModel resetPasswordrequestDto)
			=> CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));


		[RateLimit(3, 60)]
		[HttpPost]
		public async Task<IActionResult> CheckVerifyCode([FromBody] SingleItemModel model)
			=> CreateActionResult(await _userService.CheckVerifyCode(model.VerificationCode));


	}
}
