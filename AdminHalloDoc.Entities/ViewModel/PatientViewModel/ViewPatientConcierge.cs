using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientConcierge
    {
        [Required(ErrorMessage = "CON_FirstName Is Required!")]
        public string CON_FirstName { get; set; }
        [Required(ErrorMessage = "CON_LastName Is Required!")]
        public string CON_LastName { get; set; }
        [Required(ErrorMessage = "Symptoms Is Required!")]
        public string CON_PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string CON_Email { get; set; }
        public string? CON_PropertyName { get; set; }
        public string? Id { get; set; } = null!;
        [Required(ErrorMessage = "Symptoms Is Required!")]
        public string Symptoms { get; set; }
        [Required(ErrorMessage = "FirstName Is Required!")]
        public string FirstName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string PhoneNumber { get; set; }
        public string? CON_Street { get; set; }
        public string? CON_City { get; set; }
        public string? CON_State { get; set; }
        public string? CON_ZipCode { get; set; }
        public string? RoomSite { get; set; }
    }
}
