using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    
    public class AdminProfileController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        public AdminProfileController(IMyProfileRepository myProfileRepository,IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, IPhysicianRepository physicianRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
        }
        #endregion

        #region Profile_User
        [AdminAuth("Admin,Provider")]
        [Route("Physician/Profile")]
        [Route("Admin/Profile")]
        public async Task<IActionResult> Index(int? id)
        {
            ViewAdminProfile p = await _myProfileRepository.GetProfileDetails( (id !=null ? (int) id: Convert.ToInt32(CV.UserID())) );
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.userrolecombobox = await _requestRepository.UserRoleComboBox();
            if (CV.role() == "Provider")
            {
                ViewData["PhysicianAccount"] = "";
                Physicians v = await _physicianRepository.GetPhysicianById(Convert.ToInt32(CV.UserID()));
                return View("../AdminViews/Physician/PhysicianAddEdit", v);
            }
            return  View("../AdminViews/Profile/Index",p);
        }
        #endregion

        #region Update_Profile


        public async Task<IActionResult> SaveAdministrationinfo(ViewAdminProfile vm)
        {


            bool data =await _myProfileRepository.EditAdminProfileAsync(vm);
            if (data)
            {
                TempData["Status"] = "Administration Information Changed...";
            }
            else
            {
                TempData["Status"] = "Imformation not Changed properly...";
            }

            return RedirectToAction("Index");
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

        public async Task<IActionResult> ResetPassAdmin(string password)
        {



            bool data = await _myProfileRepository.ChangePasswordAsync(password, Convert.ToInt32(CV.UserID()));
            if (data)
            {
                TempData["Status"] = "Password changed Successfully...";
            }
            else
            {
                TempData["Status"] = "Password not Changed...";
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
