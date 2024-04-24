using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IInvoiceRepository
    {
        bool isFinalizeTimesheet(int PhysicianId, DateOnly StartDate);
        List<Timesheetdetail> PostTimesheetDetails(int PhysicianId, DateOnly StartDate, int AfterDays, string AdminId);
        ViewTimeSheet GetTimesheetDetails(List<Timesheetdetail> td, List<Timesheetdetailreimbursement> tr, int PhysicianId);
        bool PutTimesheetDetails(List<Timesheetdetails> tds, string AdminId);
    }
}
