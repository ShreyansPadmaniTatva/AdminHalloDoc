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
    public class PatientBusinessController : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        public ApplicationDbContext _context;

        public PatientBusinessController(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View("../PatientViews/PatientBusiness/Index");
        }
        #endregion

        #region Post
        public async Task<IActionResult> Post(ViewPatientBusiness viewdata)
        {
            if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientBusiness(viewdata);
            }
            else
            {
                return View("../PatientViews/PatientBusiness/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
