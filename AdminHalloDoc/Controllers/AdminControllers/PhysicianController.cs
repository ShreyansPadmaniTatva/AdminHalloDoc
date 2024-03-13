﻿using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class PhysicianController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        public PhysicianController(IPhysicianRepository physicianRepository,IMyProfileRepository myProfileRepository, IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, EmailConfiguration emailConfiguration)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
        }
        #endregion

        #region Physician_Location
        public async Task<IActionResult> PhysicianLocation()
        {
           ViewBag.Log = await _physicianRepository.FindPhysicianLocation();
            return View("../AdminViews/Physician/PhysicianLocation");
        }
        #endregion

        #region PhysicianAll
        public async Task<IActionResult> PhysicianAll(int? region)
        {
            TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            var v = await _physicianRepository.PhysicianAll();
            if (region == null)
            {
                v = await _physicianRepository.PhysicianAll();

            }
            else
            {
                v = await _physicianRepository.PhysicianByRegion(region);
                return Json(v);

            }
            return View("../AdminViews/Physician/Index",v);
        }
        #endregion

        #region ChangeNotificationPhysician
        public async Task<IActionResult> ChangeNotificationPhysician(string changedValues)
        {
            Dictionary<int, bool> changedValuesDict = JsonConvert.DeserializeObject<Dictionary<int, bool>>(changedValues);

            _physicianRepository.ChangeNotificationPhysician(changedValuesDict);

            return RedirectToAction("PhysicianAll");
        }
        #endregion

        #region PhysicianProfile
        public async Task<IActionResult> PhysicianProfile(int? id)
        {
            //TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
            if (id == null)
            {
                ViewData["PhysicianAccount"] = "Add";
            }
            else
            {

                ViewData["PhysicianAccount"] = "Edit";
                Physicians v = await _physicianRepository.GetPhysicianById((int)id);
                return View("../AdminViews/Physician/PhysicianAddEdit",v);

            }
            return View("../AdminViews/Physician/PhysicianAddEdit");
        }
        #endregion

        #region PhysicianAddEdit
        [HttpPost]
        public async Task<IActionResult> PhysicianAddEdit(Physicians physicians)
        {
            //TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
           // bool b = physicians.Isagreementdoc[0];

          if(ModelState.IsValid)
            {
              await  _physicianRepository.PhysicianAddEdit(physicians, CV.ID());

            }
            else
            {
                return View("../AdminViews/Physician/PhysicianAddEdit",physicians);
            }
           
            return View("../AdminViews/Physician/PhysicianAddEdit");
        }
        #endregion

        #region SendMessage
        public async Task<IActionResult> SendMessage(string? id, string? email, int? way, string? msg)
        {
            bool s;
            if (way == 1)
            {
                 s = await _emailconfig.SendMail(email, "Check massage" ,"Heyy "+msg);

            }
            else if (way == 2)
            {

                s = await _emailconfig.SendMail(email, "Check massage" ,"Heyy "+msg);
            }
            else
            {
              s = await  _emailconfig.SendMail(email, "Check massage", "Heyy " + msg);

            }
            if (s)
            {
                TempData["Status"] = "Massage Send Successfully..!";
            }

            return RedirectToAction("PhysicianAll");
        }
        #endregion

        #region Update_Physician_Profile


        public async Task<IActionResult> SavePhysicianInfo(Physicians physicians)
        {


            bool data = await _physicianRepository.SavePhysicianInfo(physicians);
            if (data)
            {
                TempData["Status"] = "Administration Information Changed...";
            }
            else
            {
                TempData["Status"] = "Imformation not Changed properly...";
            }

            return RedirectToAction("PhysicianProfile", new { id = physicians.Physicianid });
        }
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

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ResetPassAdmin(string password, int Physicianid)
        {



            bool data = await _physicianRepository.ChangePasswordAsync(password, Physicianid);
            if (data)
            {
                TempData["Status"] = "Password changed Successfully...";
            }
            else
            {
                TempData["Status"] = "Password not Changed...";
            }

            return RedirectToAction("PhysicianProfile", new { id = Physicianid });
        }
        #endregion

    }
}
