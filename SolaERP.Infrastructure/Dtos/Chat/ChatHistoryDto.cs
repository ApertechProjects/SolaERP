namespace SolaERP.Infrastructure.Dtos.Chat
{
    public class ChatHistoryDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
