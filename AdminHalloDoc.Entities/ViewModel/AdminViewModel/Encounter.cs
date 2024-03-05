using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class Encounter
    {
        public int? Requesid { get; set; }
        public int? RequesClientid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Location { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? Date { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? HistoryOfP { get; set; }
        public string? HistoryOfMedical { get; set; }
        public string? Medications { get; set; }
        public string? Allergies { get; set; }
        public string? Temp { get; set; }
        public string? Hr { get; set; }
        public string? Rr { get; set; }
        public string? BloodPressureS { get; set; }
        public string? BloodPressureD { get; set; }
        public string? O2 { get; set; }
        public string? Pain { get; set; }
        public string? Heent { get; set; }
        public string? CV { get; set; }
        public string? Chest { get; set; }
        public string? ABD { get; set; }
        public string? Extr { get; set; }
        public string? Skin { get; set; }
        public string? Neuro { get; set; }
        public string? Other { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? MedicationsDispensed { get; set; }
        public string? Procedures { get; set; }
        public string? Followup { get; set; }
       
    }
}
