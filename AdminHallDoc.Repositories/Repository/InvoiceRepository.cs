using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public InvoiceRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailConfig = emailConfig;
            this.httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Timesheet_Finalize_Or_Not

        public bool isFinalizeTimesheet(int PhysicianId,DateOnly StartDate)
        {
            var data = _context.Timesheets.Where(e => e.Physicianid == PhysicianId && e.Startdate == StartDate).FirstOrDefault();
            if(data == null)
            {
                return false;
            }else if(data.Isfinalize ==  false)
            {

                return false;
            }
            return true;
        }
        #endregion

        #region Timesheet_Add

        public List<Timesheetdetail> PostTimesheetDetails(int PhysicianId, DateOnly StartDate,int AfterDays, string AdminId)
        {
                var Timesheet = new Timesheet();
            var data = _context.Timesheets.Where(e => e.Physicianid == PhysicianId && e.Startdate == StartDate).FirstOrDefault();
            if (data == null && AfterDays != 0)
            {
                DateOnly EndDate = StartDate.AddDays(AfterDays-1);
                Timesheet.Startdate = StartDate;
                Timesheet.Physicianid = PhysicianId;
                Timesheet.Isfinalize = false;
                Timesheet.Enddate = EndDate;
                Timesheet.Createddate = DateTime.Now;
                Timesheet.Createdby = AdminId;
                _context.Timesheets.Add(Timesheet);
                _context.SaveChanges();

                for (DateOnly i = StartDate; i <= EndDate;i=i.AddDays(1))
                {
                    var Timesheetdetail = new Timesheetdetail();
                    Timesheetdetail.Timesheetid = Timesheet.Timesheetid;
                    Timesheetdetail.Timesheetdate = i;
                    _context.Timesheetdetails.Add(Timesheetdetail);
                    _context.SaveChanges();
                }

                 return _context.Timesheetdetails.Where(e => e.Timesheetid == Timesheet.Timesheetid).ToList();
            }
            else if (data == null && AfterDays == 0)
            {
                return null;

            }
            else
            {
                return _context.Timesheetdetails.Where(e => e.Timesheetid == data.Timesheetid).ToList();
            }

        }
        #endregion

        #region TimesheetDetails_Get

        public ViewTimeSheet GetTimesheetDetails(List<Timesheetdetail> td, List<Timesheetdetailreimbursement> tr,int PhysicianId)
        {
            try
            {
                var TimeSheet = new ViewTimeSheet();

                TimeSheet.Timesheetdetails = td.Select(e => new Timesheetdetails
                {
                    Isweekend = e.Isweekend == null || e.Isweekend == false ? false : true,
                    Modifiedby = e.Modifiedby,
                    Modifieddate = e.Modifieddate,
                    Numberofhousecall = e.Numberofhousecall,
                    Numberofphonecall = e.Numberofphonecall,
                    OnCallhours = FindOnCallProvider(PhysicianId, e.Timesheetdate),
                    Timesheetdate = e.Timesheetdate,
                    Timesheetdetailid = e.Timesheetdetailid,
                    Totalhours = e.Totalhours,
                    Timesheetid = e.Timesheetid
                }).ToList();

                return TimeSheet;
            }
            catch(Exception e)
            {
                return null;
            }
          

        }
        #endregion
        public int FindOnCallProvider( int PhysicianId,DateOnly Timesheetdate)
        {
            int i = 0;
            var s = _context.Shifts.Where(r => r.Physicianid == PhysicianId).ToList();
            foreach (var item in s)
            {
                i += _context.Shiftdetails.Where(r => r.Shiftid == item.Shiftid && DateOnly.FromDateTime(r.Shiftdate) == Timesheetdate).Count();
            }
            return i;
        }

        #region Timesheet_Edit

        public bool PutTimesheetDetails(List<Timesheetdetails> tds, string AdminId)
        {
            try
            {
                foreach (var item in tds)
                {
                    var td = _context.Timesheetdetails.Where(r => r.Timesheetdetailid == item.Timesheetdetailid).FirstOrDefault();
                    td.Totalhours = item.Totalhours;
                    td.Numberofhousecall = item.Numberofhousecall;
                    td.Numberofphonecall = item.Numberofphonecall;
                    td.Isweekend = item.Isweekend;
                    td.Modifiedby = AdminId;
                    td.Modifieddate = DateTime.Now;
                    _context.Timesheetdetails.Update(td);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            } 
           
            
        }
        #endregion

    }
}
