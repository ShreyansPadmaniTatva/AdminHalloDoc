using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewDashboardList
    {
        public int? Requestid { get; set; }
        public int? RequestClientid { get; set; }
        public int? Status { get; set; }
        public string? PatientName { get; set; }
        public DateTime? Dob { get; set; }
        public string? PatientId { get; set; }
        public string? Requestor { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? RequestorPhoneNumber { get; set; }
        public string? requesttypecolor { get; set; }

        public int? RequestTypeID { get; set; }
        public int? RegionID { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }

        public int? ProviderID { get; set; }

    }
}
