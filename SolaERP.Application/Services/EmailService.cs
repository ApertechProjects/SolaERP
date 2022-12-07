namespace SolaERP.Application.Services
{
    //public class EmailService : IEmailService
    //{
    //    private readonly IConfiguration _configuration;
    //    private string ResetVrificationCode { get; set; }

    //    public MailService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }


    //    public async Task<ApiResponse<bool>> SendEmailForResetPassword(string email)
    //    {
    //        if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(dto.VerifyCode))
    //        {
    //            using (SmtpClient smtpClient = new SmtpClient())
    //            {
    //                var basicCredential = new NetworkCredential("username", "password");
    //                using (MailMessage message = new MailMessage())
    //                {
    //                    MailAddress fromAddress = new MailAddress("test@apertech.com");

    //                    smtpClient.Host = "mail.apertech.net";
    //                    smtpClient.Port = 587;
    //                    smtpClient.EnableSsl = true;
    //                    smtpClient.UseDefaultCredentials = false;
    //                    smtpClient.Credentials = basicCredential;

    //                    message.From = fromAddress;
    //                    message.Subject = "Reset Password";
    //                    message.IsBodyHtml = true;
    //                    Random rand = new Random();
    //                    ResetVrificationCode = rand.Next(100000, 999999).ToString();
    //                    message.Body = ResetVrificationCode;
    //                    message.To.Add(email);
    //                    await smtpClient.SendMailAsync(message);
    //                }
    //            }
    //            return ApiResponse<bool>.Success(200);
    //        }

    //        public bool ValidateEmail(UserCheckVerifyCodeDto dto)
    //        {
    //            if (string.IsNullOrEmpty(dto.Email))
    //                return false;

    //            if (dto.Email.Contains("@"))
    //            {
    //                string[] host = (dto.Email.Split('@'));
    //                string hostname = host[1];

    //                IPHostEntry IPhst = Dns.GetHostEntry(hostname);
    //                IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
    //                Socket s = new Socket(endPt.AddressFamily,
    //                        SocketType.Stream, ProtocolType.Tcp);

    //                if (endPt != null)
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            return false;

    //        }


    //    }
}
