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
    public class PatientConcierge : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        private readonly IRequestRepository _requestRepository;
        public ApplicationDbContext _context;

        public PatientConcierge(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository, IRequestRepository requestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _requestRepository = requestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../PatientViews/PatientConcierge/Index");
        }
        #endregion

        #region Post
        public async Task<IActionResult> Post(ViewPatientConcierge viewdata)
        {
            if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientConcierge(viewdata);
            }

            else
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/PatientConcierge/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}   
