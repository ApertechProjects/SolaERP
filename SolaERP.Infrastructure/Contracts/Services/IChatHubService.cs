namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IChatHubService
    {
        Task GetUsernameAsync(string userName);
        Task SendMessageAsync(string userName, string message);
        Task JoinToGroupAsync(string groupName);
        Task AddGroupAsync(string groupName);
        Task AddParticipantAsync(string groupName, string userName);
        Task GetUserInGroupsAsync(string groupName);
        Task SendMessageToGroupAsync(string groupName, string message);
    }
}
