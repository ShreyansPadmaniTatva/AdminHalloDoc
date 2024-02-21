using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class SubmitRequest : Controller
    {

        #region Index
        public IActionResult Index()
        {
            return View("../PatientViews/SubmitRequest/Index");
        }
        #endregion
    }
}
