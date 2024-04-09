using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.WebPages;
using System.Xml.Linq;
using Twilio.Types;
using static AdminHalloDoc.Entities.ViewModel.Constant;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class RecordsRepository : IRecordsRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public RecordsRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region SearchRecords
        public async Task<RecordsModel> GetRequestsbyfilterForRecords(RecordsModel rm)
        {
            RecordsModel dm = new RecordsModel();



            List<ViewSearchRecord> allData = (from req in _context.Requests
                                                    join reqClient in _context.Requestclients
                                                    on req.Requestid equals reqClient.Requestid into reqClientGroup
                                                    from rc in reqClientGroup.DefaultIfEmpty()
                                                    join phys in _context.Physicians
                                                    on req.Physicianid equals phys.Physicianid into physGroup
                                                    from p in physGroup.DefaultIfEmpty()
                                                    join reg in _context.Regions
                                                    on rc.Regionid equals reg.Regionid into RegGroup
                                                    from rg in RegGroup.DefaultIfEmpty()
                                                    join nts in _context.Requestnotes
                                                    on req.Requestid equals nts.Requestid into ntsgrp
                                                    from nt in ntsgrp.DefaultIfEmpty()
                                                    where (rm.Status == 0 || req.Status == rm.Status) &&
                                                          (rm.RequestType == 0 || req.Requesttypeid == rm.RequestType) &&
                                                          (!rm.Startdate.HasValue || req.Createddate.Date >= rm.Startdate.Value.Date) &&
                                                          (!rm.Enddate.HasValue || req.Createddate.Date <= rm.Enddate.Value.Date) &&
                                                          (rm.Patientname.IsNullOrEmpty() || (rc.Firstname + " " + rc.Lastname).ToLower().Contains(rm.Patientname.ToLower())) &&
                                                          (rm.Physicianname.IsNullOrEmpty() || (p.Firstname + " " + p.Lastname).ToLower().Contains(rm.Physicianname.ToLower())) &&
                                                          (rm.Email.IsNullOrEmpty() || rc.Email.ToLower().Contains(rm.Email.ToLower())) &&
                                                          (rm.Phonenumber.IsNullOrEmpty() || rc.Phonenumber.ToLower().Contains(rm.Phonenumber.ToLower()))
                                                          orderby req.Createddate   
                                                    select new ViewSearchRecord
                                                    {
                                                        Modifieddate = req.Modifieddate,
                                                        PatientName = req.Firstname + " " + req.Lastname,
                                                        RequestID = req.Requestid,
                                                        DateOfService = req.Createddate,
                                                        PhoneNumber = rc.Phonenumber ?? "-",
                                                        Email = rc.Email ?? "-",
                                                        Address = rc.Address + "," + rc.City + " " + rc.Zipcode,
                                                        RequestTypeID = req.Requesttypeid,
                                                        Status = req.Status,
                                                        PhysicianName = p.Firstname + " " + p.Lastname ?? "-",
                                                        AdminNote = nt != null ? nt.Adminnotes ?? "-" : "-",
                                                        PhysicianNote = nt != null ? nt.Physiciannotes ?? "-" : "-",
                                                        PatientNote = rc.Notes ?? "-"
                                                    }).ToList();


            if (rm.IsAscending == true)
            {
                allData = rm.SortedColumn switch
                {
                    "CloseCaseDate" => allData.OrderBy(x => x.Modifieddate).ToList(),
                    _ => allData.OrderBy(x => x.DateOfService).ToList()
                };
            }
            else
            {
                allData = rm.SortedColumn switch
                {
                    "CloseCaseDate" => allData.OrderByDescending(x => x.Modifieddate).ToList(),
                    _ => allData.OrderBy(x => x.DateOfService).ToList()
                };
            }

            dm.TotalPages = (int)Math.Ceiling((double)allData.Count() / rm.PageSize);
            

           
            if (rm.PageSize == -1)
            {
                dm.SearchRecordList = allData.ToList();
            }
            else
            {
                dm.SearchRecordList = allData.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();
            }

            for (int i = 0; i < dm.SearchRecordList.Count; i++)
            {
                if (dm.SearchRecordList[i].Status == 9)
                {
                    dm.SearchRecordList[i].CloseCaseDate = dm.SearchRecordList[i].Modifieddate;
                }
                else
                {
                    dm.SearchRecordList[i].CloseCaseDate = null;
                }
                if (dm.SearchRecordList[i].Status == 3 && dm.SearchRecordList[i].PhysicianName != null)
                {
                    var data = _context.Requeststatuslogs.FirstOrDefault(x => (x.Status == 3) && (x.Requestid == dm.SearchRecordList[i].RequestID));
                    dm.SearchRecordList[i].CancelByProviderNote = data.Notes;
                }

            }
            dm.PageSize = rm.PageSize;
            dm.CurrentPage = rm.CurrentPage;
            dm.SortedColumn = rm.SortedColumn;
            dm.IsAscending = rm.IsAscending;
            return dm;
        }
        #endregion

        #region Patienthistory
        public async Task<RecordsModel> Patienthistorybyfilter(RecordsModel rm)
        {
            RecordsModel dm = new RecordsModel();



            List<User> allData = (from user in _context.Users
                                 where (string.IsNullOrEmpty(rm.Email) || user.Email.ToLower().Contains(rm.Email.ToLower())) &&
                                 (string.IsNullOrEmpty(rm.Phonenumber) || user.Mobile.ToLower().Contains(rm.Phonenumber.ToLower())) &&
                                 (string.IsNullOrEmpty(rm.FirstName) || user.Firstname.ToLower().Contains(rm.FirstName.ToLower())) &&
                                 (string.IsNullOrEmpty(rm.LastName) || user.Lastname.ToLower().Contains(rm.LastName.ToLower()))
                                  select new User
                                  {

                                      Userid = user.Userid,
                                      Firstname = user.Firstname,
                                      Lastname = user.Lastname,
                                      Email = user.Email,
                                      Mobile = user.Mobile,
                                      Street = user.Street,
                                      City = user.City,
                                      State = user.State,
                                      Zipcode = user.Zipcode


                                  }).ToList();



            dm.TotalPages = (int)Math.Ceiling((double)allData.Count() / rm.PageSize);
            allData = allData.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();


            dm.PatientHistorybList = allData.ToList();


   
            dm.PageSize = rm.PageSize;
            dm.CurrentPage = rm.CurrentPage;
            return dm;
        }

        public async Task<PaginatedViewModel> PatientRecord(int UserId,PaginatedViewModel data)
        {


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
                                                     where req.Userid == (UserId == null ? data.UserId : UserId)
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
                                                         Address = rc.Address + "," + rc.Street + "," + rc.City + "," + rc.State + "," + rc.Zipcode,
                                                         Notes = rc.Notes,
                                                         ProviderID = req.Physicianid,
                                                         RegionID = rc.Regionid,
                                                         RequestorPhoneNumber = req.Phonenumber,
                                                         ConcludedDate = req.Createddate,
                                                         Confirmation = req.Confirmationnumber
                                                     }).ToListAsync();

            int totalItemCount = allData.Count();
            int totalPages = (int)Math.Ceiling(totalItemCount / (double)data.PageSize);

            List<ViewDashboardList> list1 = allData.ToList();


            PaginatedViewModel paginatedViewModel = new PaginatedViewModel
            {
                UserId = UserId,
                DashboardList = list1,
                CurrentPage = data.CurrentPage,
                TotalPages = totalPages,
                PageSize = 10,
                SearchInput = data.SearchInput
            };
            return paginatedViewModel;
        }
        #endregion

        #region EmailLogs
        public async Task<RecordsModel> EmailLogs(RecordsModel rm)
        {
            RecordsModel dm = new RecordsModel();
            List<Emaillogdata> data = (from req in _context.Emaillogs
                                       where (rm.AccountType == 0 || req.Roleid == rm.AccountType) &&
                                             (!rm.Startdate.HasValue || req.Createdate.Date == rm.Startdate.Value.Date) &&
                                             (!rm.Enddate.HasValue || req.Sentdate.Value.Date == rm.Enddate.Value.Date) &&
                                             (rm.ReciverName.IsNullOrEmpty() || _context.Aspnetusers.FirstOrDefault(e => e.Email == req.Emailid).Username.ToLower().Contains(rm.ReciverName.ToLower()) ) &&
                                             (rm.Email.IsNullOrEmpty() || req.Emailid.ToLower().Contains(rm.Email.ToLower()))
                                       select new Emaillogdata
                                       {
                                           Recipient = _context.Aspnetusers.FirstOrDefault(e => e.Email == req.Emailid).Username ?? null,
                                           Confirmationnumber = req.Confirmationnumber,
                                           Createdate = req.Createdate,
                                           Emailtemplate = req.Emailtemplate,
                                           Filepath = req.Filepath,
                                           Sentdate = (DateTime)req.Sentdate,
                                           Roleid = req.Roleid,
                                           Emailid = req.Emailid,
                                           Senttries = req.Senttries,
                                           Subjectname = req.Subjectname,
                                           Action = req.Action
                                       }).ToList();



            dm.TotalPages = (int)Math.Ceiling((double)data.Count() / rm.PageSize);
            data = data.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();
            dm.EmailLogList = data.ToList();

            dm.PageSize = rm.PageSize;
            dm.CurrentPage = rm.CurrentPage;
            return dm;
        }
        #endregion

        #region SMSLogs
        public async Task<RecordsModel> SMSLogs(RecordsModel rm)
        {
            RecordsModel dm = new RecordsModel();
            List<SMSLogsData> data = (from req in _context.Smslogs
                                      where (rm.AccountType == 0 || req.Roleid == rm.AccountType) &&
                                            (!rm.Startdate.HasValue || req.Createdate.Date == rm.Startdate.Value.Date) &&
                                            (!rm.Enddate.HasValue || req.Sentdate.Value.Date == rm.Enddate.Value.Date) &&
                                            (rm.ReciverName.IsNullOrEmpty() || _context.Aspnetusers.FirstOrDefault(e => e.Phonenumber == req.Mobilenumber).Username.ToLower().Contains(rm.ReciverName.ToLower()) ) &&
                                            (rm.Phonenumber.IsNullOrEmpty() || req.Mobilenumber.ToLower().Contains(rm.Phonenumber.ToLower()))
                                      select new SMSLogsData
                                      {
                                          Recipient = _context.Aspnetusers.FirstOrDefault(e => e.Phonenumber == req.Mobilenumber).Username,
                                          Confirmationnumber = req.Confirmationnumber,
                                          Createdate = req.Createdate,
                                          Smstemplate = req.Smstemplate,
                                          Sentdate = (DateTime)req.Sentdate,
                                          Roleid = req.Roleid,
                                          Mobilenumber = req.Mobilenumber,
                                          Senttries = req.Senttries,
                                          Action = req.Action
                                      }).ToList();



            dm.TotalPages = (int)Math.Ceiling((double)data.Count() / rm.PageSize);
            data = data.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();
            dm.SMSLogList = data.ToList();

            dm.PageSize = rm.PageSize;
            dm.CurrentPage = rm.CurrentPage;
            return dm;
        }
        #endregion

        #region BlockHistory
        public async Task<RecordsModel> BlockHistory(RecordsModel rm)
        {
            RecordsModel dm = new RecordsModel();
            List<BlockRequestData> data = (from req in _context.Blockrequests
                                           where (!rm.Startdate.HasValue || req.Createddate.Value.Date == rm.Startdate.Value.Date) &&
                                                 (rm.Patientname.IsNullOrEmpty() || _context.Requests.FirstOrDefault(e => e.Requestid == Convert.ToInt32(req.Requestid)).Firstname.ToLower().Contains(rm.Patientname.ToLower())) &&
                                                 (rm.Email.IsNullOrEmpty() || req.Email.ToLower().Contains(rm.Email.ToLower())) &&
                                                 (rm.Phonenumber.IsNullOrEmpty() || req.Phonenumber.ToLower().Contains(rm.Phonenumber.ToLower()))
                                           select new BlockRequestData
                                           {
                                               PatientName = _context.Requests.FirstOrDefault(e => e.Requestid == Convert.ToInt32(req.Requestid)).Firstname,
                                               Email = req.Email,
                                               Createddate = (DateTime)req.Createddate,
                                               Isactive = req.Isactive,
                                               Requestid = Convert.ToInt32(req.Requestid),
                                               Phonenumber = req.Phonenumber,
                                               Reason = req.Reason
                                           }).ToList();



            dm.TotalPages = (int)Math.Ceiling((double)data.Count() / rm.PageSize);
            data = data.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();
            dm.BlockRequestList = data.ToList();

            dm.PageSize = rm.PageSize;
            dm.CurrentPage = rm.CurrentPage;
            return dm;
        }
        #endregion

        #region UnBlock
        public async Task<bool> UnBlock(int RequestID, string id)
        {
            try
            {
                Blockrequest br = _context.Blockrequests.FirstOrDefault(e => e.Requestid == RequestID.ToString());
                br.Isactive = new BitArray(1);
                br.Isactive[0] = true;
                _context.Blockrequests.Update(br);
                _context.SaveChanges();


                Request re = _context.Requests.FirstOrDefault(e => e.Requestid == RequestID);
                re.Status = 1;

                re.Modifieddate = DateTime.Now;
                _context.Requests.Update(re);
                _context.SaveChanges();
                var admindata = _context.Admins.FirstOrDefault(e => e.Aspnetuserid == id);
                Requeststatuslog rs = new Requeststatuslog();
                rs.Status = 1;
                rs.Requestid = RequestID;
                rs.Adminid = admindata.Adminid;
                rs.Createddate = DateTime.Now;

                _context.Requeststatuslogs.Add(rs);
                _context.SaveChanges();






                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
