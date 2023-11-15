namespace SolaERP.Application.Dtos.Auth
{
    public class AccountResponseDto
    {
        public Token Token { get; set; }
        public int UserId { get; set; }
        public bool IsEvaluation { get; set; }
        public string  MyProperty { get; set; }
    }
}
