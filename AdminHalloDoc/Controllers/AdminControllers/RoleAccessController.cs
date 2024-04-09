using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class RoleAccessController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        private readonly IRoleAccessRepository _roleAccessRepository;
        public RoleAccessController(IMyProfileRepository myProfileRepository, IPhysicianRepository physicianRepository, IViewActionRepository viewActionRepository,EmailConfiguration emailConfiguration,IRoleAccessRepository roleAccessRepository, IRequestRepository requestRepository)
        {

            _viewActionRepository = viewActionRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
            _roleAccessRepository = roleAccessRepository;
            _requestRepository = requestRepository;
            _myProfileRepository = myProfileRepository;
        }
        #endregion

       // [AdminAuth("Admin")]
        #region Role_Access
        public async Task<IActionResult> Index()
        {
            
            TempData["Status"] = TempData["Status"];
          List<Role> v = await  _roleAccessRepository.GetRoleAccessDetails();
            return View("../AdminViews/RoleAccess/Index",v);
        }
        #endregion

        #region Create_Role_Access-ADDEdit

        public async Task<IActionResult> CreateRoleAccess(int? id)
        {
            if (id != null)
            {
                ViewData["RolesAddEdit"] = "Edit";
                ViewRoleByMenu v = await _roleAccessRepository.GetRoleByMenus((int)id);
                return View("../AdminViews/RoleAccess/CreateRoleAccess", v);
            }
            ViewData["RolesAddEdit"] = "Add";

            return View("../AdminViews/RoleAccess/CreateRoleAccess");
        }
        #endregion

        #region GetMenusByAccount
        public async Task<IActionResult> GetMenusByAccount(short Accounttype, int roleid)
        {
            List<Menu> v = await _roleAccessRepository.GetMenusByAccount(Accounttype);

            if (roleid != null)
            {
                List<ViewRoleByMenu.Menu> vm = new List<ViewRoleByMenu.Menu>();
                List<int> rm = await _roleAccessRepository.CheckMenuByRole(roleid); 
                foreach (var item in v)
                {
                    ViewRoleByMenu.Menu menu = new ViewRoleByMenu.Menu();
                    menu.Name = item.Name;
                    menu.Menuid = item.Menuid;

                    if (rm.Contains(item.Menuid))
                    {
                        menu.checekd = "checked";
                        vm.Add(menu);
                    }
                    else
                    {
                        vm.Add(menu);
                    }
                }
                return Json(vm);
            }

            return Json(v);
        }
        #endregion

        #region PostRoleMenu
        public async Task<IActionResult> PostRoleMenu(ViewRoleByMenu role ,string Menusid)
        {
            bool data = false;

            if (role.Roleid == 0)
            {
                data = await _roleAccessRepository.PostRoleMenu(role, Menusid , CV.ID());

            }
            else
            {
                data = await _roleAccessRepository.PutRoleMenu(role, Menusid, CV.ID());
            }

            if (data)
            {
                TempData["Status"] = "Role Add Successfully...";
            }
            else
            {
                TempData["Status"] = "Role not Add...";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region User_Access
        public async Task<IActionResult> UserAccess(int? User)
        {

            TempData["Status"] = TempData["Status"];
            List<ViewUserAcces> v = await _roleAccessRepository.GetAllUserDetails(User);
            if(User != null)
            {
                return Json(v);
            }
            return View("../AdminViews/RoleAccess/UserAccess", v);
        }
        #endregion

        #region PhysicianAddEdit-ADDEdit

        public async Task<IActionResult> PhysicianAddEdit(int? id)
        {

            //TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox(3);
            if (id == null)
            {
                ViewData["PhysicianAccount"] = "Add";
            }
            else
            {

                ViewData["PhysicianAccount"] = "Edit";
                Physicians v = await _physicianRepository.GetPhysicianById((int)id);
                return View("../AdminViews/Physician/PhysicianAddEdit", v);

            }
            return View("../AdminViews/Physician/PhysicianAddEdit");
        }
        #endregion

        #region AdminAddEdit-ADDEdit

        public async Task<IActionResult> AdminAddEdit(int? id)
        {
            

            //TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
            if (id == null)
            {
                ViewData["AdminAccount"] = "Add Admin";
            }
            else
            {
                ViewData["AdminAccount"] = "Edit Admin";

                ViewAdminProfile p = await _myProfileRepository.GetProfileDetails((int)id);
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
                return View("../AdminViews/RoleAccess/AdminAddEdit", p);

            }
            return View("../AdminViews/RoleAccess/AdminAddEdit");
        }
        #endregion

        #region Creat_Admin
        [HttpPost]
        public async Task<IActionResult> AdminAdd(ViewAdminProfile vm)
        {
            //TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
            // bool b = physicians.Isagreementdoc[0];

            if (ModelState.IsValid)
            {
                await _myProfileRepository.AdminPost(vm, CV.ID());

            }
            else
            {
                return View("../AdminViews/RoleAccess/AdminAddEdit", vm);
            }

            return RedirectToAction("UserAccess");
        }
        #endregion

        #region Update_Profile_Admin

        #region SaveAdminInfo
        public async Task<IActionResult> SaveAdminInfo(ViewAdminProfile vm)
        {
            string actionName = RouteData.Values["action"].ToString();
            string actionNameaq = ControllerContext.ActionDescriptor.ActionName; // Get the current action name

            bool data = await _myProfileRepository.SaveAdminInfo(vm);
            if (data)
            {
                TempData["Status"] = "Administration Information Changed...";
            }
            else
            {
                TempData["Status"] = "Imformation not Changed properly...";
            }

            return RedirectToAction("AdminAddEdit", new { id = vm.AdminId });
        }
        #endregion

        #region SaveAdministrationinfo
        public async Task<IActionResult> SaveAdministrationinfo(ViewAdminProfile vm)
        {


            bool data = await _myProfileRepository.EditAdminProfileAsync(vm);
            if (data)
            {
                TempData["Status"] = "Administration Information Changed...";
            }
            else
            {
                TempData["Status"] = "Imformation not Changed properly...";
            }

            return RedirectToAction("AdminAddEdit", new { id = vm.AdminId });
        }
        #endregion

        #region EditBillingInfo
        public async Task<IActionResult> EditBillingInfo(ViewAdminProfile vm)
        {



            bool data = await _myProfileRepository.EditBillingInfoAsync(vm);
            if (data)
            {
                TempData["Status"] = "Billing Information Changed...";
            }
            else
            {
                TempData["Status"] = "Billing not Changed properly...";
            }

            return RedirectToAction("AdminAddEdit", new { id = vm.AdminId });
        }
        #endregion

        #region ResetPassAdmin
        public async Task<IActionResult> ResetPassAdmin(string password,int AdminId)
        {



            bool data = await _myProfileRepository.ChangePasswordAsync(password, AdminId);
            if (data)
            {
                TempData["Status"] = "Password changed Successfully...";
            }
            else
            {
                TempData["Status"] = "Password not Changed...";
            }

            return RedirectToAction("AdminAddEdit", new { id = AdminId });
        }
        #endregion

        #endregion

        #region CheckUsernameAvailability

        public IActionResult CheckUsernameAvailability(string username)
        {
            
            return Json(new
            {
               
                Message = _myProfileRepository.IsUsernameAvailable(username)
        });
        }
        #endregion
    }
}
