
using Microsoft.AspNetCore.Mvc;


namespace ProgrammingCode.Areas.MST_Level.Controllers
{
    public class MST_LevelController : Controller
    {
       
        #region index
        public IActionResult Index()
        {
          
            return View();
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult()
        {
           
            return PartialView("_List");
        }
        #endregion

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int LevelID)
        {
            return Content(null);
        }
        #endregion

    

        #region _AddEdit
        public IActionResult _AddEdit(int? LevelID)
        {
          
            return PartialView();
        }
        #endregion
    }
}
