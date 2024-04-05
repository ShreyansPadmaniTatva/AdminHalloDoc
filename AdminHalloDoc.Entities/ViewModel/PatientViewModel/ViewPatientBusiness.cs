using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientBusiness
    {
        [Required(ErrorMessage = "FirstName Is Required!")]
        public string BUP_FirstName { get; set; }
        [Required(ErrorMessage = "LastName Is Required!")]
        public string BUP_LastName { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string BUP_PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string BUP_Email { get; set; }
        public string? BUP_PropertyName { get; set; }
        public string? BUP_CaseNumber { get; set; }
        public string? Id { get; set; } = null!;
        [Required(ErrorMessage = "Symptoms Is Required!")]
        public string Symptoms { get; set; }
        [Required(ErrorMessage = "FirstName Is Required!")]
        public string FirstName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "LastName Is Required!")]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? RoomSite { get; set; }
    }
}
