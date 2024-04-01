using System;
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

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class RequestRepository : IRequestRepository
	{
		private readonly ApplicationDbContext _context;

		public RequestRepository(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<List<UserRoleCombobox>> UserRoleComboBox()
        {
            return await _context.Roles.Select(req => new UserRoleCombobox()
            {
                RoleId = req.Roleid,
                RoleName = req.Name
            })
                .ToListAsync();
        }
        public async Task<List<VenderTypeComboBox>> VenderTypeComboBox()
        {
            return await _context.Healthprofessionaltypes.Select(req => new VenderTypeComboBox()
            {
                VenderTypeId = req.Healthprofessionalid,
                Name = req.Professionname
            })
                .ToListAsync();
        }

        public async Task<List<RegionComboBox>> RegionComboBox()
        {
            return await _context.Regions.Select(req => new RegionComboBox()
                {
                   RegionId =req.Regionid,
                   RegionName =req.Name
                })
                .ToListAsync();
        }
        public async Task<List<CaseReasonComboBox>> CaseReasonComboBox()
        {
            return await _context.Casetags.Select(req => new CaseReasonComboBox()
            {
                CaseReasonId = req.Casetagid,
                CaseReasonName = req.Name
            })
                .ToListAsync();
        }

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
                    rc.Firstname.Contains(data.SearchInput) || rc.Lastname.Contains(data.SearchInput) ||
                    req.Firstname.Contains(data.SearchInput) || req.Lastname.Contains(data.SearchInput) ||
                    rc.Email.Contains(data.SearchInput) || rc.Phonenumber.Contains(data.SearchInput) ||
                    rc.Address.Contains(data.SearchInput) || rc.Notes.Contains(data.SearchInput) ||
                    p.Firstname.Contains(data.SearchInput) || p.Lastname.Contains(data.SearchInput) ||
                    rg.Name.Contains(data.SearchInput)) && (data.RegionId == null || rc.Regionid == data.RegionId)
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
                        RequestedDate = req.Createddate,
                        Dob = new DateTime((int)rc.Intyear,(int) Convert.ToInt32(rc.Strmonth), (int)rc.Intdate),
                        PhoneNumber = rc.Phonenumber,
                        Address = rc.Address + "," + rc.Street + "," + rc.City + "," + rc.State + "," + rc.Zipcode,
                        Notes = rc.Notes,
                        ProviderID = req.Physicianid,
                        RegionID = rc.Regionid,
                        RequestorPhoneNumber = req.Phonenumber
                    }).ToListAsync();

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
                SearchInput = data.SearchInput
            };
            return paginatedViewModel;
        }

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
                                                         Address = rc.Address + "," + rc.Street + "," + rc.City + "," + rc.State + "," + rc.Zipcode,
                                                         Notes = rc.Notes,
                                                         ProviderID = req.Physicianid,
                                                         RegionID = rc.Regionid,
                                                         RequestorPhoneNumber = req.Phonenumber
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
                    PhoneNumber = requestclients.Phonenumber,
                    Address = requestclients.Address + "," + requestclients.Street + "," + requestclients.City + "," + requestclients.State + "," + requestclients.Zipcode,
                   
                    RegionID = requestclients.Regionid
                }
            ).FirstOrDefaultAsync();

            return requestDetails;
        }

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
    }
}
