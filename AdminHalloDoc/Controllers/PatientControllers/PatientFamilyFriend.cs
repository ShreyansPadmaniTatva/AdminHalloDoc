using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientFamilyFriend : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        public ApplicationDbContext _context;

        public PatientFamilyFriend(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View("../PatientViews/PatientFamilyFriend/Index");
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post(ViewPatientFamilyFriend viewdata)
        {
             if (ModelState.IsValid)
            {
                bool v = await _patientRequestRepository.PatientFamilyFriend(viewdata);
            }
            else
            {
                return View("../PatientViews/PatientFamilyFriend/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
