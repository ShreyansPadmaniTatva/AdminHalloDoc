using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class PushNotification
    {
        public string ClientName { get; set; }
        public string EndPoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }

    }
}
