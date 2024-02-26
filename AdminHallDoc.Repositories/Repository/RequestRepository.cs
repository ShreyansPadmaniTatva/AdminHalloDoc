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

        public async Task<int> CountNewRequest()
        {
			int count = 0;
			count = _context.Requests.Count(e => e.Status == 1); 
			return count;
        }
        public async Task<int> CountPandingRequest()
        {
            int count = 0;
            count = _context.Requests.Count(e => e.Status == 2); 
            return count;
        }
        public async Task<int> CountActiveRequest()
        {
            int count = 0;
            count = _context.Requests.Where(n => ( n.Status == 5 || n.Status == 6)).Count();
            return count;
        }
        public async Task<int> CountConcludeRequest()
        {
            int count = 0;
            count = _context.Requests.Count(e => e.Status == 7); 
            return count;
        }
        public async Task<int> CountToCloseRequest()
        {
            int count = 0;
            count = _context.Requests.Count(e => e.Status == 8); 
            return count;
        }
        public async Task<int> CountUnPaidRequest()
        {
            int count = 0;
            count = _context.Requests.Count(e => e.Status == 9); 
            return count;
        }

        public async Task<List<ViewDashboardList>> GetContactAsync(string status)
        {
            List<int> priceList = status.Split(',').Select(int.Parse).ToList();


            return await (from req in _context.Requests
                    join reqClient in _context.Requestclients
                    on req.Requestid equals reqClient.Requestid into reqClientGroup
                    from rc in reqClientGroup.DefaultIfEmpty()
                    join phys in _context.Physicians
                    on req.Physicianid equals phys.Physicianid into physGroup
                    from p in physGroup.DefaultIfEmpty()
                    where priceList.Contains(req.Status)
                    select new ViewDashboardList
                    {
                        RequestClientid = rc.Requestclientid,
                        Status = req.Status,
                        Requestid = req.Requestid,
                        RequestTypeID = req.Requesttypeid,
                        Requestor = req.Firstname + " " + req.Lastname,
                        PatientName = rc.Firstname + rc.Lastname,
                        RequestedDate = req.Createddate,
                        Dob = new DateTime((int)rc.Intyear,(int) Convert.ToInt32(rc.Strmonth), (int)rc.Intdate),
                        PhoneNumber = rc.Phonenumber,
                        Address = rc.Address + "," + rc.Street + "," + rc.City + "," + rc.State + "," + rc.Zipcode,
                        Notes = rc.Notes,
                        ProviderID = req.Physicianid,
                        RegionID = rc.Regionid,
                        RequestorPhoneNumber = req.Phonenumber
                    }).ToListAsync();

           
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
                     BirthDate = new DateTime((int)requestclients.Intyear, Convert.ToInt32(requestclients.Strmonth), (int)requestclients.Intdate),
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
                DateTime sd = viewcase.BirthDate.Value;
                requestclient.Firstname = viewcase.FirstName;
                requestclient.Lastname = viewcase.LastName;
                requestclient.Phonenumber = viewcase.PhoneNumber;
                requestclient.Email = viewcase.Email;
                requestclient.Intdate = sd.Day;
                requestclient.Intyear = sd.Year;
                requestclient.Strmonth = sd.Month.ToString();
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
