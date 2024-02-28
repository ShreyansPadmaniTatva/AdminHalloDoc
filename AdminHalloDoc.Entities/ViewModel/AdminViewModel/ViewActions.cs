﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewActions
    {
        public int? RequestID { get; set; }
        public string? Notes { get; set; }
        public string? AdminNotes { get; set; }
        public string? PhysicianNotes { get; set; }
        public string? PatientName { get; set; }
        public int? RegionID { get;  set; }
        public int? ReasonID { get; set;}
        public string? ReasonTag { get; set; }
        public int? ProviderId { get; set; }
        public int? TransferToProviderId { get; set;}
    }
}
