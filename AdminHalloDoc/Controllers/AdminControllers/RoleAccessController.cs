﻿using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class RoleAccessController : Controller
    {
        #region Constoter
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        private readonly IRoleAccessRepository _roleAccessRepository;
        public RoleAccessController(IPhysicianRepository physicianRepository, IViewActionRepository viewActionRepository,EmailConfiguration emailConfiguration,IRoleAccessRepository roleAccessRepository)
        {

            _viewActionRepository = viewActionRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
            _roleAccessRepository = roleAccessRepository;
        }
        #endregion

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
        public async Task<IActionResult> UserAccess()
        {

            TempData["Status"] = TempData["Status"];
            List<ViewUserAcces> v = await _roleAccessRepository.GetAllUserDetails();
            return View("../AdminViews/RoleAccess/UserAccess", v);
        }
        #endregion
    }
}
