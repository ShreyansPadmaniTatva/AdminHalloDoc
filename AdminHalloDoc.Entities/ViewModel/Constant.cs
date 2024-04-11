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
        public enum EmailAction
        {
            Sendorder = 1, 
            Request, 
            SendLink, 
            SendAgreement, 
            Forgot, 
            NewRegistration, 
            contact

        }
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
                  UrlList = new List<string> { "/Admin/DashBoard", "/ViewAction", "/SubmitForm","/Admin/DashBoard" },
                   Submenu = null
              },
               new MenuItem
              {
                  DbName ="PhysicianDashbord",
                  Label = "Dashboard",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Physician/DashBoard",
                   Submenu = null

              },
              new MenuItem {
                  DbName ="Provider Location",
                  Label = "Provider Location",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Physician/PhysicianLocation",
                  Submenu = null
              },
              new MenuItem {
                  DbName ="MyProfile-Admin",
                  Label = "My Profile",
                  Url = "/Admin/Profile",
                  ContollerAction ="/AdminProfile/Index",
                  Submenu = null

              },
               new MenuItem {
                  DbName ="MyProfile-Physician",
                  Label = "My Profile",
                  Url = "/Physician/Profile",
                  ContollerAction ="/AdminProfile/Index",
                  Submenu = null

              },
              new MenuItem {
                  DbName ="MyShedule",
                  Label = "My Schedule",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Scheduling/Index",
                  Submenu = null
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
                UrlList = new List<string> { "/Partner/Index", "/Partner/PartnerAddEdit", "/Partner" },
                Submenu = null
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
                  ContollerAction ="/Repsworts/Index",
                  Url = "/Repowsrts/Index",
                  Submenu = new List < MenuItem > {

                    new MenuItem {
                        DbName="Search Records",
                      Label = "Search Records", Url = "/Reports/Index",
                       ContollerAction ="/Reports/Index",
                    },
                    new MenuItem {
                        DbName="Email Log",
                      Label = "Email Log", Url = "/Reports/EmailLog"
                    },
                    new MenuItem {
                        DbName="SMS Log",
                      Label = "SMS Log", Url = "/Reports/SMSLog"
                    },
                    new MenuItem {
                        DbName="Patient Record",
                      Label = "Patient Record", Url = "/Reports/PatientHistory"
                    },
                    new MenuItem
                    {
                        DbName="Block History",
                      Label = "Block History", Url = "/Reports/BlockHistory"
                    }
                  }
              }
            }
        };


        public static Dictionary<int, List<string>> statusTdHtmlMap = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Name", "Date of Birth", "Requestor", "Requested Date", "Phone" ,"Address", "Notes" ,  "Actions" } }, 
            { 2, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name",  "Date Of Service", "Phone" ,"Address", "Notes" , "Actions" } }, 
            { 3, new List<string> { "Name", "Date of Birth", "Requestor", "Physician Name", "Date Of Service", "Phone", "Address", "Notes" , "Actions" } }, 
            { 4, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service", "Phone", "Address" , "Actions" } }, 
            { 5, new List<string> { "Name", "Date of Birth", "Physician Name", "Date Of Service" ,"Address", "Notes" , "Actions" } }, 
            { 6, new List<string> { "Name", "Physician Name", "Date Of Service", "Phone" ,"Address" , "Actions" } }

        };

        public static Dictionary<int, List<string>> PstatusTdHtmlMap = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Name",  "Phone" ,"Address", "Actions" } },
            { 2, new List<string> { "Name",  "Phone" ,"Address", "Actions" } },
            { 3, new List<string> { "Name",  "Phone" ,"Address", "Status", "Actions" } },
            { 4, new List<string>  { "Name",  "Phone" ,"Address", "Actions" } }

        };
    }
}
