using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SolaERP.SignalR.Hubs;

namespace SolaERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _context;

        public ChatController(IHubContext<ChatHub> context)
        {
            _context = context;
        }

        [HttpPost("message")]
        public async Task SenMessage(string message)
        {
            await _context.Clients.All.SendAsync(message);
        }
    }
}
