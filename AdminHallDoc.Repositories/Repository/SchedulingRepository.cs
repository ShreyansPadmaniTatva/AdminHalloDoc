using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class SchedulingRepository : ISchedulingRepository
    {
        #region Constructor
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public SchedulingRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region PhysicianAll
        public async Task<List<Physicians>> PhysicianAll()
        {


            List<Physicians> pl = await (from r in _context.Physicians
                                         join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into aspGroup
                                         from nof in aspGroup.DefaultIfEmpty()
                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()
                                         where r.Isdeleted == new BitArray(1)
                                         select new Physicians
                                         {
                                             notificationid = nof.Id,
                                             Createddate = r.Createddate,
                                             Physicianid = r.Physicianid,
                                             Address1 = r.Address1,
                                             Address2 = r.Address2,
                                             Adminnotes = r.Adminnotes,
                                             Altphone = r.Altphone,
                                             Businessname = r.Businessname,
                                             Businesswebsite = r.Businesswebsite,
                                             City = r.City,
                                             Firstname = r.Firstname,
                                             Lastname = r.Lastname,
                                             notification = nof.Isnotificationstopped,
                                             role = roles.Name,
                                             Status = r.Status,
                                             Email = r.Email,
                                             Photo = r.Photo

                                         })
                                        .ToListAsync();

            return pl;

        }
        #endregion

        #region PhysicianByRegion
        public async Task<List<Physicians>> PhysicianByRegion(int? region)
        {
            List<Physicians> pl = await (
                                        from pr in _context.Physicianregions

                                        join ph in _context.Physicians
                                         on pr.Physicianid equals ph.Physicianid into rGroup
                                        from r in rGroup.DefaultIfEmpty()

                                        join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into aspGroup
                                        from nof in aspGroup.DefaultIfEmpty()

                                        join role in _context.Roles
                                        on r.Roleid equals role.Roleid into roleGroup
                                        from roles in roleGroup.DefaultIfEmpty()

                                        where pr.Regionid == region
                                        select new Physicians
                                        {
                                            Createddate = r.Createddate,
                                            Physicianid = r.Physicianid,
                                            Address1 = r.Address1,
                                            Address2 = r.Address2,
                                            Adminnotes = r.Adminnotes,
                                            Altphone = r.Altphone,
                                            Businessname = r.Businessname,
                                            Businesswebsite = r.Businesswebsite,
                                            City = r.City,
                                            Firstname = r.Firstname,
                                            Lastname = r.Lastname,
                                            notification = nof.Isnotificationstopped,
                                            role = roles.Name,
                                            Status = r.Status

                                        })
                                        .ToListAsync();


            return pl;

        }
        #endregion

        #region CreateShift
        public async Task<bool> CreateShift(Schedule s,string AdminID)
        {
            try
            {
                Shift shift = new Shift();
                shift.Physicianid = s.Physicianid;
                shift.Repeatupto = s.Repeatupto;
                shift.Startdate = s.Startdate;
                shift.Createdby = AdminID; 
                shift.Createddate = DateTime.Now;
                shift.Isrepeat =new BitArray(1);
                shift.Isrepeat[0] = s.Isrepeat;
                _context.Shifts.Add(shift);
                _context.SaveChanges();

                List<int> day = s.checkWeekday.Split(',').Select(int.Parse).ToList();

                foreach (int d in day)
                {
                    DayOfWeek desiredDayOfWeek = (DayOfWeek)d;
                    DateTime today = DateTime.Today;
                    DateTime nextOccurrence = today;
                    int occurrencesFound = 0;
                    while (occurrencesFound < s.Repeatupto)
                    {
                        if (nextOccurrence.DayOfWeek == desiredDayOfWeek)
                        {
                          
                            Shiftdetail sd = new Shiftdetail();
                            sd.Shiftid = shift.Shiftid;
                            sd.Shiftdate = nextOccurrence;
                            sd.Starttime = s.Starttime;
                            sd.Endtime = s.Endtime;
                            sd.Regionid = s.Regionid;
                            sd.Status = s.Status;
                            sd.Isdeleted = new BitArray(1);
                            sd.Isdeleted[0] = false;
                            _context.Shiftdetails.Add(sd);
                            _context.SaveChanges();

                            Shiftdetailregion sr = new Shiftdetailregion();
                            sr.Shiftdetailid = sd.Shiftdetailid;
                            sr.Regionid = s.Regionid;
                            sr.Isdeleted = new BitArray(1);
                            sr.Isdeleted[0] = false;
                            _context.Shiftdetailregions.Add(sr);
                            _context.SaveChanges();
                            occurrencesFound++;

                        }
                        nextOccurrence = nextOccurrence.AddDays(1);
                    }
                }
              

                

                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region GetShift
        public async Task<List<Schedule>> GetShift(int month)
        {
            List<Schedule> ScheduleDetails = new List<Schedule>();

            var uniqueDates = await _context.Shiftdetails
                            .Where(sd => sd.Shiftdate.Month == month) // Filter by month if needed
                            .Select(sd => sd.Shiftdate.Date) // Select the date part of Shiftdate
                            .Distinct() // Get distinct dates
                            .ToListAsync();


            foreach (DateTime schedule in uniqueDates)
            {
               List<Schedule> ss = await (from s in _context.Shifts
                                          join pd in _context.Physicians
                                          on s.Physicianid equals pd.Physicianid
                                          join sd in _context.Shiftdetails
                                          on s.Shiftid equals sd.Shiftid into shiftGroup
                                          from sd in shiftGroup.DefaultIfEmpty()
                                          where sd.Shiftdate == schedule
                                          select new Schedule
                                          {
                                              Starttime = sd.Starttime,
                                              Endtime = sd.Starttime,
                                              PhysicianName = pd.Firstname + ' '+pd.Lastname,
                                          })
                                             .ToListAsync();

               Schedule temp = new Schedule();
                temp.ShidtDate = schedule;
                temp.DayList = ss;
                ScheduleDetails.Add(temp);
            }


            return ScheduleDetails;

        }
        #endregion

    }
}
