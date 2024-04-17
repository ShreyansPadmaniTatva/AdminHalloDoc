using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
   // [AdminAuth("Admin,Provider")]
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

        #region DashBoard_Index
        [AdminAuth("Admin,Provider")]
        [Route("Physician/DashBoard")]
        [Route("Admin/DashBoard")]
        public async Task<IActionResult> Index()
        {
            TempData["Status"] = TempData["Status"];
            PaginatedViewModel sm = _requestRepository.Indexdata(-1);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            if (CV.role() == "Provider")
            {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox(Convert.ToInt32(CV.UserID()));
                 sm = _requestRepository.Indexdata(Convert.ToInt32(CV.UserID()));

            }
           
            return View("../AdminViews/AdminDashboard/Index",sm);
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultAsync(PaginatedViewModel data, string status)
        {

            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            data.status = status;
            if (status == null)
            {
                status = "1";
            }
            if(CV.role() == "Provider")
            {
                var pr = await _requestRepository.GetContactAsync(status, data,Convert.ToInt32(CV.UserID()));
                return PartialView("../AdminViews/AdminDashboard/_PList", pr);
            }
            var r = await _requestRepository.GetContactAsync(status,data);
            return PartialView("../AdminViews/AdminDashboard/_List", r);
        }
        #endregion

        #region View_Case
        [Route("Physician/Viewcase/{id}")]
        [Route("Admin/Viewcase/{id}")]
        public async Task<IActionResult> Viewcase(string id)
        {
            TempData["Status"] = TempData["Status"];
            if (id.Decode() == null)
            {
                return Redirect("/PageNoteFound");
            }
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            Viewcase v =  await _requestRepository.GetRequestDetails(id.Decode());
            if (v == null)
            {
                return Redirect("/PageNoteFound");
            }
            return View("../AdminViews/ViewAction/Viewcase",v);
        }
        #endregion

        #region View_Notes
        public async Task<IActionResult> ViewNotes(string id)
        {
            TempData["Status"] = TempData["Status"];
            if (id.Decode() == null)
            {
                return Redirect("/PageNoteFound");
            }
            var n = await _viewNotesRepository.GetNotesByRequest((int)id.Decode());
            if ( n == null)
            {
                return Redirect("/PageNoteFound");
            }
            return View("../AdminViews/ViewAction/ViewNotes",n);
        }
        #endregion

        #region ChangeNotes

        [HttpPost]
        public IActionResult ChangeNotes(int? RequestID, string? adminnotes, string? physiciannotes)
        {
            if (adminnotes != null || physiciannotes != null)
            {
                bool result = _viewNotesRepository.PutNotes(adminnotes, physiciannotes, RequestID,CV.ID());

                if (result)
                {
                    TempData["Status"] = "Change Successfully..!";
                    return RedirectToAction("ViewNotes", new { id = RequestID.Encode() });
                }
                else
                {
                    TempData["Status"] = "Not Change In Note";
                    return RedirectToAction("ViewNotes", new { id = RequestID.Encode() });
                }
            }
            else
            {
                TempData["Status"] = "Please Select one of the note!!";
                return RedirectToAction("ViewNotes", new { id = RequestID.Encode() });
            }

        }
        #endregion

        #region View_Upload
        [Route("Physician/ViewUpload/{id}")]
        [Route("Admin/ViewUpload/{id}")]
        public async Task<IActionResult> ViewUpload(string id)
        {
            if (id.Decode() == null)
            {
                return Redirect("PageNoteFound");
            }
            ViewDocuments v = await _viewActionRepository.GetDocumentByRequest(id.Decode());
            if (v == null)
            {
                return Redirect("/PageNoteFound");
            }
            return View("../AdminViews/ViewAction/ViewUpload", v);
        }
        #endregion

        #region ViewOrder
        public async Task<IActionResult> ViewOrder(string id)
        {
            if (id.Decode() == null)
            {
                return Redirect("PageNoteFound");
            }
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            ViewOrder v = new ViewOrder();
            v.RequestId = (int)id.Decode();
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
