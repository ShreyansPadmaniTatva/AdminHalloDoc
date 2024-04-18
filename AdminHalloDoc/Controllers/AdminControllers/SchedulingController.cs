using AdminHalloDoc.Controllers.Login;
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
        [AdminAuth("Admin,Provider")]
        [Route("/Admin/Scheduling")]
        [Route("/Physician/Scheduling")]

        public async Task<IActionResult> Index()
        {
            TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
          
            return View("../AdminViews/Schedule/MySchedule");
        }
        #endregion

        #region ShiftForMonth
        public async Task<IActionResult> GetShiftForMonth(int? month,int? regionId, int? year)
        {
            var v = await _schedulingRepository.GetShift((int)year, (int)month, regionId);
            return Json(v);
        }
        #endregion

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
            if (CV.role() == "Provider")
            {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox(Convert.ToInt32(CV.UserID()));
                return PartialView("../AdminViews/Schedule/_CreateShift-Provider");
            }
            return PartialView("../AdminViews/Schedule/_CreateShift");
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

            if(CV.role() == "Provider")
            {
                return PartialView("../AdminViews/Schedule/_EditShift-Provider", v);
            }
            return PartialView("../AdminViews/Schedule/_EditShift", v);
        }



        #region _EditShiftPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditShiftPost(Schedule v, string submittt)
        {
            if (submittt == "Return" && await _schedulingRepository.UpdateStatusShift("" + v.Shiftid, CV.ID()))
            {
                TempData["Status"] = "Update Shift Successfully..!";
            }
            else
            {

                if (await _schedulingRepository.EditShift(v, CV.ID()))
                {
                    TempData["Status"] = "Edit Shift Successfully..!";
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region _UpdateShiftPost

        public async Task<IActionResult> _UpdateShiftPost(int id)
        {
            if (await _schedulingRepository.UpdateStatusShift("" + id, CV.ID()))
            {
                TempData["Status"] = "Update Shift Successfully..!";
            }


            return RedirectToAction("Index");
        }
        #endregion

        #region _DeleteShiftPost

        public async Task<IActionResult> _DeleteShiftPost(int id)
        {
            if (await _schedulingRepository.DeleteShift("" + id, CV.ID()))
            {
                TempData["Status"] = "Delete Shift Successfully..!";
            }

            return RedirectToAction("Index");
        }
        #endregion
        #endregion

        #region RequestedShift
        public async Task<IActionResult> RequestedShift(int? regionId)
        {
            TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            List<Schedule> v = await _schedulingRepository.GetAllNotApprovedShift(regionId);
            if (regionId != null)
            {
                return Json(v);
            }
            return View("../AdminViews/Schedule/RequestedShift",v);
        }
        #endregion

        #region _ApprovedShifts

        public async Task<IActionResult> _ApprovedShifts(string shiftids)
        {
            if (await _schedulingRepository.UpdateStatusShift(shiftids, CV.ID()))
            {
                TempData["Status"] = "Approved Shifts Successfully..!";
            }


            return RedirectToAction("RequestedShift");
        }
        #endregion

        #region _DeleteShifts

        public async Task<IActionResult> _DeleteShifts(string shiftids)
        {
            if (await _schedulingRepository.DeleteShift(shiftids, CV.ID()))
            {
                TempData["Status"] = "Delete Shifts Successfully..!";
            }

            return RedirectToAction("RequestedShift");
        }
        #endregion

        #region Provider_on_call
        public async Task<IActionResult> ProviderOnCall(int? regionId)
        {
            TempData["Status"] = TempData["Status"];
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            List<Physicians> v = await _schedulingRepository.PhysicianOnCall(regionId);
            if (regionId != null)
            {
                return Json(v);
            }
            return View("../AdminViews/Schedule/ProviderOnCall",v);
        }
        #endregion
    }
}
