using AdminHalloDoc.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Entities.ViewModel
{
    public class Constant
    {
        public string Name { get; set; }
        public enum RequestType
        {
            Business = 1,
            Patient,
            Family,
            Concierge
        }
        public enum AdminDashStatus
        {
            New = 1,
            Pending,
            Active,
            Conclude,
            ToClose,
            UnPaid
        }
        public enum Status
        {
            Unassigne = 1,
            Accepted,
            Cancelled,
            MDEnRoute,
            MDONSite,
            Conclude,
            CancelledByPatients,
            Closed,
            Unpaid,
            Clear,
            Block

        }
        public enum AdminStatus
        {
            Pending = 1,
            Active,
            NotActive


        }

        public enum AccountType
        {
            All = 1,
            Admin,
            Physician,
            Patient
        }

        public static int FindStatus(int status)
        {
            if (status == 1)
            {
                return 1;
            }
            else if (status == 2)
            {
                return 2;
            }
            else if (status == 3 || status == 7 || status == 8)
            {
                return 5;
            }
            else if (status == 4 || status == 5)
            {
                return 3;
            }
            else if (status == 6)
            {
                return 4;
            }
            else
            {
                return 6;
            }

        }

        Dictionary<string, object> menuItems = new Dictionary<string, object>
    {
        { "MyShedule", "//AdminDashboard/Index" },
        { "MyProfile", "/Physician/PhysicianLocation" },
        { "3", "/AdminProfile/Index" },
        { "4", "/Physician/PhysicianAll" },
        { "5", new Dictionary<string, string>
            {
                { "Account Access", "/RoleAccess/Index" },
                { "User Access", "/RoleAccess/UserAccess" }
            }
        },
        // Add more menu items as needed
    };
        public class MenuItem
        {
            public string DbName { get; set; }
            public string Label { get; set; }
            public string Url { get; set; }
            public string ContollerAction { get; set; }
            public List<string> UrlList { get; set; }
            public List<MenuItem> Submenu { get; set; }
        }

        public  class StaticMenu
        {
            public List<MenuItem> Items
            {
                get;
                set;
            }
        }

        public static StaticMenu staticmenu = new StaticMenu
        {
            Items = new List<MenuItem>

            {
              new MenuItem
              {
                  DbName ="AdminDashboard",
                  Label = "Dashboard",
                  Url = "/Admin/DashBoard",
                  ContollerAction ="/AdminDashboard/Index",
                  UrlList = new List<string> { "/Admin/DashBoard", "/ViewAction", "/SubmitForm" }
              },
               new MenuItem
              {
                  DbName ="PhysicianDashbord",
                  Label = "Dashboard",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Physician/DashBoard",
                 
              },
              new MenuItem {
                  DbName ="Provider Location",
                  Label = "Provider Location",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Physician/PhysicianLocation",
                 
              },
              new MenuItem {
                  DbName ="MyProfile",
                  Label = "My Profile",
                  Url = "/AdminProfile/Index",
                  ContollerAction ="/AdminDashboard/Index",
               
              },
              new MenuItem {
                  DbName ="MyShedule",
                  Label = "My Schedule",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Scheduling/Index",
                
              },
              new MenuItem {
                  DbName = "Provider-Details",
                  Label = "Provider",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "#",
                   UrlList = new List<string> { "/Physician/PhysicianAll", "/Scheduling/Index", "/Partner" },
                  Submenu = new List < MenuItem > {
                    new MenuItem {
                        DbName = "Provider",
                      Label = "Provider", Url = "/Physician/PhysicianAll",
                       UrlList = new List<string> { "/Physician/PhysicianAll" }
                    },
                    new MenuItem {
                        DbName = "Scheduling",

                      Label = "Scheduling", 
                        Url = "/Scheduling/Index",
                       UrlList = new List<string> { "/Scheduling/Index" }

                    },
                    new MenuItem {
                        DbName = "Invoicing",    
                      Label = "Invoicing",
                        Url = "#",
                       UrlList = new List<string> { "/Scheduling/Index" }
                    }
                  }
              },
              new MenuItem {
                  DbName ="Partner",
                Label = "Partner",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Partner/Index",
                UrlList = new List<string> { "/Partner/Index", "/Partner/PartnerAddEdit", "/Partner" }
              },
              new MenuItem {
                  DbName ="Access-Details",
                Label = "Access",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "#",
                  Submenu = new List < MenuItem > {
                      
                    new MenuItem {
                        DbName="Account Access",
                      Label = "Account Access", Url = "/RoleAccess/Index"
                    },
                    new MenuItem {
                        DbName="User Access",
                      Label = "User Access", Url = "/RoleAccess/UserAccess"
                    }
                  }
              },
              new MenuItem {
                  DbName ="Records",
                Label = "Records",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Patient_Profile.html",
                  Submenu = new List < MenuItem > {

                    new MenuItem {
                        DbName="Search Records",
                      Label = "Search Records", Url = "/Reports/Index"
                    },
                    new MenuItem {
                        DbName="Email Log",
                      Label = "Email Log", Url = "/RoleAccess/UserAccess"
                    },
                    new MenuItem {
                        DbName="SMS Log",
                      Label = "SMS Log", Url = "/RoleAccess/Index"
                    },
                    new MenuItem {
                        DbName="Patient Record",
                      Label = "Patient Record", Url = "/Reports/PatientHistory"
                    },
                    new MenuItem
                    {
                        DbName="Block History",
                      Label = "Block History", Url = "/RoleAccess/UserAccess"
                    }
                  }
              }
            }
        };


        public static Dictionary<int, List<string>> statusTdHtmlMap = new Dictionary<int, List<string>>
    {
           // New
        { 1, new List<string> { "Name", "Date of Birth", "Requestor", "Requested Date", "Phone" ,"Address", "Notes" ,  "Actions" } }, 
        // Panding
        { 2, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name",  "Date Of Service", "Phone" ,"Address", "Notes" , "Actions" } }, 
        // Active
        { 3, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Actions" } }, 
        //{ 5, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Chat With" , "Actions" } }, 
        
        //Conclude
        { 4, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service", "Phone", "Address" , "Actions" } }, 

        // to close 
        { 5, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Actions" } }, 
       // { 7, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Chat With" , "Actions" } }, 
        //{ 8, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Chat With" , "Actions" } }, 

        //un paid
        { 6, new List<string> { "Name", "Physician Name", "Date Of Service", "Phone" ,"Address" , "Actions" } }

    };
    }
}
