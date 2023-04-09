namespace SolaERP.Infrastructure.Models
{
    public class ResetPasswordModel
    {
        public string ResetPasswordCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
