using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult UploadDoc(int Requestid, IFormFile file)
        {
            var result = _patientDashrepo.UploadDoc(Requestid, file);

            return RedirectToAction("Index", new { id = Requestid });
        }
        #endregion
    }
}
