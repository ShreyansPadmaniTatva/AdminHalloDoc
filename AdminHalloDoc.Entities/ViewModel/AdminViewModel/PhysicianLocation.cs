using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class PhysicianLocation
    {
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string? physicianname { get; set; }
        public int? locationid { get; set; }

    }
}
