using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ReportsController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        private readonly IRecordsRepository _recordsRepository;
        public ReportsController(IRecordsRepository recordsRepository,IPhysicianRepository physicianRepository, IMyProfileRepository myProfileRepository, IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, EmailConfiguration emailConfiguration)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
            _recordsRepository = recordsRepository;
        }
        #endregion

        [AdminAuth("Admin")]
        #region SerchRecords
        //Serch Records
        public async Task<IActionResult> Index()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../AdminViews/Records/SearchRecords/Index");
        }


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResult(RecordsModel rm)
        {
            RecordsModel r = await _recordsRepository.GetRequestsbyfilterForRecords(rm);
            return PartialView("../AdminViews/Records/SearchRecords/_List",r);
        }
        #endregion
        #endregion

        #region PatientHistoryRecords
        public async Task<IActionResult> PatientHistory()
        {
            return View("../AdminViews/Records/PatientHistory/Index");
        }


        #region _SearchResultPatientRecords
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultPatientRecords(RecordsModel rm)
        {
            RecordsModel r = await _recordsRepository.Patienthistorybyfilter(rm);
            return PartialView("../AdminViews/Records/PatientHistory/_List", r);
        }
        #endregion

        #endregion

        #region PatientRecords
        public async Task<IActionResult> PatientRecords(PaginatedViewModel data,int UserId)
        {
            var r = await _recordsRepository.PatientRecord(UserId, data);
            return View("../AdminViews/Records/PatientHistory/PatientRecord", r);
        }
        #endregion

        #region EmailLogs
        //Serch Records
        public async Task<IActionResult> EmailLog()
        {
            return View("../AdminViews/Records/EmailLog/Index");
        }


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultEmailLog(RecordsModel rm)
        {
            RecordsModel r = await _recordsRepository.EmailLogs(rm);
            return PartialView("../AdminViews/Records/EmailLog/_List", r);
        }
        #endregion

        #endregion

        #region SMSLogs
        //Serch Records
        public async Task<IActionResult> SMSLog()
        {
            return View("../AdminViews/Records/SMSLog/Index");
        }


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultSMSLog(RecordsModel rm)
        {
            RecordsModel r = await _recordsRepository.SMSLogs(rm);
            return PartialView("../AdminViews/Records/SMSLog/_List", r);
        }
        #endregion

        #endregion

        #region BlockHistory
        //Serch Records
        public async Task<IActionResult> BlockHistory()
        {
            TempData["Status"] = TempData["Status"];
            return View("../AdminViews/Records/BlockHistory/Index");
        }


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResultBlockHistory(RecordsModel rm)
        {
            RecordsModel r = await _recordsRepository.BlockHistory(rm);
            return PartialView("../AdminViews/Records/BlockHistory/_List", r);
        }
        #endregion

        #endregion

        #region UnBlock
        public async Task<IActionResult> UnBlock(int RequestId)
        {


            bool UnBlock = await _recordsRepository.UnBlock(RequestId, CV.ID());
            if (UnBlock)
            {
                TempData["Status"] = "UnBlock Request Successfully";

            }
            else
            {
                TempData["Status"] = "UnBlock Request Canceled";

            }

            return RedirectToAction("BlockHistory");

        }
        #endregion
    }
}
