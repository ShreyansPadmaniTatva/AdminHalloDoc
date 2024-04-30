using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace AdminHalloDoc.Controllers
{
    public class SendAgreementController : Controller
    {
        #region Constructor
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly ApplicationDbContext _context;

        public SendAgreementController(ApplicationDbContext context, IRequestRepository requestRepository, IViewActionRepository viewActionRepository)
        {
            _context = context;
            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
        }
        #endregion

        public IActionResult Index(string RequestID)
        {
            var request = _context.Requests.Find(RequestID.Decode());
            if (request == null)
            {
                return Redirect("PageNotFound");
            }
            TempData["RequestID"] = RequestID.Decode();
            TempData["PatientName"] = request.Firstname + " " + request.Lastname;
            return View();
        }

        [Route("PageNoteFound")]
        [Route("ServerError")]
        [Route("EmptyResponseError")]
        public ActionResult Error404()
        {
            return View("404");
        }

        #region _SendAgreement
        public async Task<IActionResult> _SendAgreement(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            return PartialView("../AdminViews/ViewAction/_modelS/_sendagreement", v);
        }


        #region _SendAgreementPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SendAgreementPost(ViewActions v)
        {
            if(CV.role() == "Admin")
            {
                v.AdminId = Convert.ToInt32(CV.UserID());
            }
            else
            {
                v.ProviderId = Convert.ToInt32(CV.UserID());
            }

            if (await _viewActionRepository.SendAgreement(v))
            {
                TempData["Status"] = "Mail Send  Successfully..!";
            }

            if (CV.role() == "Provider")
            {
                
                return Redirect("~/Physician/DashBoard");
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

       


        public IActionResult accept(int RequestID)
        {
            _viewActionRepository.SendAgreement_accept(RequestID);
            return Redirect("/Home");
        }

        public IActionResult Reject(int RequestID, string Notes)
        {
            _viewActionRepository.SendAgreement_Reject(RequestID, Notes);
            return Redirect("/Home");
        }
    }
}
