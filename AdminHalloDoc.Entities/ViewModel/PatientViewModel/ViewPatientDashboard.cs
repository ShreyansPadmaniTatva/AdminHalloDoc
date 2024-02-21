using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientDashboard
    {
        public int? Requestid { get; set; }
        public int? Status { get; set; }
        public DateTime? Createddate { get; set; }
        public int? FileCount { get; set; }
        public string? Filename { get; set; }

    }
}
