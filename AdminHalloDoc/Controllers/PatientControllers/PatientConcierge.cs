using AdminHalloDoc.Entities;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientConcierge : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        public ApplicationDbContext _context;

        public PatientConcierge(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public IActionResult Index()
        {
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
                return View("../PatientViews/PatientConcierge/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}   
