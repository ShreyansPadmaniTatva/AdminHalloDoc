using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class AdminProfileController : Controller
    {
        public IActionResult Index()
        {
           return  View("../AdminViews/Profile/Index");
        }
    }
}
