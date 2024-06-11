namespace AdminHalloDoc.Models
{
    public class ConnectedUsers
    {
        public static List<ChatUser> myConnectedUsers = new List<ChatUser>();
    }
    public class ChatUser
    {
        public string ConnectionId { get; set; }
        public int RequestId { get; set; }
        public string SenderAspId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderType { get; set; }
        public string RecieverAspId { get; set; }
        public int RecieverId { get; set; }
        public string RecieverName { get; set; }
        public string ReceiverType { get; set; }
    }
    public class ChatJsonObject
    {
        public int AdminId { get; set; }
        public int PhysicianId { get; set; }
        public int RequestId { get; set; }
        public string Message { get; set; }
        public DateTime Datetime { get; set; }
    }
    public class GroupChatMsg
    {
        public int GroupName { get; set; }
        public string SenderAspnetId { get; set; }
        public string SenderConnectionId { get; set; }
        public string SenderType { get; set; }
        public string ConnectionIdPatient { get; set; }
        public string AspnetIdPatient { get; set; }
        public string ConnectionIdPhysician { get; set; }
        public string AspnetIdPhysician { get; set; }
        public string ConnectionIdAdmin { get; set; }
        public string AspnetIdAdmin { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }

    }
}
