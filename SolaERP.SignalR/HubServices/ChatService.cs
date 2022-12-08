namespace SolaERP.SignalR.HubServices
{
    //public class ChatService //: IChatHubService
    //{
    //    private readonly IHubContext<ChatHub> _hubContext;

    //    public ChatService(IHubContext<ChatHub> hubContext)
    //    {
    //        _hubContext = hubContext;
    //    }

    //    public async Task GetUsernameAsync(string userName)
    //    {
    //        ClientModel client = new ClientModel
    //        {
    //            ConnectionId = _hubContext.ConnectionId,
    //            Username = userName
    //        };
    //        ClientSource.Source.Add(client);
    //        await Clients.All.SendAsync("userJoined", userName);
    //        await Clients.Others.SendAsync("activeUsers", ClientSource.Source);
    //    }

    //    public async Task SendMessageAsync(string userName, string message)
    //    {
    //        var client = ClientSource.Source.FirstOrDefault(c => c.Username == userName);

    //        if (client != null)
    //            await Clients.Clients(client.ConnectionId).SendAsync("getMessages", message);
    //    }

    //    public async Task JoinToGroupAsync(string groupName)
    //    {
    //        var user = ClientSource.Source.FirstOrDefault(c => c.ConnectionId == _hubContext.ConnectionId);
    //        var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);

    //        var isUserInGroup = group.UserInGroup.Any(x => x.ConnectionId == user.ConnectionId);

    //        if (!isUserInGroup)
    //        {
    //            await Groups.AddToGroupAsync(_hubContext.ConnectionId, groupName);
    //        }
    //        group.UserInGroup.Add(user);
    //    }

    //    public async Task AddGroupAsync(string groupName)
    //    {
    //        await Groups.AddToGroupAsync(_hubContext.ConnectionId, groupName);
    //        GroupSource.Source.Add(new GroupModel { GroupName = groupName });

    //        await Clients.All.SendAsync("activeGroups", GroupSource.Source);
    //    }

    //    public async Task AddParticipantAsync(string groupName, string userName)
    //    {
    //        var participant = ClientSource.Source.FirstOrDefault(p => p.Username == userName);
    //        var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);
    //        var exsist = group.UserInGroup.Any(c => c.ConnectionId == participant.ConnectionId);

    //        if (!exsist)
    //        {
    //            group.UserInGroup.Add(participant);
    //            await Groups.AddToGroupAsync(participant.ConnectionId, groupName);
    //        }
    //    }

    //    public async Task GetUserInGroupsAsync(string groupName)
    //    {
    //        var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);
    //        await Clients.Caller.SendAsync("getUserInGroups", group.UserInGroup);
    //    }

    //    public async Task SendMessageToGroupAsync(string groupName, string message)
    //    {
    //        await Clients.Group(groupName).SendAsync("getMessages", message,
    //            ClientSource.Source.FirstOrDefault(c => c.ConnectionId == _hubContext.ConnectionId).Username);
    //    }


    //}
}
