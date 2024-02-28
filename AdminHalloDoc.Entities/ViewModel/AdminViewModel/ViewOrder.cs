using System;
using System.Collections.Generic;
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
        public string? BusinessContact { get; set; }
        public string? Email { get; set; }
        public string? FaxNumber { get; set; }
        public string? Prescription { get; set; }
        public int? Refills { get; set; }
    }
}
