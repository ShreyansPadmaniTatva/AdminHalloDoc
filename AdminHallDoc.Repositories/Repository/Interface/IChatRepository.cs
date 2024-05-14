using AdminHalloDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IChatRepository
    {
        Task<List<ChatJsonObject>> CheckHistory(ChatUser user);
        bool AddText(ChatUser user, string msg);
    }
}
