using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.InkML;
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
        public IActionResult Index(int RequestID)
        {
            var request = _context.Requests.Find(RequestID);
            TempData["RequestID"] = RequestID;
            TempData["PatientName"] = request.Firstname + " " + request.Lastname;
            return View();
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
            if (_viewActionRepository.SendAgreement(v))
            {
                TempData["Status"] = "Mail Send  Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

       


        public IActionResult accept(int RequestID)
        {
            _viewActionRepository.SendAgreement_accept(RequestID);
            return RedirectToAction("Index", "AdminDashboard");
        }

        public IActionResult Reject(int RequestID, string Notes)
        {
            _viewActionRepository.SendAgreement_Reject(RequestID, Notes);
            return RedirectToAction("Index", "AdminDashboard");
        }
    }
}
