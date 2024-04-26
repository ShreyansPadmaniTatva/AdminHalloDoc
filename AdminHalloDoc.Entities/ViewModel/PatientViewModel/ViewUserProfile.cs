using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminHalloDoc.Entities.ViewModel.PatientViewModel
{
    public class ViewUserProfile
    {
        public int? Userid { get; set; }

        public string? Aspnetuserid { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$).+", ErrorMessage = "Enter a valid Name")]
        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public BitArray? Ismobile { get; set; }

        public string? Street { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter valid City")]
        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid city name")]
        public string? City { get; set; }

        public string? State { get; set; }

        public int? Regionid { get; set; }
        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        public string? Zipcode { get; set; }

        public string? Strmonth { get; set; }
        public DateTime? Birthdate { get; set; }

        public int? Intyear { get; set; }

        public int? Intdate { get; set; }

        public string Createdby { get; set; } = null!;

        public DateTime Createddate { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        public short? Status { get; set; }

        public string? Ip { get; set; }

    }
}
