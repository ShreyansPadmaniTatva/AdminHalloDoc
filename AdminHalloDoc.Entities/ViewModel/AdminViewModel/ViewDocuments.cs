using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewDocuments
    {
        public List<Documents>? documentslist { get; set; } = null;
        public string Firstanme { get; set; }
        public string Lastanme { get; set; }
        public string? ConfirmationNumber { get; set; }
        public int? RequestID { get; set; }
        public int RequesClientid { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|gov\.in)$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string? Email { get; set; }
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Enter valid Mobile Number")]
        [RegularExpression(@"^\+(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        [Required(ErrorMessage = "Plese enter your Phone Number")]
        public string? PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public class Documents
        {
            public string? Uploader { get; set; }
            public int? Status { get; set; }
            public string? Filename { get; set; }
            public DateTime Createddate { get; set; }
            public int? RequestwisefilesId { get; set; }
            public string isDeleted { get; set; }

        }
    }

}
