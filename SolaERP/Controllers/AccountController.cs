using AutoMapper;
using FluentEmail.Core;
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
using System.Diagnostics;
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
            IVendorRepository vendorRepository)
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
        }


        [HttpGet("{email}")]
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
                var token = await _tokenHandler.GenerateJwtTokenAsync(30, userdto);
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
                return CreateActionResult(ApiResponse<bool>.Fail("email", $" This mail is already in use", 422));

            dto.VerifyToken = Helper.GetVerifyToken(_tokenHandler.CreateRefreshToken());

            dto.VendorId = await _vendorService.GetByTaxIdAsync(dto.TaxId);
            var response = await _userService.UserRegisterAsync(dto);
            if (dto.VendorId > 0)
            {
                await _userRepository.UserSendToApprove(response.Data);
                await _unitOfWork.SaveChangesAsync();

            }
            AccountResponseDto account = new();
            if (response.Data > 0)
            {
                account.UserId = response.Data;
                await _mailService.SendEmailVerification(Response, account.UserId);
                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(account, 200));
            }

            return CreateActionResult(ApiResponse<AccountResponseDto>.Fail(response.Errors, 400));
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



        [HttpPost]
        public async Task<IActionResult> CheckVerifyCode([FromBody] SingleItemModel model)
            => CreateActionResult(await _userService.CheckVerifyCode(model.VerificationCode));


    }
}