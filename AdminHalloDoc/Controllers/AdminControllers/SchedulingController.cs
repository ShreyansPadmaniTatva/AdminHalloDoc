using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class SchedulingController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        private readonly ISchedulingRepository _schedulingRepository;
        public SchedulingController(IPhysicianRepository physicianRepository, IMyProfileRepository myProfileRepository, IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, EmailConfiguration emailConfiguration, ISchedulingRepository schedulingRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
            _schedulingRepository = schedulingRepository;
        }
        #endregion

        #region Physician_Schedule
        public async Task<IActionResult> Index()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
          
            return View("../AdminViews/Physician/MySchedule");
        }
        #endregion

        public async Task<IActionResult> GetShiftForMonth(int? month)
        {
            var v = await _schedulingRepository.GetShift((int)month);
            return Json(v);
        }


        #region PhysicianAll
        public async Task<IActionResult> PhysicianAll(int? region)
        {
            TempData["Status"] = TempData["Status"];
           
            var v = await _schedulingRepository.PhysicianAll();

            if (region != null)
            {
                v = await _schedulingRepository.PhysicianByRegion(region);

            }

             return Json(v);
        }
        #endregion

        #region _CreateShift
        public async Task<IActionResult> _CreateShift()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/Physician/_CreateShift");
        }


        #region _CreateShiftPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateShiftPost(Schedule v)
        {
            if (await _schedulingRepository.CreateShift(v,CV.ID()))
            {
                TempData["Status"] = "Create Shift Successfully..!";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        #region _EditShift
        public async Task<IActionResult> _EditShift(int id)
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
           Schedule v = await _schedulingRepository.GetShiftByShiftdetailId(id);
            ViewBag.ProviderComboBox = await _viewActionRepository.ProviderbyRegion(v.Regionid);
            return PartialView("../AdminViews/Physician/_EditShift");
        }


        #region _CreateShiftPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditShiftPost(Schedule v)
        {
            if (await _schedulingRepository.CreateShift(v, CV.ID()))
            {
                TempData["Status"] = "Create Shift Successfully..!";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #endregion
    }
}
