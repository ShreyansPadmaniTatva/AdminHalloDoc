using AdminHalloDoc.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IPushNotificationRepository
    {
        Task<bool> AddNotificationUserData(string client, string endpoint, string p256dh, string auth);
        Task<List<PushNotification>> GetUserDataList();
        PushNotification GetUserData(string aspid);
    }
}
