using AutoMapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.API.Extensions;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
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

        public AccountController(UserManager<Application.Entities.Auth.User> userManager,
                                 SignInManager<Application.Entities.Auth.User> signInManager,
                                 IUserService userService,
                                 IVendorService vendorService,
                                 ITokenHandler handler,
                                 IMapper mapper,
                                 IMailService mailService,
                                 IEmailNotificationService emailNotificationService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _vendorService = vendorService;
            _tokenHandler = handler;
            _mapper = mapper;
            _mailService = mailService;
            _emailNotificationService = emailNotificationService;
        }



        [HttpGet("{email}")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
            => CreateActionResult(await _userService.SendResetPasswordEmail(email));


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return CreateActionResult(ApiResponse<bool>.Fail("email", $" {dto.Email} not found", 422));

            var userdto = _mapper.Map<UserRegisterModel>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            var emailVerified = await _userService.CheckEmailIsVerified(dto.Email);
            if (!emailVerified)
                return CreateActionResult(ApiResponse<bool>.Fail("email", "Please,verify your account", 422));

            if (signInResult.Succeeded && emailVerified)
            {
                await _userService.UpdateSessionAsync(user.Id, 1);
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
            List<Task> emails = new List<Task>();
            return CreateActionResult(await _userService.ConfirmEmail(verifyToken));
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel dto)
        {
            var newtoken = Guid.NewGuid();

            dto.VerifyToken = newtoken + _tokenHandler.CreateRefreshToken();
            dto.VerifyToken = Regex.Replace(dto.VerifyToken, @"[^a-zA-Z0-9_.~\-]", "");
            dto.VendorId = await _vendorService.GetByTaxIdAsync(dto.TaxId);
            ApiResponse<int> response = response = await _userService.UserRegisterAsync(dto);

            AccountResponseDto account = new();
            if (response.Data > 0)
            {

                var templateDataForVerification = await _emailNotificationService.GetEmailTemplateData(dto.Language, EmailTemplateKey.VER);
                var companyName = await _emailNotificationService.GetCompanyName(dto.Email);

                VM_EmailVerification emailVerification = new VM_EmailVerification()
                {
                    Username = dto.UserName,
                    Body = new HtmlString(string.Format(templateDataForVerification.Body, dto.FullName)),
                    CompanyName = companyName,
                    Header = templateDataForVerification.Header,
                    Language = dto.Language,
                    Subject = templateDataForVerification.Subject,
                    Token = HttpUtility.HtmlDecode(dto.VerifyToken),
                };

                string body = await GetMailBody<VM_EmailVerification>.GetBody(emailVerification, @"EmailVerification.cshtml", @"verification.png");
                MailModel mailModel = new MailModel()
                {
                    Body = body,
                    Header = templateDataForVerification.Header,
                    EmailType = EmailTemplateKey.VER,
                    Subject = templateDataForVerification.Subject,
                    Tos = new List<string> { dto.Email }
                };

                //Stopwatch stopwatch = Stopwatch.StartNew();
                //await _mailService.SendRequest(mailModel);
                //stopwatch.Stop();
                //TimeSpan timeSpan = stopwatch.Elapsed;
                //System.IO.File.WriteAllText(@"C:\Log.txt",timeSpan.ToString());

                Response.OnCompleted(async () =>
                {
                    await _mailService.SendUsingTemplate(templateDataForVerification.Subject, emailVerification, emailVerification.TemplateName(), emailVerification.ImageName(), new List<string> { dto.Email });
                });

                account.UserId = response.Data;
                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(account, 200));
            }

            return CreateActionResult(ApiResponse<bool>.Fail(response.Errors, 422));


            //string body = await GetMailBody<VM_EmailVerification>.GetBody(emailVerification, @"EmailVerification.cshtml", @"verification.png");
            //MailModel mailModel = new MailModel()
            //{
            //    Body = body,
            //    Header = templateDataForVerification.Header,
            //    EmailType = EmailTemplateKey.VER,
            //    Subject = templateDataForVerification.Subject,
            //    Tos = new List<string> { dto.Email }
            //};
            //await _mailService.SendRequest(mailModel);

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
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel resetPasswordrequestDto)
            => CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));

    }
}
