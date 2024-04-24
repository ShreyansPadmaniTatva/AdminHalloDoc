using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Web.WebPages;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class InvoiceController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        private readonly ISchedulingRepository _schedulingRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceController(IPhysicianRepository physicianRepository, IMyProfileRepository myProfileRepository, IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, EmailConfiguration emailConfiguration, ISchedulingRepository schedulingRepository, IInvoiceRepository invoiceRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
            _schedulingRepository = schedulingRepository;
            _invoiceRepository = invoiceRepository;
        }

        public IActionResult Index()
        {
            return View("../AdminViews/Invoice/Index");
        }

        public IActionResult IsFinalizeSheet(int PhysicianId, DateOnly StartDate)
        {
            bool x = _invoiceRepository.isFinalizeTimesheet(PhysicianId, StartDate);
            return Json(new { x });
        }
        public IActionResult GetTimesheetDetailsData(int PhysicianId, DateOnly StartDate)
        {
            List<Timesheetdetail> x = _invoiceRepository.PostTimesheetDetails(PhysicianId, StartDate, 0, CV.ID());
            List<Entities.Models.Timesheetdetailreimbursement> h = null;
            var Timesheet = _invoiceRepository.GetTimesheetDetails(x, h, PhysicianId);
           
            if(Timesheet == null)
            {
                return Json(null);
            }
            return Json(Timesheet);
        }

        public IActionResult TimeSheetAddEdit(int PhysicianId, DateOnly StartDate,int AfterDays)
        {
           var TimesheetDetails =  _invoiceRepository.PostTimesheetDetails(PhysicianId,  StartDate,  AfterDays,CV.ID());
            List<Entities.Models.Timesheetdetailreimbursement> h = null;
            var Timesheet = _invoiceRepository.GetTimesheetDetails(TimesheetDetails,  h , PhysicianId);
            Timesheet.PhysicianId = PhysicianId;
            return View("../AdminViews/Invoice/TimesheetDetails", Timesheet);
        }
        
        public IActionResult TimeSheetDetailsEdit([FromForm] List<Timesheetdetails> timesheetdetails,int PhysicianId)
        {
           if( _invoiceRepository.PutTimesheetDetails(timesheetdetails,CV.ID()) )
            {

            }

            return RedirectToAction("TimeSheetAddEdit",new { PhysicianId = PhysicianId, StartDate  = timesheetdetails[0].Timesheetdate, AfterDays = timesheetdetails.Count()});
        }

    }
}
