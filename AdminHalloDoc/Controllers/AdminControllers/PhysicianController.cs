using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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

    }
}
