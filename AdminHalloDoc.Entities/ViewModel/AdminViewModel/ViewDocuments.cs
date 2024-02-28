using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewDocuments
    {
        public List<Documents>? documentslist { get; set; } = null;
        public string Firstanme { get; set; }
        public string Lastanme { get; set; }
        public string? ConfirmationNumber { get; set; }
        public int RequestID { get; set; }
        public class Documents
        {
            public string? Uploader { get; set; }
            public int? Status { get; set; }
            public string? Filename { get; set; }
            public DateTime Createddate { get; set; }

        }
    }

}
