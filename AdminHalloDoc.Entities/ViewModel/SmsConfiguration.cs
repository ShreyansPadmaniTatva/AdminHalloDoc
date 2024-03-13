using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class SmsConfiguration
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string Phonenumber { get; set; }
    }
}
