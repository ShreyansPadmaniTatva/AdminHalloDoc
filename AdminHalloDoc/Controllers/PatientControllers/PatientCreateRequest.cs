using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Globalization;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientCreateRequest : Controller
    {
        #region Configuration
        private IPatientRequestRepository _patientRequestRepository;
        private readonly IRequestRepository _requestRepository;
        public ApplicationDbContext _context;

        public PatientCreateRequest(ApplicationDbContext context, IPatientRequestRepository patientRequestRepository, IRequestRepository requestRepository)
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
            return View("../PatientViews/PatientCreateRequest/Index");
        }
        #endregion

        #region CheckEmailAsync
        [HttpPost]
        public async Task<IActionResult> CheckEmailAsync(string email)
        {
            string message;
            string Data = "0";
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
            //await _context.SaveChangesAsync();
            if (aspnetuser == null)
            {
                message = "False";

                Data = "0";
            }
            else
            {
                message = "success";
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Aspnetuserid == aspnetuser.Id.ToString());
                HttpContext.Session.SetString("UserName", aspnetuser.Username.ToString());
                HttpContext.Session.SetString("UserID", user.Userid.ToString());
                Data = user.Userid.ToString();
            }
            return Json(new
            {
                Data,
                Message = message,
            });
        }
        #endregion

        #region Post
       
        public async Task<IActionResult> Post(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            if (_patientRequestRepository.IsEmailBlock(viewpatientcreaterequest.Email))
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                ModelState.AddModelError("Email", "This Email Id Is Block Try Another One");
                return View("../PatientViews/PatientCreateRequest/Index", viewpatientcreaterequest);
            }
            if (ModelState.IsValid)
            {

               bool v = await _patientRequestRepository.PatientCreateRequest(viewpatientcreaterequest);
            }
            else
            {
                ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
                return View("../PatientViews/PatientCreateRequest/Index", viewpatientcreaterequest);
            }
                return RedirectToAction("Index", "Dashboard");
            
        }
        #endregion
    }
}
