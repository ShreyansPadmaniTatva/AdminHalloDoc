using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AdminHalloDoc.Entities.Models;
using User = AdminHalloDoc.Models.User;

namespace AdminHalloDoc.ChatHub
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            User user = new User();
            user.ConnectionId = Context.ConnectionId;
            user.UserId = CV.ID();
            user.Type = CV.role();
            User usercheck = ConnectedUsers.myConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (usercheck == null)
            {
                ConnectedUsers.myConnectedUsers.Add(user);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            User user = ConnectedUsers.myConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            ConnectedUsers.myConnectedUsers.Remove(user);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
            else
            {
                await Clients.Client(user).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
            }
        }
        public string GetConnectionId() => Context.ConnectionId;
    }
}
