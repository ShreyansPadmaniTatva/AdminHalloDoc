using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class ViewDashboardList 
    {
        [Required(ErrorMessage = "Symptoms is required")]
        public string Symptoms { get; set; }
       
        public string? FirstName { get; set; }

        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? PassWord { get; set; }
        [Required]
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
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string RoomSite { get; set; }

        public string? UploadImage { get; set; }


        public string? Realtion { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }

        // not mandatory
        public int? date { get; set; }
        public int? year { get; set; }
        public string? month { get; set; }
    }
}
