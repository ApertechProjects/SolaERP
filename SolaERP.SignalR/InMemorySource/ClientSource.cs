using SignalRChatExample.Models;

namespace SolaERP.SignalR.InMemorySource
{
    public static class ClientSource
    {
        public static List<ClientModel> Source { get; } = new List<ClientModel>();
    }
}
