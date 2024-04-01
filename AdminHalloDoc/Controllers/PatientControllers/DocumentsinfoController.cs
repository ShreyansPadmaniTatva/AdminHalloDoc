using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    [AdminAuth("Patient")]
    public class DocumentsinfoController : Controller
    {
        #region Configuration
        private IPatientDashboardRepository _patientDashrepo;

        public DocumentsinfoController(IPatientDashboardRepository patientDashrepo)
        {

            this._patientDashrepo = patientDashrepo;

        }
        #endregion

        #region Index
        public IActionResult Index(int id)
        {
            var result = _patientDashrepo.Documentsinfo(id);

            
            return View("../PatientViews/Documentsinfo/Index", result);
        }
        #endregion

        #region UploadDoc_Files
        public IActionResult UploadDoc(int Requestid,IFormFile file)
        {
            var result = _patientDashrepo.UploadDoc(Requestid,file);

            return RedirectToAction("Index", new { id = Requestid } );
        }
        #endregion
    }
}
