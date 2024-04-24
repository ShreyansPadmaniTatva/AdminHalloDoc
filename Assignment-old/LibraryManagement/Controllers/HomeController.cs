using LibraryManagement.Entities.ViewModel;
using LibraryManagement.Models;
using LibraryManagement.Repositories.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepositories _bookRepositories;

        public HomeController(ILogger<HomeController> logger,IBookRepositories bookRepositories)
        {
            _logger = logger;
            _bookRepositories = bookRepositories;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            ViewBag.ComboBoxBorrower = await  _bookRepositories.ComboBoxBorrower();
            return View();
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultBook(BookModel bm)
        {
            ViewBag.ComboBoxBorrower = _bookRepositories.ComboBoxBorrower();

            BookModel v = _bookRepositories.GetBooks(bm);
            return PartialView("_BookList", v);
        }
        #endregion


        #region _AddBook
        public async Task<IActionResult> _AddBook(int? BookId )
        {
            if (BookId != null)
            {
                var v = _bookRepositories.GetBookById((int)BookId);
                return PartialView(v);

            }
            return PartialView();
        }


        #region _BookPostPut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _BookPost(ViewBook vb)
        { 
            if(vb.Bookid == null || vb.Bookid == 0)
            {
                await _bookRepositories.AddBook(vb);
                return RedirectToAction("Index");
            }
            else
            {
                await _bookRepositories.EditBook(vb);
                return RedirectToAction("Index");
            }
          
        }
        #endregion

        #endregion

        #region _EditBook
        public async Task<IActionResult> _EditBook()
        {

            return PartialView();
        }



        #endregion

        #region BookDelete
        [HttpPost]
        public async Task<IActionResult> BookDelete(int BookId)
        {
            await _bookRepositories.RemoveBook(BookId);
            return RedirectToAction("Index");
        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}