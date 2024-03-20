﻿using AdminHalloDoc.Entities.Models;
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
        { "MyShedule", "/AdminDashboard/Index" },
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
            public bool IsActive { get; set; }
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
                  Url = "/AdminDashboard/Index",
                  //IsActive = path.StartsWith("/AdminDashboard") || path.StartsWith("/ViewAction") || path.StartsWith("/SubmitForm")
              },
              new MenuItem {
                  DbName ="Provider Location",
                Label = "Provider Location",
                  Url = "/Physician/PhysicianLocation",
                  // IsActive = path.StartsWith("/Physician/PhysicianLocation")
              },
              new MenuItem {
                  DbName ="AdminDashboard",
                Label = "My Profile",
                  Url = "/AdminProfile/Index",
                  //  IsActive = path.StartsWith("/AdminProfile")
              },
              new MenuItem {
                  DbName = "Provider-Details",
                Label = "Provider",
                  Url = "#",
                  // IsActive = path.StartsWith("/Physician"),
                  Submenu = new List < MenuItem > {
                    new MenuItem {
                        DbName = "Provider",
                      Label = "Provider", Url = "/Physician/PhysicianAll"
                    },
                    new MenuItem {
                        DbName = "Scheduling",

                      Label = "Scheduling", Url = "/Scheduling/Index"
                    },
                    new MenuItem {
                        DbName = "nothing",    
                      Label = "Invoicing", Url = "#"
                    }
                  }
              },
              new MenuItem {
                  DbName ="Partner",
                Label = "Partner",
                  Url = "/Physician/PhysicianAll",
                  IsActive = false
              },
              new MenuItem {
                  DbName ="Access-Details",
                Label = "Access",
                  Url = "#",
                  //  IsActive = path.StartsWith("/RoleAccess"),
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
                  Url = "/Patient_Profile.html",
                  IsActive = false
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
