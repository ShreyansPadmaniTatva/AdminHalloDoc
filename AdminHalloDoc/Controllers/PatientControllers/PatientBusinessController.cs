using AdminHalloDoc.Entities;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientBusinessController : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        private readonly IRequestRepository _requestRepository;
        public ApplicationDbContext _context;

        public PatientBusinessController(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository, IRequestRepository requestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _context = context;
            _requestRepository = requestRepository;
        }
        #endregion

        #region Index
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../PatientViews/PatientBusiness/Index");
        }
        #endregion

        #region Post
        public async Task<IActionResult> Post(ViewPatientBusiness viewdata)
        {
            if (_patientRequestRepository.IsEmailBlock(viewdata.Email))
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                ModelState.AddModelError("Email", "This Email Id Is Block Try Another One");
                return View("../PatientViews/PatientBusiness/Index", viewdata);
            }
            if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientBusiness(viewdata);
            }
            else
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/PatientBusiness/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
