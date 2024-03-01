using AdminHalloDoc.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class TransfernotesModel
    {
        public int Requeststatuslogid { get; set; }
        public int Requestid { get; set; }
        public int? Physicianid { get; set; }
        public int? Transtophysicianid { get; set; }
        public DateTime Createddate { get; set; }
        public string? Notes { get; set; }
        public BitArray? Transtoadmin { get; set; }
        public short? Status { get;  set; }
        public string? Admin { get;  set; }
        public string? Physician { get;  set; }
        public string? TransPhysician { get;  set; }
        public string TransferNotes => $"{Admin} transferred  <b> {Physician}  </b> to <b> {TransPhysician} </b> on {Createddate}: <b>{Notes}</b>";
    }
}
