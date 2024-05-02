using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ReportsController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IRecordsRepository _recordsRepository;
        public ReportsController(IRecordsRepository recordsRepository, IRequestRepository requestRepository)
        {

            _requestRepository = requestRepository;
            _recordsRepository = recordsRepository;
        }
        #endregion

        #region SerchRecords
        [AdminAuth("Admin")]
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
            return PartialView("../AdminViews/Records/SearchRecords/_List", r);
        }
        #endregion
        #endregion

        #region PatientHistoryRecords
        [AdminAuth("Admin")]
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
        public async Task<IActionResult> PatientRecords(PaginatedViewModel data, int UserId)
        {
            var r = await _recordsRepository.PatientRecord(UserId, data);
            return View("../AdminViews/Records/PatientHistory/PatientRecord", r);
        }
        #endregion

        #region EmailLogs
        [AdminAuth("Admin")]
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
        [AdminAuth("Admin")]
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
        [AdminAuth("Admin")]
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

        #region Delete_Record
        public async Task<IActionResult> Delete(int RequestId)
        {


            bool UnBlock = _recordsRepository.Delete(RequestId, CV.ID());
            if (UnBlock)
            {
                TempData["Status"] = " Request Deleted Successfully";

            }
            else
            {
                TempData["Status"] = "Request Not Deleted ";

            }

            return RedirectToAction("Index");

        }
        #endregion
    }
}
