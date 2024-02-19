using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult( )
        {
            return PartialView("_List" );
        }
        #endregion
    }
}
