using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewOrder
    {
        public int RequestId { get; set; }
        public int? OrderId { get; set; }
        public int? VenderTypeId { get; set; }
        public int? VenderId { get; set; }
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Enter valid Business Contact")]
        [RegularExpression(@"^\+(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid Business Contact")]
        [Required(ErrorMessage = "Plese enter your Business Contact")]
        public string? BusinessContact { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@$", ErrorMessage = "Enter a valid email address with valid domain")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please Enter your Fax Number")]
        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "Please Enter your Prescription")]
        public string? Prescription { get; set; }
        public int? Refills { get; set; }
        public string? UserId { get; set; }
    }
}
