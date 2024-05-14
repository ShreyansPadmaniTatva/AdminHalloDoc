using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;

namespace AdminHalloDoc.ChatHub
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;
        public ChatHub(IChatRepository chatRepository) {
            _chatRepository = chatRepository;
        }
        public override Task OnConnectedAsync()
        {
            ChatUser user = new ChatUser();
            user.ConnectionId = Context.ConnectionId;
            user.SenderAspId = CV.ID();
            user.SenderType = CV.role();
            ChatUser usercheck = ConnectedUsers.myConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (usercheck == null)
            {
                ConnectedUsers.myConnectedUsers.Add(user);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            ChatUser user = ConnectedUsers.myConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();
            ConnectedUsers.myConnectedUsers.Remove(user);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message,int requestId, int RecieverId, string RecieverName, string RecieverType)
        {
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
            else
            {
                await Clients.Client(user).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
                ChatUser ChatUser = ConnectedUsers.myConnectedUsers.Where(u => u.SenderAspId == CV.ID()).FirstOrDefault();
                ChatUser.ReceiverType = RecieverType;
                ChatUser.RecieverId = RecieverId;
                ChatUser.RecieverName = RecieverName;
                ChatUser.RequestId = requestId;
                _chatRepository.AddText(ChatUser , message);
            }
        }
        public async Task<List<ChatJsonObject>> CheckHistory(string user, string message, int requestId, int RecieverId, string RecieverName, string RecieverType)
        {
            try
            {
                ChatUser chatUser = ConnectedUsers.myConnectedUsers.FirstOrDefault(u => u.SenderAspId == CV.ID());
                if (chatUser != null)
                {
                    chatUser.ReceiverType = RecieverType;
                    chatUser.RecieverId = RecieverId;
                    chatUser.RecieverName = RecieverName;
                    chatUser.RequestId = requestId;
                    chatUser.SenderId = Convert.ToInt32(CV.UserID());
                    chatUser.SenderName = CV.UserName();
                    return await _chatRepository.CheckHistory(chatUser);
                }
                else
                {
                    // Handle case where ChatUser is not found
                    return new List<ChatJsonObject>();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error in CheckHistory: {ex.Message}");
                throw; // Rethrow the exception to propagate it further
            }
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
