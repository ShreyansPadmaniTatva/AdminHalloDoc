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
        public int Status { get; set; }
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
        public string? Physician { get; set;}
        public string? Confirmation { get; set; }
        public DateTime? ConcludedDate { get; set; }

    }
    public class PaginatedViewModel
    {
        public List<ViewDashboardList>? DashboardList { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //Extra Inputs
        public string? SearchInput { get; set; }
        public int? RegionId { get; set; }
        public int? RequestType { get; set; }
        public string? status { get; set; }
        public int? UserId { get; set; }
        public string SortedColumn { get; set; }
        public bool? IsAscending { get; set; }

        //Count
        public int NewRequest { get; set; }
        public int PendingRequest { get; set; }
        public int ActiveRequest { get; set; }
        public int ConcludeRequest { get; set; }
        public int ToCloseRequest { get; set; }
        public int UnpaidRequest { get; set; }

    }
}
