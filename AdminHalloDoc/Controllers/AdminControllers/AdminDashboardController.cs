using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        public AdminDashboardController(IRequestRepository requestRepository, IViewActionRepository viewActionRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
        }
        // [Authorize(Roles = "Admin")]
        #region DashBoard_Index
        public async Task<IActionResult> Index()
        {
            TempData["Status"] = TempData["Status"];
            ViewBag.CountNewRequest = await _requestRepository.CountNewRequest();
            ViewBag.CountPandingRequest = await _requestRepository.CountPandingRequest();
            ViewBag.CountActiveRequest = await _requestRepository.CountActiveRequest();
            ViewBag.CountConcludeRequest = await _requestRepository.CountConcludeRequest();
            ViewBag.CountToCloseRequest = await _requestRepository.CountToCloseRequest();
            ViewBag.CountUnPaidRequest = await _requestRepository.CountUnPaidRequest();

            return View("../AdminViews/AdminDashboard/Index");
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync(string status)
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            if (status == null)
            {
                status = "1";
            }
            var r = await _requestRepository.GetContactAsync(status);
            return PartialView("../AdminViews/AdminDashboard/_List", r);
        }
        #endregion

        #region View_Case
        public async Task<IActionResult> Viewcase(int? id)
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            Viewcase v =  await _requestRepository.GetRequestDetails(id);
            return View("../AdminViews/ViewAction/Viewcase",v);
        }
        #endregion

        #region View_Upload
        public async Task<IActionResult> ViewUpload(int? id)
        {
           List<ViewPatientDashboard> v = await _viewActionRepository.GetDocumentByRequest(id);
            return View("../AdminViews/ViewAction/ViewUpload", v);
        }
        #endregion

        #region UploadDoc_Files
        public async Task<IActionResult> ProviderbyRegion(int? Regionid)
        {
            var v = await _viewActionRepository.ProviderbyRegion(Regionid);
            return Json(v);
        }
        #endregion
    }
}
