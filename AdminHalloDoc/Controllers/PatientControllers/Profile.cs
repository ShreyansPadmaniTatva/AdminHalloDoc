using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    [AdminAuth("Patient")]
    public class Profile : Controller
    {
        #region Configuration
        private IPatientDashboardRepository _patientDashrepo;

        public Profile(IPatientDashboardRepository patientDashrepo)
        {

            this._patientDashrepo = patientDashrepo;

        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            var UsersProfile = _patientDashrepo.Profile(Convert.ToInt32(CV.UserID()));

            return View("../PatientViews/Profile/Index", UsersProfile);
        }
        #endregion

        #region Put
        public async Task<IActionResult> Put(ViewUserProfile userprofile)
        {
            bool v = await _patientDashrepo.ProfileEdit(userprofile);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
