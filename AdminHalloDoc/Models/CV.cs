namespace AdminHalloDoc.Models.CV
{
    public class CV
    {
        private static IHttpContextAccessor _httpContextAccessor;


        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public static int? RoleId()
        {
            string cookieValue;
            string RoleId = null;

            if (_httpContextAccessor.HttpContext.Request.Cookies["jwt"] != null)
            {
                cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["jwt"].ToString();

                RoleId = DecodedToken.DecodeJwt(DecodedToken.ConvertJwtStringToJwtSecurityToken(cookieValue)).claims.FirstOrDefault(t => t.Key == "RoleId").Value;
            }

            return Convert.ToInt32(RoleId);
        }

        public static string? role()
        {
            string cookieValue;
            string role = null;

            if (_httpContextAccessor.HttpContext.Request.Cookies["jwt"] != null)
            {
                cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["jwt"].ToString();

                role = DecodedToken.DecodeJwt(DecodedToken.ConvertJwtStringToJwtSecurityToken(cookieValue)).claims.FirstOrDefault(t => t.Key == "Role").Value;
            }

            return role;
        }


        public static string? UserName()
        {
            string cookieValue;
            string UserName = null;

            if (_httpContextAccessor.HttpContext.Request.Cookies["jwt"] != null)
            {
                cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["jwt"].ToString();

                UserName = DecodedToken.DecodeJwt(DecodedToken.ConvertJwtStringToJwtSecurityToken(cookieValue)).claims.FirstOrDefault(t => t.Key == "UserName").Value;
            }

            return UserName;
        }

        public static string? UserID()
        {
            string cookieValue;
            string UserID = null;

            if (_httpContextAccessor.HttpContext.Request.Cookies["jwt"] != null)
            {
                cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["jwt"].ToString();

                UserID = DecodedToken.DecodeJwt(DecodedToken.ConvertJwtStringToJwtSecurityToken(cookieValue)).claims.FirstOrDefault(t => t.Key == "UserID").Value;
            }

            return UserID;
        }

        public static string? ID()
        {
            string cookieValue;
            string UserID = null;

            if (_httpContextAccessor.HttpContext.Request.Cookies["jwt"] != null)
            {
                cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["jwt"].ToString();

                UserID = DecodedToken.DecodeJwt(DecodedToken.ConvertJwtStringToJwtSecurityToken(cookieValue)).claims.FirstOrDefault(t => t.Key == "ID").Value;
            }

            return UserID;
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

        public List<MenuItem> staticmenu = new List<MenuItem>
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
                       UrlList = new List<string> { "/Physician/PhysicianAll" },
                        ContollerAction ="/Reports",
                    },
                    new MenuItem {
                        DbName = "Scheduling",

                      Label = "Scheduling",
                        Url = "/Scheduling/Index",
                       UrlList = new List<string> { "/Scheduling/Index" },
                        ContollerAction ="/Reports",

                    },
                    new MenuItem {
                        DbName = "Invoicing",
                      Label = "Invoicing",
                        Url = "#",
                       UrlList = new List<string> { "/Scheduling/Index" },
                        ContollerAction ="/Reports",
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
                      Label = "Account Access", Url = "/RoleAccess/Index",
                       ContollerAction ="/Reports",
                    },
                    new MenuItem {
                        DbName="User Access",
                      Label = "User Access", Url = "/RoleAccess/UserAccess",
                       ContollerAction ="/Reports",
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
    }
}
