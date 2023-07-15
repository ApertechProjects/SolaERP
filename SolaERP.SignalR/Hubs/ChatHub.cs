using Microsoft.AspNetCore.SignalR;
using SignalRChatExample.Models;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Entities.Auth;
using SolaERP.SignalR.InMemorySource;
using SolaERP.SignalR.Models;

namespace SolaERP.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task GetUsernameAsync(ChatModel model)
        {
            #region Version 1
            //ClientModel client = new ClientModel
            //{
            //    ConnectionId = Context.ConnectionId,
            //};
            //ClientSource.Source.Add(client);
            //await Clients.GetAll.SendAsync("userJoined", userName);
            //await Clients.Others.SendAsync("activeUsers", ClientSource.Source);
            #endregion

            var sender = new User();
            if (sender != null)
            {
                ClientModel client = new ClientModel
                {
                    ConnectionId = Context.ConnectionId,
                    Username = sender.UserName,
                    UserType = sender.UserTypeId
                };
                ClientSource.Source.Add(client);
                await Clients.All.SendAsync("clientJoined", client.Username);
                await Clients.Others.SendAsync("activeClient", ClientSource.Source);
            }
        }

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }


        public async Task SendMessageAsync(ChatModel model)
        {
            var client = ClientSource.Source.FirstOrDefault(c => c.Username == model.Username);
            var sender = ClientSource.Source.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            if (client != null)
                await Clients.Clients(client.ConnectionId).SendAsync("getMessages", model.Message);



            //_chatService.SaveChatHistoryAsync()
        }

        public async Task JoinToGroupAsync(string groupName)
        {
            var user = ClientSource.Source.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);

            var isUserInGroup = group.UserInGroup.Any(x => x.ConnectionId == user.ConnectionId);

            if (!isUserInGroup)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
            group.UserInGroup.Add(user);
        }

        public async Task AddGroupAsync(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            GroupSource.Source.Add(new GroupModel { GroupName = groupName });

            await Clients.All.SendAsync("activeGroups", GroupSource.Source);
        }

        public async Task AddParticipantAsync(string groupName, string userName)
        {
            var participant = ClientSource.Source.FirstOrDefault(p => p.Username == userName);
            var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);
            var exsist = group.UserInGroup.Any(c => c.ConnectionId == participant.ConnectionId);

            if (!exsist)
            {
                group.UserInGroup.Add(participant);
                await Groups.AddToGroupAsync(participant.ConnectionId, groupName);
            }
        }

        public async Task GetUserInGroupsAsync(string groupName)
        {
            var group = GroupSource.Source.FirstOrDefault(g => g.GroupName == groupName);
            await Clients.Caller.SendAsync("getUserInGroups", group.UserInGroup);
        }

        public async Task SendMessageToGroupAsync(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("getMessages", message,
                ClientSource.Source.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId).Username);
        }
    }
}
