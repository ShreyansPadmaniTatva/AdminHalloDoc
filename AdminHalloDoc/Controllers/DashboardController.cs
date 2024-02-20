using AdminHalloDoc.Repositories.Repository;
using AdminHalloDoc.Repositories.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        public DashboardController(IRequestRepository requestRepository)
        {

            _requestRepository = requestRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CountNewRequest = await _requestRepository.CountNewRequest();
            ViewBag.CountPandingRequest = await _requestRepository.CountPandingRequest();
            ViewBag.CountActiveRequest = await _requestRepository.CountActiveRequest();
            ViewBag.CountConcludeRequest = await _requestRepository.CountConcludeRequest();
            ViewBag.CountToCloseRequest = await _requestRepository.CountToCloseRequest();
            ViewBag.CountUnPaidRequest = await _requestRepository.CountUnPaidRequest();

            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync(string status)
        {
            if (status == null)
            {
                status = "1";
            }
            var r = await _requestRepository.GetContactAsync(status);
            return PartialView("_List",r );
        }
        #endregion
    }
}
