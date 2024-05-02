﻿using AdminHalloDoc.Entities.Models;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class RecordsModel
    {
        //Search Record
        public List<ViewSearchRecord>? SearchRecordList { get; set; }
        public List<User>? PatientHistorybList { get; set; }
        public List<Emaillogdata>? EmailLogList { get; set; }
        public List<SMSLogsData>? SMSLogList { get; set; }
        public List<BlockRequestData>? BlockRequestList { get; set; }

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

        //Email Logs
        public int? AccountType { get; set; }
        public string? ReciverName { get; set; }
    }
}
