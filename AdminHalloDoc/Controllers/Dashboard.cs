using AdminHalloDoc.Repositories.Repository;
using AdminHalloDoc.Repositories.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers
{
    public class Dashboard : Controller
    {
        private readonly IRequestRepository _requestRepository;
        public Dashboard(IRequestRepository requestRepository)
        {

            _requestRepository = requestRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CountNewRequest = await _requestRepository.CountNewRequest();
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync( )
        {
            var r = await _requestRepository.GetContactAsync();
            return PartialView("_List",r );
        }
        #endregion
    }
}
