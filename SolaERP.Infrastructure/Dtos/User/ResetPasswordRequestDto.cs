namespace SolaERP.Infrastructure.Dtos.User
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
