using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Configuration;
using System.Data;

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
