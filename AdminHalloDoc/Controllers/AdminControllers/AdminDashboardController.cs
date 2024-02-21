using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        public AdminDashboardController(IRequestRepository requestRepository)
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


            return View("../AdminViews/AdminDashboard/Index");
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync(string status)
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            if (status == null)
            {
                status = "1";
            }
            var r = await _requestRepository.GetContactAsync(status);
            return PartialView("../AdminViews/AdminDashboard/_List", r);
        }
        #endregion
    }
}
