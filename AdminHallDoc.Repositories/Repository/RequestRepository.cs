using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class RequestRepository : IRequestRepository
	{
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly EmailConfiguration _emailConfig;


        public RequestRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
		{
			_context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region UserRoleComboBox
        public async Task<List<UserRoleCombobox>> UserRoleComboBox()
        {
            return await _context.Roles.Select(req => new UserRoleCombobox()
            {
                RoleId = req.Roleid,
                RoleName = req.Name
            })
                .ToListAsync();
        }
        #endregion

        #region UserRoleComboBox
        public async Task<List<UserRoleCombobox>> UserRoleComboBox(int accounttype)
        {
            return await _context.Roles.Where(r => r.Accounttype == accounttype).Select(req => new UserRoleCombobox()
            {
                RoleId = req.Roleid,
                RoleName = req.Name
            })
                .ToListAsync();
        }
        #endregion

        #region VenderTypeComboBox
        public async Task<List<VenderTypeComboBox>> VenderTypeComboBox()
        {
            return await _context.Healthprofessionaltypes.Select(req => new VenderTypeComboBox()
            {
                VenderTypeId = req.Healthprofessionalid,
                Name = req.Professionname
            })
                .ToListAsync();
        }
        #endregion

        #region RegionComboBox
        public async Task<List<RegionComboBox>> RegionComboBox()
        {
            return await _context.Regions.Select(req => new RegionComboBox()
                {
                   RegionId =req.Regionid,
                   RegionName =req.Name
                })
                .ToListAsync();
        }
        #endregion

        #region RegionComboBox
        public async Task<List<RegionComboBox>> RegionComboBox(int UserId)
        {
            if(UserId < 0 || UserId == null)
            {
                return await _context.Regions.Select(req => new RegionComboBox()
                {
                    RegionId = req.Regionid,
                    RegionName = req.Name
                })
               .ToListAsync();
               
            }
            else
            {
                List<int> combinedData = ((from pr in _context.Physicianregions
                                           where pr.Physicianid == UserId
                                           select pr.Regionid)
                            .Union
                            (from ar in _context.Adminregions
                             where ar.Adminid == UserId
                             select ar.Regionid)).ToList();



                return await _context.Regions.Where(r => combinedData.Contains(r.Regionid)).Select(req => new RegionComboBox()
                {
                    RegionId = req.Regionid,
                    RegionName = req.Name
                })
                    .ToListAsync();
            }
            
        }
        #endregion

        #region CaseReasonComboBox
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CaseReasonComboBox>> CaseReasonComboBox()
        {
            return await _context.Casetags.Select(req => new CaseReasonComboBox()
            {
                CaseReasonId = req.Casetagid,
                CaseReasonName = req.Name
            })
                .ToListAsync();
        }
        #endregion

        #region Number_Of_Request_For_Provider
        /// <summary>
        /// Number_Of_Request_For_Provider
        /// </summary>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        public PaginatedViewModel Indexdata(int ProviderId)
        {
            if(ProviderId < 0)
            {
                return new PaginatedViewModel
                {
                    NewRequest = _context.Requests.Where(r => r.Status == 1).Count(),
                    PendingRequest = _context.Requests.Where(r => r.Status == 2).Count(),
                    ActiveRequest = _context.Requests.Where(r => (r.Status == 4 || r.Status == 5)).Count(),
                    ConcludeRequest = _context.Requests.Where(r => r.Status == 6).Count(),
                    ToCloseRequest = _context.Requests.Where(r => (r.Status == 3 || r.Status == 7 || r.Status == 8)).Count(),
                    UnpaidRequest = _context.Requests.Where(r => r.Status == 9).Count()
                };
            }
            return new PaginatedViewModel
            {
                NewRequest = _context.Requests.Where(r => r.Status == 1 && r.Physicianid == ProviderId).Count(),
                PendingRequest = _context.Requests.Where(r => r.Status == 2 && r.Physicianid == ProviderId).Count(),
                ActiveRequest = _context.Requests.Where(r => (r.Status == 4 || r.Status == 5) && r.Physicianid == ProviderId).Count(),
                ConcludeRequest = _context.Requests.Where(r => r.Status == 6 && r.Physicianid == ProviderId).Count(),
                ToCloseRequest = _context.Requests.Where(r => (r.Status == 3 || r.Status == 7 || r.Status == 8) && r.Physicianid == ProviderId).Count(),
                UnpaidRequest = _context.Requests.Where(r => r.Status == 9 && r.Physicianid == ProviderId).Count()
            };
        }
        #endregion

        #region GetDashboard_Notes
        /// <summary>
        /// Get DashBorad Notes With Request
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        public string GetDashboardNotesName(int requestid)
        {
            Requeststatuslog requeststatuslog = _context.Requeststatuslogs.OrderByDescending(x => x.Createddate).Where(x => x.Requestid == requestid).FirstOrDefault();
            Requestclient request = _context.Requestclients.Where(x => x.Requestid == requestid).FirstOrDefault();
            AdminHalloDoc.Entities.Models.Admin admin = new AdminHalloDoc.Entities.Models.Admin();
            Physician physician = new Physician();
            Physician transphysician = new Physician();

            if (requeststatuslog != null)
            {
                if (requeststatuslog.Adminid != null)
                {
                    admin = _context.Admins.FirstOrDefault(x => x.Adminid == requeststatuslog.Adminid);
                }
                if (requeststatuslog.Physicianid != null)
                {
                    physician = _context.Physicians.FirstOrDefault(x => x.Physicianid == requeststatuslog.Physicianid);
                }
                if (requeststatuslog.Transtophysicianid != null)
                {
                    transphysician = _context.Physicians.FirstOrDefault(x => x.Physicianid == requeststatuslog.Transtophysicianid);
                }

                if (requeststatuslog.Adminid != null)
                {
                    if (requeststatuslog.Status == 2 && requeststatuslog.Transtophysicianid != null)
                    {
                        return "Admin " + admin.Firstname + " " + admin.Lastname + " transferred to Physician " + transphysician.Firstname + " " + transphysician.Lastname + " on " + requeststatuslog.Createddate.ToString();
                    }
                    if (requeststatuslog.Status == 3)
                    {
                        return "Admin " + admin.Firstname + " " + admin.Lastname + " cancelled on " + requeststatuslog.Createddate.ToString();
                    }
                }

                if (requeststatuslog.Physicianid != null)
                {
                    if (requeststatuslog.Status == 3)
                    {
                        return "Physician " + physician.Firstname + " " + physician.Lastname + " cancelled on " + requeststatuslog.Createddate.ToString();
                    }
                    return "";
                }

                if (requeststatuslog.Adminid == null && requeststatuslog.Physicianid == null)
                {
                    if (requeststatuslog.Status == 4)
                    {
                        return "Patient accepted agreement on " + requeststatuslog.Createddate.ToString();
                    }
                    if (requeststatuslog.Status == 7)
                    {
                        return "Patient rejected agreement on " + requeststatuslog.Createddate.ToString();
                    }
                }
            }
            else
            {
                return request.Notes;
            }

            return "";
        }
        #endregion

        #region GetContactAsync_For_Admin
        public async Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data)
        {
            List<int> statusdata = status.Split(',').Select(int.Parse).ToList();


           List<ViewDashboardList> allData = await (from req in _context.Requests
                    join reqClient in _context.Requestclients
                    on req.Requestid equals reqClient.Requestid into reqClientGroup
                    from rc in reqClientGroup.DefaultIfEmpty()
                    join phys in _context.Physicians
                    on req.Physicianid equals phys.Physicianid into physGroup
                    from p in physGroup.DefaultIfEmpty()
                    join reg in _context.Regions
                    on rc.Regionid equals reg.Regionid into RegGroup
                    from rg in RegGroup.DefaultIfEmpty()
                    where statusdata.Contains(req.Status) && (data.SearchInput == null ||
                    rc.Firstname.ToLower().Contains(data.SearchInput.ToLower()) || rc.Lastname.ToLower().Contains(data.SearchInput.ToLower()) ||
                    req.Firstname.ToLower().Contains(data.SearchInput.ToLower()) || req.Lastname.ToLower().Contains(data.SearchInput.ToLower()) ||
                    rc.Email.ToLower().Contains(data.SearchInput.ToLower()) || rc.Phonenumber.ToLower().Contains(data.SearchInput.ToLower()) ||
                    rc.Address.ToLower().Contains(data.SearchInput.ToLower()) || rc.Notes.ToLower().Contains(data.SearchInput.ToLower()) ||
                    p.Firstname.ToLower().Contains(data.SearchInput.ToLower()) || p.Lastname.ToLower().Contains(data.SearchInput.ToLower()) ||
                    rg.Name.ToLower().Contains(data.SearchInput.ToLower())) && (data.RegionId == null || rc.Regionid == data.RegionId)
                     && (data.RequestType == null || req.Requesttypeid == data.RequestType)
                     select new ViewDashboardList
                    {
                        Physician = p.Firstname + " " + p.Lastname,
                        RequestClientid = rc.Requestclientid,
                        Status = req.Status,
                        Requestid = req.Requestid,
                        RequestTypeID = req.Requesttypeid,
                        Requestor = req.Firstname + " " + req.Lastname,
                        PatientName = rc.Firstname + " " + rc.Lastname,
                        RequestedDate = req.Accepteddate == null ? req.Createddate : req.Accepteddate,
                        Dob = new DateTime((int)rc.Intyear,(int) Convert.ToInt32(rc.Strmonth), (int)rc.Intdate),
                        PhoneNumber = rc.Phonenumber,
                        Address = rc.Address ,
                        ProviderID = req.Physicianid,
                        RegionID = rc.Regionid,
                        RequestorPhoneNumber = req.Phonenumber,
                         ModifiedDate = req.Modifieddate,
                         IsFinalize = _context.Encounterforms.Any(ef => ef.Requestid == req.Requestid && ef.Isfinalize),
                     })
                    .OrderByDescending(x => x.ModifiedDate == null?x.RequestedDate:x.ModifiedDate).ToListAsync();

            foreach (ViewDashboardList item in allData)
            {
                item.Notes = GetDashboardNotesName((int)item.Requestid) + " ";
            }

            if (data.SortedColumn != null)
            {
                if (data.IsAscending == true)
            {
                allData = data.SortedColumn switch
                {
                    "Name" => allData.OrderBy(x => x.PatientName).ToList(),
                    "Requestor" => allData.OrderBy(x => x.Physician).ToList(),
                    "Requested" => allData.OrderBy(x => x.RequestedDate).ToList(),
                    "Physician" => allData.OrderBy(x => x.Physician).ToList()
                };
            }
            else
            {
                allData = data.SortedColumn switch
                {
                    "Name" => allData.OrderByDescending(x => x.PatientName).ToList(),
                    "Requestor" => allData.OrderByDescending(x => x.Physician).ToList(),
                    "Requested" => allData.OrderByDescending(x => x.RequestedDate).ToList(),
                    "Physician" => allData.OrderByDescending(x => x.Physician).ToList()
                };
            }
            }
            

            int totalItemCount = allData.Count();
            int totalPages = (int)Math.Ceiling(totalItemCount / (double)data.PageSize);
            
            List<ViewDashboardList> list1 = allData.Skip((data.CurrentPage - 1) * data.PageSize).Take(data.PageSize).ToList();
            if (data.PageSize == -1)
            {
                list1 = allData.ToList();
            }

            PaginatedViewModel paginatedViewModel = new PaginatedViewModel
            {
                DashboardList = list1,
                CurrentPage = data.CurrentPage,
                TotalPages = totalPages,
                PageSize = 10,
                SearchInput = data.SearchInput,
                SortedColumn = data.SortedColumn,
                IsAscending = data.IsAscending
        };
            return paginatedViewModel;
        }
        #endregion

        #region GetContactAsync_For_Provider
        /// <summary>
        /// Get All Request For Only Provider
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        public async Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data, int ProviderId)
        {
            List<int> statusdata = status.Split(',').Select(int.Parse).ToList();


            List<ViewDashboardList> allData = await (from req in _context.Requests
                                                     join reqClient in _context.Requestclients
                                                     on req.Requestid equals reqClient.Requestid into reqClientGroup
                                                     from rc in reqClientGroup.DefaultIfEmpty()
                                                     join phys in _context.Physicians
                                                     on req.Physicianid equals phys.Physicianid into physGroup
                                                     from p in physGroup.DefaultIfEmpty()
                                                     join reg in _context.Regions
                                                     on rc.Regionid equals reg.Regionid into RegGroup
                                                     from rg in RegGroup.DefaultIfEmpty()
                                                     where statusdata.Contains(req.Status) && (data.SearchInput == null ||
                                                     rc.Firstname.Contains(data.SearchInput) || rc.Lastname.Contains(data.SearchInput) ||
                                                     req.Firstname.Contains(data.SearchInput) || req.Lastname.Contains(data.SearchInput) ||
                                                     rc.Email.Contains(data.SearchInput) || rc.Phonenumber.Contains(data.SearchInput) ||
                                                     rc.Address.Contains(data.SearchInput) || rc.Notes.Contains(data.SearchInput) ||
                                                     p.Firstname.Contains(data.SearchInput) || p.Lastname.Contains(data.SearchInput) ||
                                                     rg.Name.Contains(data.SearchInput)) && (data.RegionId == null || rc.Regionid == data.RegionId)
                                                      && (data.RequestType == null || req.Requesttypeid == data.RequestType) && req.Physicianid == ProviderId
                                                     select new ViewDashboardList
                                                     {
                                                         Physician = p.Firstname + " " + p.Lastname,
                                                         RequestClientid = rc.Requestclientid,
                                                         Status = req.Status,
                                                         Requestid = req.Requestid,
                                                         RequestTypeID = req.Requesttypeid,
                                                         Requestor = req.Firstname + " " + req.Lastname,
                                                         PatientName = rc.Firstname + " " + rc.Lastname,
                                                         RequestedDate = req.Createddate,
                                                         Dob = new DateTime((int)rc.Intyear, (int)Convert.ToInt32(rc.Strmonth), (int)rc.Intdate),
                                                         PhoneNumber = rc.Phonenumber,
                                                         Address = rc.Address ,
                                                         Notes = rc.Notes,
                                                         ProviderID = req.Physicianid,
                                                         RegionID = rc.Regionid,
                                                         RequestorPhoneNumber = req.Phonenumber,
                                                         IsFinalize = _context.Encounterforms.Any(ef => ef.Requestid == req.Requestid && ef.Isfinalize),
                                                         ModifiedDate = req.Modifieddate,
                                                     }).OrderByDescending(x => x.ModifiedDate).ToListAsync();

            int totalItemCount = allData.Count();
            int totalPages = (int)Math.Ceiling(totalItemCount / (double)data.PageSize);

            List<ViewDashboardList> list1 = allData.Skip((data.CurrentPage - 1) * data.PageSize).Take(data.PageSize).ToList();


            PaginatedViewModel paginatedViewModel = new PaginatedViewModel
            {
                DashboardList = list1,
                CurrentPage = data.CurrentPage,
                TotalPages = totalPages,
                PageSize = 10,
                SearchInput = data.SearchInput
            };
            return paginatedViewModel;
        }
        #endregion

        #region GetRequestDetails
        /// <summary>
        /// find request base on request client id for changes or updateding
        /// </summary>
        /// <param name="RequestClientid"></param>
        /// <returns></returns>
        public async Task<Viewcase> GetRequestDetails(int? RequestClientid)
        {


            var requestDetails = await (
                from requestclients in _context.Requestclients
                join requests in _context.Requests
                on requestclients.Requestid equals requests.Requestid
                where requestclients.Requestclientid == RequestClientid
                select new Viewcase()
                {
                    RequesClientid = requestclients.Requestclientid,
                    Requestid = requests.Requestid,
                    RequestTypeID = requests.Requesttypeid,
                    FirstName = requestclients.Firstname,
                    LastName = requestclients.Lastname,
                    Email = requestclients.Email,
                    Notes = requestclients.Notes,
                    BirthDate = new DateTime(requestclients.Intyear != null ? (int)requestclients.Intyear : 0001, Convert.ToInt32(requestclients.Strmonth != null ? requestclients.Strmonth : 1), requestclients.Intdate != null ? (int)requestclients.Intdate : 01) != new DateTime(0001, 01, 01) ? new DateTime((int)requestclients.Intyear != 0 ? (int)requestclients.Intyear : 1, Convert.ToInt32(requestclients.Strmonth != null ? requestclients.Strmonth : 1), (int)requestclients.Intdate != 0 ? (int)requestclients.Intdate : 1) : null,
                   // BirthDate = new DateTime((int)requestclients.Intyear, Convert.ToInt32(requestclients.Strmonth), (int)requestclients.Intdate),
                   Status = requests.Status,
                    PhoneNumber = requestclients.Phonenumber,
                    Address = requestclients.Address + "," + requestclients.Street + "," + requestclients.City + "," + requestclients.State + "," + requestclients.Zipcode,
                   
                    RegionID = requestclients.Regionid
                }
            ).FirstOrDefaultAsync();

            return requestDetails;
        }
        #endregion

        #region PutViewcase
        public async Task<Boolean> PutViewcase(Viewcase viewcase)
        {
            try
            {

                var requestclient = await _context.Requestclients.Where(r => r.Requestclientid == viewcase.RequesClientid).FirstAsync();
                DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 123);
                DateTime sd = viewcase.BirthDate != null  ? viewcase.BirthDate.Value : dt;
                
                requestclient.Firstname = viewcase.FirstName != null ? viewcase.FirstName : requestclient.Firstname;
                requestclient.Lastname = viewcase.LastName != null ? viewcase.LastName : requestclient.Lastname;
                requestclient.Phonenumber = viewcase.PhoneNumber;
                requestclient.Email = viewcase.Email;
                if (sd != dt)
                {
                    requestclient.Intdate = sd.Day;
                    requestclient.Intyear = sd.Year;
                    requestclient.Strmonth = sd.Month.ToString();
                }
               
                _context.Requestclients.Update(requestclient);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    throw;
                
            }
            return true;
        }
        #endregion

        #region Email_Log
        public async Task<bool> EmailLog(Emaillogdata elog)        {            try            {                Emaillog log = new Emaillog();                //log.Emaillogid = Guid.NewGuid().ToString();                log.Emailtemplate = elog.Emailtemplate;                log.Subjectname = elog.Subjectname;                log.Emailid = elog.Emailid;                log.Roleid = elog.Roleid;                log.Createdate = DateTime.Now;                log.Sentdate = DateTime.Now;                log.Adminid =elog.Adminid;                log.Requestid = elog.Requestid;                log.Physicianid = elog.Physicianid;                log.Action = elog.Action;                log.Recipient = elog.Recipient;                if (elog.Requestid != null)                {
                    log.Confirmationnumber = _context.Requests.FirstOrDefault(r => r.Requestid == elog.Requestid).Confirmationnumber;                }                //if (await _emailConfig.SendMail(elog.Emailid, elog.Subjectname, elog.Emailtemplate))                //{                    log.Isemailsent = new BitArray(new[] { true }); ;                //}                //else                //{                //    log.Isemailsent = new BitArray(new[] { false }); ;                //}                log.Senttries = elog.Senttries;                _context.Emaillogs.Add(log);                _context.SaveChanges();                return true;            }            catch (Exception ex)            {                return false;            }        }
        #endregion

    }
}
