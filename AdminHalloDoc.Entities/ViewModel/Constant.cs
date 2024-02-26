using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class Constant
    {
        public enum RequestType
        {
            Business = 1,
            Patient,
            Family,
            Concierge
        }
        public enum AdminDashStatus
        {
            New = 1,
            Pending,
            Active,
            Conclude,
            ToClose,
            UnPaid
        }
    }
}
