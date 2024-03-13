using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel.AdminViewModel
{
    public class ViewRoleByMenu
    {
        public int Roleid { get; set; }

      
        public string Name { get; set; } = null!;

   
        public short Accounttype { get; set; }

     
        public string Createdby { get; set; } = null!;

      
        public DateTime Createddate { get; set; }

      
        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        
        public BitArray Isdeleted { get; set; } = null!;
        public List<Menu> Menus { get; set; }

        public class Menu
        {
            public int Menuid { get; set; }
            public string Name { get; set; }
            public string checekd { get; set; }
        }

    }
}
