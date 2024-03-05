using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class PhysicianController : Controller
    {
        public IActionResult PhysicianLocation()
        {
            return View("../AdminViews/Physician/PhysicianLocation");
        }
    }
}
