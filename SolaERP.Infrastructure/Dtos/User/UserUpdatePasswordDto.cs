namespace SolaERP.Infrastructure.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
