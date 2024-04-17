using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
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
        private readonly IRequestRepository _requestRepository;
        public ApplicationDbContext _context;

        public PatientFamilyFriend(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository, IRequestRepository requestRepository)
        {

            this._patientRequestRepository = patientRequestRepository;
            _requestRepository = requestRepository;
            _context = context;

        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
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
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/PatientFamilyFriend/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
