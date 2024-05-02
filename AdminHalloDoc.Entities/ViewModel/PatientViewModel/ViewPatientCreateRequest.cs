using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewPatientCreateRequest
    {
        [Required(ErrorMessage = "Symptoms is required")]
        public string Symptoms { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string FirstName { get; set; }

        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? PassWord { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter date of birth")]
        public DateTime BirthDate { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string Email { get; set; }
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Enter valid Mobile Number")]
        [RegularExpression(@"^\+(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Street is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Enter valid Street")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s.,'-]+$", ErrorMessage = "Enter a valid street address")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        public string ZipCode { get; set; }
        public string? RoomSite { get; set; }

        public string? UploadImage { get; set; }

        public IFormFile? UploadFile { get; set; }


        public string? Realtion { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int? RegionId { get; set; }

        // not mandatory
        public int? date { get; set; }
        public int? year { get; set; }
        public string? month { get; set; }
    }
}
