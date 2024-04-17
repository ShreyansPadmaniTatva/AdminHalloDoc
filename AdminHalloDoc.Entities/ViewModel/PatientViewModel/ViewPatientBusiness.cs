using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientBusiness
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string BUP_FirstName { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string BUP_LastName { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string BUP_PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        [StringLength(50)]
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
        [Required(ErrorMessage = "Please enter date of birth")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string Email { get; set; }
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Enter valid Mobile Number")]
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string PhoneNumber { get; set; }
        public int? RegionId { get; set; }
        public string? Street { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string? City { get; set; }
        public string? State { get; set; }
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        public string? ZipCode { get; set; }
        public string? RoomSite { get; set; }
    }
}
