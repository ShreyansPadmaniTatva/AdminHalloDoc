using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
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
        private readonly IRequestRepository _requestrepository;
        private readonly IPhysicianRepository _physicianrepository;
        public SchedulingRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IRequestRepository requestrepository, IPhysicianRepository physicianrepository)
        {
            _context = context;
            _emailConfig = emailConfig;
            _requestrepository = requestrepository;
            _physicianrepository = physicianrepository;
        }
        #endregion

        #region PhysicianAll
        public async Task<List<Schedule>> PhysicianAll()
        {

            List<Schedule> ScheduleDetails = new List<Schedule>();

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
            foreach (Physicians schedule in pl)
            {
                List<Schedule> ss = await (from s in _context.Shifts
                                           join pd in _context.Physicians
                                           on s.Physicianid equals pd.Physicianid
                                           join sd in _context.Shiftdetails
                                           on s.Shiftid equals sd.Shiftid into shiftGroup
                                           from sd in shiftGroup.DefaultIfEmpty()
                                           join rg in _context.Regions
                                           on sd.Regionid equals rg.Regionid
                                           where s.Physicianid == schedule.Physicianid && sd.Isdeleted == new BitArray(1) 
                                           select new Schedule
                                           {
                                               RegionName = rg.Name,
                                               Shiftid = sd.Shiftdetailid,
                                               Status = sd.Status,
                                               Starttime = sd.Starttime,
                                               Shiftdate = sd.Shiftdate,
                                               Endtime = sd.Endtime,
                                               PhysicianName = pd.Firstname + ' ' + pd.Lastname,

                                           })
                                              .ToListAsync();

                Schedule temp = new Schedule();
                temp.PhysicianName = schedule.Firstname + ' ' + schedule.Lastname;
                temp.PhysicianPhoto = schedule.Photo;
                temp.Physicianid = (int)schedule.Physicianid;
                temp.DayList = ss;
                ScheduleDetails.Add(temp);
            }

            return ScheduleDetails;


        }
        #endregion

        #region PhysicianByRegion
        public async Task<List<Schedule>> PhysicianByRegion(int? region)
        {
            List<Schedule> ScheduleDetails = new List<Schedule>();
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

                                        where pr.Regionid == region && r.Isdeleted == new BitArray(1)
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
                                            Status = r.Status,
                                            Email = r.Email,
                                            Photo = r.Photo

                                        })
                                        .ToListAsync();


            foreach (Physicians schedule in pl)
            {
                List<Schedule> ss = await (from s in _context.Shifts
                                           join pd in _context.Physicians
                                           on s.Physicianid equals pd.Physicianid
                                           join sd in _context.Shiftdetails
                                           on s.Shiftid equals sd.Shiftid into shiftGroup
                                           from sd in shiftGroup.DefaultIfEmpty()
                                           join rg in _context.Regions
                                           on sd.Regionid equals rg.Regionid
                                           where s.Physicianid == schedule.Physicianid && sd.Isdeleted == new BitArray(1) && sd.Regionid == region
                                           select new Schedule
                                           {
                                               RegionName = rg.Abbreviation,
                                               Shiftid = sd.Shiftdetailid,
                                               Status = sd.Status,
                                               Starttime = sd.Starttime,
                                               Shiftdate = sd.Shiftdate,
                                               Endtime = sd.Endtime,
                                               PhysicianName = pd.Firstname + ' ' + pd.Lastname,
                                           })
                                             .ToListAsync();

                Schedule temp = new Schedule();
                temp.PhysicianName = schedule.Firstname + ' ' + schedule.Lastname;
                temp.PhysicianPhoto = schedule.Photo;
                temp.Physicianid = (int)schedule.Physicianid;
                temp.DayList = ss;
                ScheduleDetails.Add(temp);
            }

            return ScheduleDetails;

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

                Shiftdetail sd = new Shiftdetail();
                sd.Shiftid = shift.Shiftid;
                sd.Shiftdate = new DateTime(s.Startdate.Year,s.Startdate.Month,s.Startdate.Day);
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
                if (s.Status == 1)
                {
                    Emaillogdata elog = new Emaillogdata();
                    elog.Emailtemplate = "Your Shift  Add To Calendar";
                    elog.Subjectname = "Your Appr Shift Details";
                    elog.Emailid = (await _physicianrepository.GetPhysicianById(s.Physicianid)).Email;
                    elog.Createdate = DateTime.Now;
                    elog.Sentdate = DateTime.Now;
                    elog.Action = 10;
                    elog.Recipient = (await _physicianrepository.GetPhysicianById(s.Physicianid)).Firstname ;
                    elog.Roleid = 3;
                    elog.Senttries = 1;
                    await _requestrepository.EmailLogForShift(elog, sd.Shiftdate.Date + s.Starttime.ToTimeSpan(), sd.Shiftdate.Date + s.Endtime.ToTimeSpan());
                }
                if (s.checkWeekday != null)
                {
                    List<int> day = s.checkWeekday.Split(',').Select(int.Parse).ToList();

                    foreach (int d in day)
                    {
                        DayOfWeek desiredDayOfWeek = (DayOfWeek)d;
                        DateTime today = DateTime.Today.AddDays(1);
                        DateTime nextOccurrence = new DateTime(s.Startdate.Year, s.Startdate.Month, s.Startdate.Day + 1);
                        int occurrencesFound = 0;
                        while (occurrencesFound < s.Repeatupto)
                        {
                            if (nextOccurrence.DayOfWeek == desiredDayOfWeek)
                            {

                                Shiftdetail sdd = new Shiftdetail();
                                sdd.Shiftid = shift.Shiftid;
                                sdd.Shiftdate = nextOccurrence;
                                sdd.Starttime = s.Starttime;
                                sdd.Endtime = s.Endtime;
                                sdd.Regionid = s.Regionid;
                                sdd.Status = s.Status;
                                sdd.Isdeleted = new BitArray(1);
                                sdd.Isdeleted[0] = false;
                                _context.Shiftdetails.Add(sdd);
                                _context.SaveChanges();

                                Shiftdetailregion srr = new Shiftdetailregion();
                                srr.Shiftdetailid = sdd.Shiftdetailid;
                                srr.Regionid = s.Regionid;
                                srr.Isdeleted = new BitArray(1);
                                srr.Isdeleted[0] = false;
                                _context.Shiftdetailregions.Add(srr);
                                _context.SaveChanges();
                                occurrencesFound++;
                                if (s.Status == 1)
                                {
                                    Emaillogdata elog = new Emaillogdata();
                                    elog.Emailtemplate = "Your Shift  Add To Calendar";
                                    elog.Subjectname = " Shift Details";
                                    elog.Emailid = (await _physicianrepository.GetPhysicianById(s.Physicianid)).Email;
                                    elog.Createdate = DateTime.Now;
                                    elog.Sentdate = DateTime.Now;
                                    elog.Action = 10;
                                    elog.Recipient = (await _physicianrepository.GetPhysicianById(s.Physicianid)).Firstname;
                                    elog.Roleid = 3;
                                    elog.Senttries = 1;
                                   await _requestrepository.EmailLogForShift(elog, sd.Shiftdate.Date + s.Starttime.ToTimeSpan(), sd.Shiftdate.Date + s.Endtime.ToTimeSpan());
                                }

                            }
                            nextOccurrence = nextOccurrence.AddDays(1);
                        }
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

        #region UpdateStatusShift
        public async Task<bool> UpdateStatusShift(string s, string AdminID)
        {
            List<int> shidtID = s.Split(',').Select(int.Parse).ToList();
            try
            {
                foreach(int i in shidtID)
                {
                  Shiftdetail sd = _context.Shiftdetails.FirstOrDefault(sd => sd.Shiftdetailid == i);
                    if (sd != null)
                    {
                        sd.Status = (short)(sd.Status == 1 ? 0 : 1);
                        sd.Modifiedby = AdminID;
                        sd.Modifieddate = DateTime.Now;
                        _context.Shiftdetails.Update(sd);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return false;
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

        #region EditShift
        public async Task<bool> EditShift(Schedule s, string AdminID)
        {
            try
            {
                Shiftdetail sd =  _context.Shiftdetails.FirstOrDefault(sd => sd.Shiftdetailid == s.Shiftid);
                if (sd != null)
                {
                    sd.Shiftdate = (DateTime)s.Shiftdate;
                    sd.Starttime = s.Starttime;
                    sd.Endtime = s.Endtime;
                    sd.Modifiedby = AdminID;
                    sd.Modifieddate = DateTime.Now;
                    _context.Shiftdetails.Update(sd);
                    _context.SaveChanges();
                }
                else
                {
                    return false;
                }
              
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region GetShift_For_Month
        public async Task<List<Schedule>> GetShift(int year,int month,int? regionId)
        {
            List<Schedule> ScheduleDetails = new List<Schedule>();

            var uniqueDates = await _context.Shiftdetails
                            .Where(sd => sd.Shiftdate.Month == month && sd.Shiftdate.Year == year && sd.Isdeleted == new BitArray(1) && (regionId == null || regionId == -1 || sd.Regionid == regionId)) // Filter by month if needed
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
                                          where sd.Shiftdate == schedule && sd.Isdeleted == new BitArray(1)
                                          select new Schedule
                                          {
                                              Shiftid = sd.Shiftdetailid,
                                              Status = sd.Status,
                                              Starttime = sd.Starttime,
                                              Endtime = sd.Endtime,
                                              PhysicianName = pd.Firstname + ' '+pd.Lastname,
                                              Physicianid = pd.Physicianid
                                          })
                                             .ToListAsync();

               Schedule temp = new Schedule();
                temp.Shiftdate = schedule;
                temp.DayList = ss;
                ScheduleDetails.Add(temp);
            }


            return ScheduleDetails;

        }
        #endregion

        #region GetShiftByShiftdetailId
        public async Task<Schedule> GetShiftByShiftdetailId(int Shiftdetailid)
        {
            
                Schedule ss =  (from s in _context.Shifts
                                           join pd in _context.Physicians
                                           on s.Physicianid equals pd.Physicianid
                                           join sd in _context.Shiftdetails
                                           on s.Shiftid equals sd.Shiftid into shiftGroup
                                           from sd in shiftGroup.DefaultIfEmpty()
                                           join rg in _context.Regions
                                           on sd.Regionid equals rg.Regionid
                                           where sd.Shiftdetailid == Shiftdetailid
                                           select new Schedule
                                           {
                                               Regionid = (int)sd.Regionid,
                                               Shiftid = sd.Shiftdetailid,
                                               Status = sd.Status,
                                               Starttime = sd.Starttime,
                                               Endtime = sd.Endtime,
                                               Physicianid = s.Physicianid,
                                               PhysicianName = pd.Firstname + ' ' + pd.Lastname,
                                               Shiftdate = sd.Shiftdate
                                           })
                                              .FirstOrDefault();

               
        


            return ss;

        }
        #endregion

        #region GetAllNotApprovedShift
        public async Task<List<Schedule>> GetAllNotApprovedShift(int? regionId)
        {

           List<Schedule> ss = await (from s in _context.Shifts
                               join pd in _context.Physicians
                               on s.Physicianid equals pd.Physicianid
                               join sd in _context.Shiftdetails
                               on s.Shiftid equals sd.Shiftid into shiftGroup
                               from sd in shiftGroup.DefaultIfEmpty()
                               join rg in _context.Regions
                               on sd.Regionid equals rg.Regionid
                               where (regionId == null || regionId == -1 || sd.Regionid == regionId) && sd.Status == 0 && sd.Isdeleted == new BitArray(1)
                               select new Schedule
                               {
                                   Regionid = (int)sd.Regionid,
                                   RegionName = rg.Name,
                                   Shiftid = sd.Shiftdetailid,
                                   Status = sd.Status,
                                   Starttime = sd.Starttime,
                                   Endtime = sd.Endtime,
                                   Physicianid = s.Physicianid,
                                   PhysicianName = pd.Firstname + ' ' + pd.Lastname,
                                   Shiftdate = sd.Shiftdate
                               })
                               .ToListAsync();

            return ss;

        }
        #endregion

        #region DeleteShift
        public async Task<bool> DeleteShift(string s, string AdminID)
        {
            List<int> shidtID = s.Split(',').Select(int.Parse).ToList();
            try
            {
                foreach (int i in shidtID)
                {
                    Shiftdetail sd = _context.Shiftdetails.FirstOrDefault(sd => sd.Shiftdetailid == i);
                    if (sd != null)
                    {
                        sd.Isdeleted[0] = true;
                        sd.Modifiedby = AdminID;
                        sd.Modifieddate = DateTime.Now;
                        _context.Shiftdetails.Update(sd);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return false;
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

        #region PhysicianOnCall
        public async Task<List<Physicians>> PhysicianOnCall(int? region)
        {
            DateTime currentDateTime = DateTime.Now;
            TimeOnly currentTimeOfDay = TimeOnly.FromDateTime(DateTime.Now);

            List<Physicians> pl = await (from r in _context.Physicians
                                         where r.Isdeleted == new BitArray(1) 
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
                                             Status = r.Status,
                                             Email = r.Email,
                                             Photo = r.Photo

                                         }).ToListAsync();
            if (region != null)
            {
                pl = await (
                                        from pr in _context.Physicianregions

                                        join ph in _context.Physicians
                                         on pr.Physicianid equals ph.Physicianid into rGroup
                                        from r in rGroup.DefaultIfEmpty()
                                        where pr.Regionid == region && r.Isdeleted == new BitArray(1)
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
                                            Status = r.Status,
                                            Email = r.Email,
                                            Photo = r.Photo

                                        })
                                        .ToListAsync();
            }

            foreach (var item in pl)
            {
                List<int> shiftIds = await (from s in _context.Shifts
                                            where s.Physicianid == item.Physicianid
                                            select s.Shiftid).ToListAsync();

                foreach (var shift in shiftIds)
                {
                    var shiftDetail = (from sd in _context.Shiftdetails
                                       where sd.Shiftid == shift &&
                                             sd.Shiftdate.Date == currentDateTime.Date &&
                                             sd.Starttime <= currentTimeOfDay &&
                                             currentTimeOfDay <= sd.Endtime
                                       select sd).FirstOrDefault();

                    if (shiftDetail != null)
                    {
                        item.onCallStatus = 1;
                    }
                }
            }

            return pl;


        }
        #endregion

    }
}
