namespace SolaERP.Application.Models
{
    public class ResetPasswordModel
    {
        public int ResetPasswordCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
