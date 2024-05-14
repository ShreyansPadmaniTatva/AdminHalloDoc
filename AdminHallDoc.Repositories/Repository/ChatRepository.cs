using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminHalloDoc.Models;
using AdminHalloDoc.Entities.Models;
using System.Web.WebPages;
using Newtonsoft.Json;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class ChatRepository : IChatRepository
    {
        #region Constructor
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public string CreateTextFile(ChatUser user)
        {
            string fileName = user.SenderId + user.SenderType + "_" + user.RecieverId + user.ReceiverType + "_" + user.RequestId + ".txt";
            string FilePath = "wwwroot\\Upload\\ChatFile\\" + fileName;
            try
            {
                // Check if file already exists. If yes, delete it.
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }

                //Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                // Create a new file
                using (StreamWriter sw = File.CreateText(FilePath))
                {
                 
                }

                // Write file contents on console.
               
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return fileName;
        }

        public List<ChatJsonObject> ReadTextFile(string fileName)
        {
            List<ChatJsonObject> chatList = new List<ChatJsonObject>();
            // Write file contents on console.
            using (StreamReader sr = File.OpenText("wwwroot\\Upload\\ChatFile\\" + fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ChatJsonObject chat = JsonConvert.DeserializeObject<ChatJsonObject>(line);
                    chatList.Add(chat);
                }
            }
            return chatList;
        }

        public bool AddText(ChatUser user, string msg)
        {
            try
            {
                string fileName = user.SenderId + user.SenderType + "_" + user.RecieverId + user.ReceiverType + "_" + user.RequestId + ".txt";
                string FilePath = "wwwroot\\Upload\\ChatFile\\" + fileName;

                ChatJsonObject chatJsonObject = new ChatJsonObject
                {
                    Message = msg,
                    Datetime = DateTime.Now,
                    RequestId = user.RequestId
                };

                if (user.SenderType == "Admin" || user.ReceiverType == "Admin")
                {
                    chatJsonObject.AdminId = user.SenderType == "Admin" ? user.SenderId : user.RecieverId;
                }
                 if (user.SenderType == "Provider" || user.ReceiverType == "Provider")
                {
                    chatJsonObject.PhysicianId = user.SenderType == "Provider" ? user.SenderId : user.RecieverId;
                }

                // Serialize the ChatJsonObject to JSON
                string json = JsonConvert.SerializeObject(chatJsonObject);

                // Append the JSON data to the file
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    sw.WriteLine(json);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return true;
        }

        public async Task<List<ChatJsonObject>> CheckHistory(ChatUser user)
        {
            try
            {
                var data = _context.Chatlogs.Where(e => e.Requestid == user.RequestId && e.Recieverid == user.RecieverId && e.Senderid == user.SenderId).FirstOrDefault();
                if (data == null)
                {
                    var Chatlog = new Chatlog();
                    Chatlog.Requestid = user.RequestId;
                    Chatlog.Sendername = user.SenderName;
                    Chatlog.Sendertype = user.SenderType;
                    Chatlog.Senderid = user.SenderId;
                    Chatlog.Receivertype = user.ReceiverType;
                    Chatlog.Recieverid = user.RecieverId;
                    Chatlog.Recievername = user.RecieverName;
                    Chatlog.Filepath = CreateTextFile(user);
                    Chatlog.Createddate = DateTime.Now;
                    _context.Chatlogs.Add(Chatlog);
                    _context.SaveChanges();

                    return ReadTextFile(data.Filepath);
                }
                else
                {
                    return ReadTextFile(data.Filepath);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return new List<ChatJsonObject> { };
        }
    }
}
