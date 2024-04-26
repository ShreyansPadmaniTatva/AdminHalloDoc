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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        #region Timesheet_Approved_Or_Not

        public bool isApprovedTimesheet(int PhysicianId, DateOnly StartDate)
        {
            var data = _context.Timesheets.Where(e => e.Physicianid == PhysicianId && e.Startdate == StartDate).FirstOrDefault();
             if (data.Isapproved == false)
            {

                return false;
            }
            return true;
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

        #region Set_To_Sheet_Finalize

        public bool SetToFinalize(int timesheetid, string AdminId)
        {
            try
            {
                var data = _context.Timesheets.Where(e => e.Timesheetid == timesheetid).FirstOrDefault();
                if (data != null)
                {
                    data.Isfinalize = true;
                    _context.Timesheets.Update(data);
                    _context.SaveChanges();
                    return true;
                }

            }
            catch(Exception ex)
            {
                return false;
            }
            return false;
        }
        #endregion

        #region Set_To_Sheet_Approve

        public bool SetToApprove(int timesheetid, string AdminId)
        {
            try
            {
                var data = _context.Timesheets.Where(e => e.Timesheetid == timesheetid).FirstOrDefault();
                if (data != null)
                {
                    data.Isapproved = true;
                    _context.Timesheets.Update(data);
                    _context.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        #endregion

        #region Timesheet_Add_Return_ListOfTimeSheetDetails
        /// <summary>
        /// Add Or Edit TimeSheet Details 
        /// </summary>
        /// <param name="PhysicianId"></param>
        /// <param name="StartDate"></param>
        /// <param name="AfterDays"></param>
        /// <param name="AdminId"></param>
        /// <returns> That Add Or Edit TimesheetDetails List </returns>
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

                 return _context.Timesheetdetails.Where(e => e.Timesheetid == Timesheet.Timesheetid).OrderBy(r => r.Timesheetdate).ToList();
            }
            else if (data == null && AfterDays == 0)
            {
                return null;

            }
            else
            {
                return _context.Timesheetdetails.Where(e => e.Timesheetid == data.Timesheetid).OrderBy(r => r.Timesheetdate).ToList();
            }

        }
        #endregion

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

        #region Timesheet_Get

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
                }).OrderBy(r => r.Timesheetdate).ToList();

                TimeSheet.Timesheetdetailreimbursements = tr.Select(e => new Timesheetdetailreimbursements
                {
                    Amount = e.Amount,
                    Timesheetdetailreimbursementid = e.Timesheetdetailreimbursementid,
                    Isdeleted = e.Isdeleted,
                    Itemname = e.Itemname,
                    Bill = e.Bill,
                    Createddate = e.Createddate,
                    Timesheetdate = _context.Timesheetdetails.Where(r => r.Timesheetdetailid == e.Timesheetdetailid ).FirstOrDefault().Timesheetdate,
                    Timesheetid = _context.Timesheetdetails.Where(r => r.Timesheetdetailid == e.Timesheetdetailid ).FirstOrDefault().Timesheetid,
                    Modifiedby = e.Modifiedby,
                    Timesheetdetailid = e.Timesheetdetailid,
                }).OrderBy(r => r.Timesheetdetailid).ToList();

                TimeSheet.PayrateWithProvider = _context.Payratebyproviders.Where(r => r.Physicianid == PhysicianId).ToList();
                if (td.Count > 0)
                {
                TimeSheet.Timesheeid = TimeSheet.Timesheetdetails[0].Timesheetid;
                }
                TimeSheet.PhysicianId = PhysicianId;
                return TimeSheet;
            }
            catch(Exception e)
            {
                return null;
            }
          

        }
        #endregion

        #region FindOnCallProvider
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
        #endregion

        #region Timesheet_Bill_Get

        public async Task<List<Timesheetdetailreimbursement>> GetTimesheetBills( List<Timesheetdetail> TimeSheetDetails)
        {
            try
            {
                var TimeSheetBills = await _context.Timesheetdetailreimbursements
                                     .Where(e =>  TimeSheetDetails.Contains(e.Timesheetdetail)  && e.Isdeleted == false)
                                     .OrderBy(r => r.Timesheetdetailid)
                                     .ToListAsync();

                return TimeSheetBills;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        #endregion

        #region TimeSheet_Bill_AddEdit

        public bool TimeSheetBillAddEdit(Timesheetdetailreimbursements trb,string AdminId)
        {
            Timesheetdetail data = _context.Timesheetdetails.Where(e => e.Timesheetdetailid == trb.Timesheetdetailid).FirstOrDefault();
            if (data != null && trb.Timesheetdetailreimbursementid == null)
            {
                Timesheetdetailreimbursement timesheetdetailreimbursement = new Timesheetdetailreimbursement();
                timesheetdetailreimbursement.Timesheetdetailid = trb.Timesheetdetailid;
                timesheetdetailreimbursement.Amount = (int)trb.Amount;
                timesheetdetailreimbursement.Bill = CM.UploadTimesheetDoc(trb.Billfile, data.Timesheetid);
                timesheetdetailreimbursement.Itemname = trb.Itemname;
                timesheetdetailreimbursement.Createddate = DateTime.Now;
                timesheetdetailreimbursement.Createdby = AdminId;
                timesheetdetailreimbursement.Isdeleted = false;
                _context.Timesheetdetailreimbursements.Add(timesheetdetailreimbursement);
                _context.SaveChanges();
               

                return true;
            }
            else if (data != null && trb.Timesheetdetailreimbursementid != null)
            {
                Timesheetdetailreimbursement timesheetdetailreimbursement = _context.Timesheetdetailreimbursements.Where(r => r.Timesheetdetailreimbursementid == trb.Timesheetdetailreimbursementid).FirstOrDefault(); ;
                timesheetdetailreimbursement.Amount = (int)trb.Amount;
               
                timesheetdetailreimbursement.Itemname = trb.Itemname;
                timesheetdetailreimbursement.Modifieddate = DateTime.Now;
                timesheetdetailreimbursement.Modifiedby = AdminId;
                _context.Timesheetdetailreimbursements.Update(timesheetdetailreimbursement);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region TimeSheetBill_Delete

        public bool TimeSheetBillRemove(Timesheetdetailreimbursements trb, string AdminId)
        {
            Timesheetdetailreimbursement data = _context.Timesheetdetailreimbursements.Where(e => e.Timesheetdetailreimbursementid == trb.Timesheetdetailreimbursementid).FirstOrDefault();
            if (data != null)
            {
             
                data.Modifieddate = DateTime.Now;
                data.Modifiedby = AdminId;
                data.Isdeleted = true;
                _context.Timesheetdetailreimbursements.Update(data);
                _context.SaveChanges();


                return true;
            }
           return false;

        }
        #endregion


    }
}
