namespace SignalRChatExample.Models
{
    public class GroupModel
    {
        public string GroupName { get; set; }
        public List<ClientModel> UserInGroup { get; } = new List<ClientModel>();
    }
}
