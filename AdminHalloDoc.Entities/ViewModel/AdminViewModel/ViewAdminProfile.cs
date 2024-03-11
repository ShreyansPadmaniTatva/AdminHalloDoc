using AdminHalloDoc.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewAdminProfile
    {

        public int? AdminId { get; set; }

        public string? Aspnetuserid { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public short? Status { get; set; }
        public int? Roleid { get; set; }
        public string? Firstname { get; set; } = null!;

        public string? Lastname { get; set; }

        public string? Email { get; set; } = null!;

        public string? Mobile { get; set; }
        public string? AltMobile { get; set; }



        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public int? Regionid { get; set; }
        public string? Regionsid { get; set; }
        public List<Regions>? Regionids { get; set; }

        public string? Zipcode { get; set; }



        public string? Createdby { get; set; } = null!;

        public DateTime? Createddate { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }


        public string? Ip { get; set; }

        public class Regions{
            public int? regionid { get; set; }
            public string? regionname { get; set; }
            
        }


    }
}
