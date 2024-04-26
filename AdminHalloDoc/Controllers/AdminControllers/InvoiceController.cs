using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Web.WebPages;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Bibliography;
using AdminHalloDoc.Repositories.Admin.Repository;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class InvoiceController : Controller
    {
        #region Constoter
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IRequestRepository _requestRepository;
        public InvoiceController(IInvoiceRepository invoiceRepository, IRequestRepository requestRepository)
        {
            _invoiceRepository = invoiceRepository;
            _requestRepository = requestRepository;
        }
        #endregion

        #region Index_View
        public async  Task<IActionResult> Index()
        {
            return View("../AdminViews/Invoice/Index");
        }

        #endregion

        #region Admin_Index_View
        public async Task<IActionResult> AdminIndex()
        {
            ViewBag.ProviderComboBox = await _requestRepository.ProviderComboBox();
            return View("../AdminViews/Invoice/AdminIndex");
        }

        #endregion

        #region IsFinalizeSheet

        public IActionResult IsFinalizeSheet(int PhysicianId, DateOnly StartDate)
        {
            bool x = _invoiceRepository.isFinalizeTimesheet(PhysicianId, StartDate);
            return Json(new { x });
        }
        #endregion

        #region IsApprovedSheet

        public IActionResult IsApprovedSheet(int PhysicianId, DateOnly StartDate)
        {
            bool x = _invoiceRepository.isApprovedTimesheet(PhysicianId, StartDate);
            return Json(new { x });
        }
        #endregion

        #region SetToApprove

        public IActionResult SetToApprove(int timesheetid)
        {
            if (_invoiceRepository.SetToApprove(timesheetid, CV.ID()))
            {
                TempData["Status"] = "Sheet Is Finalize Successfully..!";
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region SetToFinalize

        public IActionResult SetToFinalize(int timesheetid)
        {
            if (_invoiceRepository.SetToFinalize(timesheetid, CV.ID()))
            {
                TempData["Status"] = "Sheet Is Finalize Successfully..!";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region GetTimesheetDetails_Jsoon
        public async Task<IActionResult> GetTimesheetDetailsDataAsync(int PhysicianId, DateOnly StartDate)
        {
            List<Timesheetdetail> x = _invoiceRepository.PostTimesheetDetails(PhysicianId, StartDate, 0, CV.ID());
            List<Entities.Models.Timesheetdetailreimbursement> h =  await _invoiceRepository.GetTimesheetBills(x);
            var Timesheet = _invoiceRepository.GetTimesheetDetails(x, h, PhysicianId);
           
            if(Timesheet == null)
            {
                return Json(null);
            }
            return Json(Timesheet);
        }
        #endregion

        #region TimeSheetDetailsAddEdit_PageData

        public async Task<IActionResult> TimeSheetAddEdit(int PhysicianId, DateOnly StartDate)
        {
            if (CV.role() == "Provider" && _invoiceRepository.isFinalizeTimesheet(PhysicianId, StartDate))
            {
                TempData["Status"] = "Sheet Is Already Finalize";
                RedirectToAction("Index");
            }
            int AfterDays = StartDate.Day == 1 ? 14 : DateTime.DaysInMonth(StartDate.Year, StartDate.Month)-14; ; 
           var TimesheetDetails =  _invoiceRepository.PostTimesheetDetails(PhysicianId,  StartDate,  AfterDays,CV.ID());
            List<Entities.Models.Timesheetdetailreimbursement> h = await _invoiceRepository.GetTimesheetBills(TimesheetDetails);
            var Timesheet = _invoiceRepository.GetTimesheetDetails(TimesheetDetails,  h , PhysicianId);
            Timesheet.PhysicianId = PhysicianId;
            return View("../AdminViews/Invoice/TimesheetDetails", Timesheet);
        }
        #endregion

        #region Edit_TimeSheetDetails

        public IActionResult TimeSheetDetailsEdit([FromForm] List<Timesheetdetails> timesheetdetails,int PhysicianId)
        {
           if( _invoiceRepository.PutTimesheetDetails(timesheetdetails,CV.ID()) )
            {
                TempData["Status"] = "Edit  TimeSheet  Successfully..!";
            }

            return RedirectToAction("TimeSheetAddEdit",new { PhysicianId = PhysicianId, StartDate  = timesheetdetails[0].Timesheetdate});
        }
        #endregion

        #region TimeSheetBill_AddEdit
        public IActionResult TimeSheetBillAddEdit(int? Trid,DateOnly Timesheetdate, IFormFile file, int Timesheetdetailid, int Amount, string Item, int PhysicianId, DateOnly StartDate)
        {
            Timesheetdetailreimbursements timesheetdetailreimbursement = new Timesheetdetailreimbursements();
            timesheetdetailreimbursement.Timesheetdetailid = Timesheetdetailid;
            timesheetdetailreimbursement.Timesheetdetailreimbursementid = Trid;
            timesheetdetailreimbursement.Amount = Amount;
            timesheetdetailreimbursement.Billfile = file;
            timesheetdetailreimbursement.Itemname = Item;
            if (_invoiceRepository.TimeSheetBillAddEdit(timesheetdetailreimbursement, CV.ID()))
            {
                TempData["Status"] = "Bill Change Successfully..!";
            }
            return RedirectToAction("TimeSheetAddEdit", new { PhysicianId = PhysicianId, StartDate = StartDate });
        }
        #endregion

        #region TimeSheetBill_Delete
        public IActionResult TimeSheetBillRemove(int? Trid, int PhysicianId, DateOnly StartDate)
        {
            Timesheetdetailreimbursements timesheetdetailreimbursement = new Timesheetdetailreimbursements();
            timesheetdetailreimbursement.Timesheetdetailreimbursementid = Trid;
            if (_invoiceRepository.TimeSheetBillRemove(timesheetdetailreimbursement, CV.ID()))
            {
                TempData["Status"] = "Bill Change Successfully..!";
            }
            return RedirectToAction("TimeSheetAddEdit", new { PhysicianId = PhysicianId, StartDate = StartDate });
        }
        #endregion
    }
}
