using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class RequestByPatient : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        private readonly IRequestRepository _requestRepository;
        public ApplicationDbContext _context;

        public RequestByPatient(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository, IRequestRepository requestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            this._requestRepository = requestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region SubmitForSomeoneElse
        public async Task<IActionResult> SubmitForSomeoneElseAsync()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../PatientViews/RequestByPatient/SubmitForSomeoneElse");
        }
        public async Task<IActionResult> SubmitForMeAsync()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            var ViewPatientCreateRequest = _context.Users
                               .Where(r => r.Userid == Convert.ToInt32(CV.UserID()))
                               .Select(r => new ViewPatientCreateRequest
                               {
                                   FirstName = r.Firstname,
                                   LastName = r.Lastname,
                                   Email = r.Email,
                                   PhoneNumber = r.Mobile,
                                   BirthDate = new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()), (int)r.Intdate),
                               })
                               .FirstOrDefault();
            return View("../PatientViews/RequestByPatient/SubmitForMe", ViewPatientCreateRequest);
        }
        #endregion

        #region PostMeAsync
        public async Task<IActionResult> PostMe(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientForMe(viewpatientcreaterequest, Convert.ToInt32(CV.UserID()));
            }
            else
            {
                var ViewPatientCreateRequest = _context.Users
                               .Where(r => r.Userid == Convert.ToInt32(CV.UserID()))
                               .Select(r => new ViewPatientCreateRequest
                               {
                                   FirstName = r.Firstname,
                                   LastName = r.Lastname,
                                   Email = r.Email,
                                   PhoneNumber = r.Mobile,
                                   BirthDate = new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()), (int)r.Intdate),
                               })
                               .FirstOrDefault();
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/RequestByPatient/SubmitForMe", ViewPatientCreateRequest);
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

        #region PostSomeoneElseAsync
        public async Task<IActionResult> PostSomeoneElse(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientForSomeoneElse(viewpatientcreaterequest);
            }
            else
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/RequestByPatient/SubmitForSomeoneElse", viewpatientcreaterequest);
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion
    }
}
