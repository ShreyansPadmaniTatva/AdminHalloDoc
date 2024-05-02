using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    [AdminAuth("Patient")]
    public class DashboardController : Controller
    {
        #region Configuration
        private IPatientDashboardRepository _patientDashrepo;

        public DashboardController(IPatientDashboardRepository patientDashrepo)
        {

            this._patientDashrepo = patientDashrepo;

        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {

            //ViewPatientDashboard 
            var result = _patientDashrepo.DashboardData(Convert.ToInt32(CV.UserID()));

            return View("../PatientViews/Dashboard/Index", result);
        }
        #endregion
    }
}
