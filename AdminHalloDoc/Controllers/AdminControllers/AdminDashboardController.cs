using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    [AdminAuth("Admin")]
    public class AdminDashboardController : Controller
    {
        #region Constructor
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        public AdminDashboardController(IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
        }
        #endregion

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
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            return View("../AdminViews/AdminDashboard/Index");
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync(PaginatedViewModel data, string status)
        {

            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            if (status == null)
            {
                status = "1";
            }
            var r = await _requestRepository.GetContactAsync(status,data);
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

        #region View_Notes
        public async Task<IActionResult> ViewNotes(int id)
        {
            TempData["Status"] = TempData["Status"];
            var n = await _viewNotesRepository.GetNotesByRequest(id);
            return View("../AdminViews/ViewAction/ViewNotes",n);
        }
        #endregion

        #region ChangeNotes

        [HttpPost]
        public IActionResult ChangeNotes(int? RequestID, string? adminnotes, string? physiciannotes)
        {
            if (adminnotes != null || physiciannotes != null)
            {
                bool result = _viewNotesRepository.PutNotes(adminnotes, physiciannotes, RequestID);

                if (result)
                {
                    TempData["Status"] = "Change Successfully..!";
                    return RedirectToAction("ViewNotes", new { id = RequestID });
                }
                else
                {
                    TempData["Status"] = "Not Change In Note";
                    return RedirectToAction("ViewNotes", new { id = RequestID });
                }
            }
            else
            {
                TempData["Status"] = "Please Select one of the note!!";
                return RedirectToAction("ViewNotes", new { id = RequestID });
            }

        }
        #endregion

        #region View_Upload
        public async Task<IActionResult> ViewUpload(int? id)
        {
           ViewDocuments v = await _viewActionRepository.GetDocumentByRequest(id);
            return View("../AdminViews/ViewAction/ViewUpload", v);
        }
        #endregion

        #region ViewOrder
        public async Task<IActionResult> ViewOrder(int? id)
        {
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            ViewOrder v = new ViewOrder();
            v.RequestId = (int)id;
            return View("../AdminViews/ViewAction/ViewOrder",v);
        }
        #endregion

        #region ProviderbyRegion
        public async Task<IActionResult> ProviderbyRegion(int? Regionid)
        {
            var v = await _viewActionRepository.ProviderbyRegion(Regionid);
            return Json(v);
        }
        #endregion
    }
}
