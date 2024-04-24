using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class PhysicianPayrate
    {
        public int? PayrateId { get; set; }
        public int? PhysicianId { get; set; }
        public decimal? Payrate { get; set; }
        public string? Category { get; set; }
       
        public DateTime? CreatedDate { get; set; }

    }
}
