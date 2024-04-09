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
        [Required]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Street { get; set; }
       
        [Required(ErrorMessage = "City is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        public string ZipCode { get; set; }
        [Required]
        public string RoomSite { get; set; }

        public string? UploadImage { get; set; }
       
        public IFormFile? UploadFile { get; set; }


        public string? Realtion { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }

        // not mandatory
        public int? date { get; set; }
        public int? year { get; set; }
        public string? month { get; set; }
    }
}
