using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class CloseCase
    {
        public List<Documents>? documentslist { get; set; } = null;
        public string Firstanme { get; set; }
        public string Lastanme { get; set; }
        public string? ConfirmationNumber { get; set; }
        public int RequestID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string RequestBody { get; set; }

        public class Documents
        {
            public string? Uploader { get; set; }
            public int? Status { get; set; }
            public string? Filename { get; set; }
            public DateTime Createddate { get; set; }
            public int? RequestwisefilesId { get; set; }

        }
    }
}
