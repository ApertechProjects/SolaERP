using AutoMapper.Execution;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Services
{
    public class EmailService : IEmailService
    {
        public ApiResponse<bool> SendEmailForResetPassword(UserResetPasswordDto dto)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential("username", "password");
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress("test@apertech.com");

                    smtpClient.Host = "mail.apertech.net";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    message.From = fromAddress;
                    message.Subject = "Reset Password";
                    message.IsBodyHtml = true;
                    Random rand = new Random();
                    int securityCode = rand.Next(100000, 999999);
                    message.Body = securityCode.ToString();
                    message.To.Add(dto.Email);
                    smtpClient.Send(message);
                    Kernel.PasswordForReset = securityCode.ToString();
                    return ApiResponse<bool>.Success(200);
                }
            }
        }

        public bool ValidateEmail(UserResetPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return false;

            if (dto.Email.Contains("@"))
            {
                string[] host = (dto.Email.Split('@'));
                string hostname = host[1];

                IPHostEntry IPhst = Dns.GetHostEntry(hostname);
                IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
                Socket s = new Socket(endPt.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                if (endPt != null)
                    return true;
                else
                    return false;
            }
            return false;

        }

        public ApiResponse<bool> VerifyIncomingCodeFromMail(string verifyCode)
        {
            verifyCode = verifyCode.Trim();
            if (verifyCode == Kernel.PasswordForReset)
                return ApiResponse<bool>.Success(200);
            return ApiResponse<bool>.Fail("Verify code is not correct", 400);
        }
    }
}
