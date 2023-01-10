namespace SolaERP.Infrastructure.Dtos.Chat
{
    public class ChatHistoryDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public Entities.Auth.User Sender { get; set; }
        public int ReceiverId { get; set; }
        public Entities.Auth.User Receiver { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
        public string ReceiverConnectionId { get; set; }
        public string SenderConnectionId { get; set; }
    }
}
