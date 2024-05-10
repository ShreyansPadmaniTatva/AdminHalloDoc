namespace AdminHalloDoc.Models
{
    public class ConnectedUsers
    {
        public static List<User> myConnectedUsers = new List<User>();
    }
    public class User
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public string Type { get; set; }
    }
}
