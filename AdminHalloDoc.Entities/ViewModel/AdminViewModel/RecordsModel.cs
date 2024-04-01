using AdminHalloDoc.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class RecordsModel
    {
        //Search Record
        public List<ViewSearchRecord>? SearchRecordList { get; set; }
        public List<User>? PatientHistorybList { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortedColumn { get; set; }
        public bool? IsAscending { get; set; }

        //Extra Inputs

        //Search Record
        public string? SearchInput { get; set; }
        public int? RegionId { get; set; }
        public int? RequestType { get; set; }
        public short? Status { get; set; }
        public string? Physicianname { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Patientname { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }

        //Patient Record
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
