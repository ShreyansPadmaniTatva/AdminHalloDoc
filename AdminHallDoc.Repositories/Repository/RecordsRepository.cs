using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.WebPages;
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
                                                        PhoneNumber = rc.Phonenumber ?? "",
                                                        Email = rc.Email ?? "",
                                                        Address = rc.Address + "," + rc.City + " " + rc.Zipcode,
                                                        RequestTypeID = req.Requesttypeid,
                                                        Status = req.Status,
                                                        PhysicianName = p.Firstname + " " + p.Lastname ?? "",
                                                        AdminNote = nt != null ? nt.Adminnotes ?? "" : "",
                                                        PhysicianNote = nt != null ? nt.Physiciannotes ?? "" : "",
                                                        PatientNote = rc.Notes ?? ""
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
            allData = allData.Skip((rm.CurrentPage - 1) * rm.PageSize).Take(rm.PageSize).ToList();


            dm.SearchRecordList = allData.ToList();


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
                                                     where req.Userid == UserId
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
    }
}
