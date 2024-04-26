using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.Constant;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class LoginRepository : ILoginRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public LoginRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region CheckAccessLogin
        /// <summary>
        /// Check User With Role
        /// </summary>
        /// <param name="aspNetUser"></param>
        /// <returns></returns>
        public async Task<UserInfo> CheckAccessLogin(Aspnetuser aspNetUser)
        {
            var user = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Username == aspNetUser.Username);

            

            UserInfo admin = new UserInfo();
            if (user != null)
            {
                var hasher = new PasswordHasher<string>();
                PasswordVerificationResult result = hasher.VerifyHashedPassword(null, user.Passwordhash, aspNetUser.Passwordhash);
                if (result != PasswordVerificationResult.Success)
                {

                    return null;
                }
                else
                {
                    var data = _context.Aspnetuserroles.FirstOrDefault(E => E.Userid == user.Id);
                    var datarole = _context.Aspnetroles.FirstOrDefault(e => e.Id == data.Roleid);

                    admin.ID = user.Id;
                    admin.Username = user.Username;
                    admin.FirstName = admin.FirstName ?? string.Empty;
                    admin.LastName = admin.LastName ?? string.Empty;
                    admin.Role = datarole.Name;
                    if (admin.Role == "Admin")
                    {
                        var admindata = _context.Admins.FirstOrDefault(u => u.Aspnetuserid == user.Id);
                        admin.UserId = admindata.Adminid;
                    admin.RoleId = (int)admindata.Roleid;
                    }
                    else if (admin.Role == "Patient")
                    {
                        var admindata = _context.Users.FirstOrDefault(u => u.Aspnetuserid == user.Id);
                        admin.UserId = admindata.Userid;


                    }
                    else
                    {
                        var admindata = _context.Physicians.FirstOrDefault(u => u.Aspnetuserid == user.Id);
                        admin.UserId = admindata.Physicianid;
                        admin.RoleId = (int)admindata.Roleid;

                    }

                    return admin;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region SetSubMenu
        /// <summary>
        /// Check That SubMenu Define Or Not
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menusub"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<MenuItem> SetSubMenu(int? roleid, int menusub, List<MenuItem> s)
        {
            List<MenuItem> StaticSubmenu = new List<MenuItem>();

            List<Menu> MenuItemsSub = (from rm in _context.Rolemenus
                                     join Menus in _context.Menus
                                     on rm.Menuid equals Menus.Menuid into MenusGroup
                                     from men in MenusGroup.DefaultIfEmpty()
                                     where rm.Roleid == roleid && men.Sortorder.ToString().StartsWith(""+menusub+"")
                                     orderby men.Sortorder
                                     select men).ToList();
            foreach (Menu menu in MenuItemsSub)
            {
                MenuItem m = new MenuItem();
                m = s.Where(item => item.DbName == menu.Name).FirstOrDefault();
                if (m != null)
                {
                    StaticSubmenu.Add(m);
                }
            }
                return StaticSubmenu;
        }
        #endregion

        #region For_Dynamic_Menu
        /// <summary>
        /// This Class for Define Or Store all Static Menu
        /// this class created here because This Class can not in Constant With  Static KeyWord !imp
        /// </summary>
        public class MenuItem
        {
            public string DbName { get; set; }
            public string Label { get; set; }
            public string Url { get; set; }
            public string ContollerAction { get; set; }
            public List<string> UrlList { get; set; }
            public List<MenuItem> Submenu { get; set; }
        }

        /// <summary>
        /// List Out The All Static Menu
        /// </summary>
        public  List<MenuItem> staticmenu = new List<MenuItem>
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
                    UrlList = new List<string> { "/Physician/DashBoard", "/ViewAction", "/SubmitForm","/Admin/DashBoard" },
                   Submenu = null

              },
              new MenuItem {
                  DbName ="Provider Location",
                  Label = "Provider Location",
                  ContollerAction ="/Physician/PhysicianLocation",
                  Url = "/Admin/PhysicianLocation",
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
                  UrlList = new List<string> { "/Physician/Profile" },
                  Submenu = null

              },
              new MenuItem {
                  DbName ="MyShedule",
                  Label = "My Schedule",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "/Physician/Scheduling",
                   UrlList = new List<string> { "/Physician/Scheduling" },
                  Submenu = null
              },
              new MenuItem {
                  DbName = "Provider-Details",
                  Label = "Provider",
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "#",
                   UrlList = new List<string> { "/Admin/PhysicianAll", "/Admin/Scheduling" },
                  Submenu = new List < MenuItem > {
                    new MenuItem {
                        DbName = "Provider",
                      Label = "Provider", Url = "/Admin/PhysicianAll",
                       UrlList = new List<string> { "/Admin/PhysicianAll" },
                        ContollerAction ="/Physician/PhysicianAll",
                    },
                    new MenuItem {
                        DbName = "Scheduling",

                      Label = "Scheduling",
                        Url = "/Admin/Scheduling",
                       UrlList = new List<string> { "/Admin/Scheduling" },
                        ContollerAction = "/Scheduling/Index",

                    },
                    new MenuItem {
                        DbName = "Invoicing",
                      Label = "Invoicing",
                        Url = "/Invoice/AdminIndex",
                       UrlList = new List<string> { "/Invoice/Index","/Invoice/TimeSheetAddEdit" },
                        ContollerAction ="/Reports",
                    }
                  }
              },
              new MenuItem {
                        DbName = "P-Invoicing",
                      Label = "Invoicing",
                        Url = "/Invoice/Index",
                       UrlList = new List<string> { "/Invoice/Index" },
                        ContollerAction ="/Reports",
                    },
              new MenuItem {
                  DbName ="Partner",
                Label = "Partner",
                  ContollerAction ="/Partner/Index",
                  Url = "/Admin/Partner",
                UrlList = new List<string> { "/Admin/Partner", "/Partner/PartnerAddEdit", "/Partner" },
                Submenu = null
              },
              new MenuItem {
                  DbName ="Access-Details",
                Label = "Access",
                 UrlList = new List<string> { "/RoleAccess/AdminAddEdit", "/RoleAccess/CreateRoleAccess", "/RoleAccess/CreateRoleAccess","/RoleAccess/PhysicianAddEdit" },
                  ContollerAction ="/AdminDashboard/Index",
                  Url = "#",
                  Submenu = new List < MenuItem > {

                    new MenuItem {
                        DbName="Account Access",
                      Label = "Account Access", Url = "/Admin/AccountAccess",
                 UrlList = new List<string> { "/RoleAccess/AdminAddEdit", "/RoleAccess/CreateRoleAccess", "/RoleAccess/CreateRoleAccess","/RoleAccess/PhysicianAddEdit" },

                       ContollerAction ="/RoleAccess/Index",
                    },
                    new MenuItem {
                        DbName="User Access",
                      Label = "User Access", Url = "/Admin/UserAccess",
                 UrlList = new List<string> { "/RoleAccess/AdminAddEdit", "/RoleAccess/CreateRoleAccess", "/RoleAccess/CreateRoleAccess","/RoleAccess/PhysicianAddEdit" },

                       ContollerAction ="/RoleAccess/UserAccess",
                    }
                  }
              },
              new MenuItem {
                  DbName ="Records",
                Label = "Records",
                  ContollerAction ="/Reports/Index",
                  Url = "/Reports/Index",
                  Submenu = new List < MenuItem > {

                    new MenuItem {
                        DbName="Search Records",
                      Label = "Search Records", Url = "/Reports/Index",
                       ContollerAction ="/Reports",
                    },
                    new MenuItem {
                        DbName="Email Log",
                      Label = "Email Log", Url = "/Reports/EmailLog",
                       ContollerAction ="/Reports",
                    },
                    new MenuItem {
                        DbName="SMS Log",
                      Label = "SMS Log", Url = "/Reports/SMSLog",
                       ContollerAction ="/Reports",
                    },
                    new MenuItem {
                        DbName="Patient Record",
                      Label = "Patient Record", Url = "/Reports/PatientHistory",
                       ContollerAction ="/Reports",
                    },
                    new MenuItem
                    {
                        DbName="Block History",
                      Label = "Block History", Url = "/Reports/BlockHistory",
                       ContollerAction ="/Reports",
                    }
                  }
              }

        };
        #endregion

        #region SetMenu
        /// <summary>
        /// With Roleid Get Menu With Database = Static Menu 
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public List<MenuItem> SetMenu(int? roleid)
        {
            List<Menu> MenuItems = null;
            List<MenuItem> Staticmenu = new List<MenuItem>();
            List<MenuItem> Staticmenudummy = new List<MenuItem>();
            Staticmenudummy = staticmenu;
            if (roleid != null)
            {
                //Set By DataBase
                MenuItems =  (from rm in _context.Rolemenus
                                 join Menus in _context.Menus
                                 on rm.Menuid equals Menus.Menuid into MenusGroup
                                 from m in MenusGroup.DefaultIfEmpty()
                                 where rm.Roleid == roleid
                                 orderby m.Sortorder
                                 select m).ToList();

                //Set By DataBase And Static Menu
                foreach (Menu menu in MenuItems)
                {
                    MenuItem m = new MenuItem();
                    m = staticmenu.Where(item => item.DbName == menu.Name).FirstOrDefault();
                   
                    if (m != null)
                    {
                        if (m.Submenu != null)
                        {
                            m.Submenu = SetSubMenu(roleid, (int)menu.Sortorder,m.Submenu);
                        }
                        Staticmenu.Add(m);
                    }

                }
            }
            else
            {
                return Staticmenu;
            }

            return Staticmenu;
        }

        #endregion

        #region
        public bool IsPasswordModify(string? email)
        {
            if (email != null)
            {
                DateTime? modifiedDate = _context.Aspnetusers.Where(r => r.Email == email).FirstOrDefault().Modifieddate;

                if (modifiedDate == null)
                {
                    return true;
                }

                if (modifiedDate != null && DateTime.UtcNow.Subtract(modifiedDate.Value).TotalHours <= 24)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
