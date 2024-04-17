using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientFamilyFriend
    {
        [Required(ErrorMessage = "FirstName Is Required!")]
        public string FF_FirstName { get; set; }
        [Required(ErrorMessage = "LastName Is Required!")]
        public string FF_LastName { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string FF_PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string FF_Email { get; set; }
        public string? FF_RelationWithPatient { get; set; }
        public string? Id { get; set; } = null!;
        [Required(ErrorMessage = "Symptoms Is Required!")]
        public string Symptoms { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string FirstName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber Is Required!")]
        public string PhoneNumber { get; set; }
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
        public string? UploadImage { get; set; }
        public IFormFile? UploadFile { get; set; }
        public int? RegionId { get; set; }
    }
}
