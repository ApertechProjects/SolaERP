namespace SignalRChatExample.Models
{
    public class ClientModel
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public int UserType { get; set; }
        public bool IsBusy { get; set; }
    }
}
