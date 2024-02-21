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


           return await _context.Requests
                    .Join(
                  _context.Requestclients,
                  requestclients => requestclients.Requestid,
                  requests => requests.Requestid,
                  (requests, requestclients) => new  { Requests = requests  , Requestclients = requestclients  }
                  )
                  // .Join(
                  //  _context.Physicians,
                  //  join => join.Requests.Physicianid,
                  //  physicianid => physicianid.Physicianid,
                  //  (join, physicianid) => new { Join = join, Physicians = physicianid }
                  // )
                  //.Where(p => priceList.Contains(p.Join.Requests.Status))
                  //.Select(req => new ViewDashboardList()
                  //{
                  //    RequestTypeID = req.Join.Requests.Requesttypeid,
                  //    Requestor = req.Join.Requests.Firstname + " " + req.Join.Requests.Lastname,
                  //    PatientName = req.Join.Requestclients.Firstname + " " + req.Join.Requestclients.Lastname,
                  //    Dob = new DateTime((int)req.Join.Requestclients.Intyear, DateTime.ParseExact(req.Join.Requestclients.Strmonth, "MMMM", new CultureInfo("en-US")).Month, (int)req.Join.Requestclients.Intdate),
                  //    RequestedDate = req.Join.Requests.Createddate,
                  //    PhoneNumber = req.Join.Requestclients.Phonenumber,
                  //    Address = req.Join.Requestclients.Address + "," + req.Join.Requestclients.Street + "," + req.Join.Requestclients.City + "," + req.Join.Requestclients.State + "," + req.Join.Requestclients.Zipcode,
                  //    Notes = req.Join.Requestclients.Notes,
                  //    ProviderID = req.Join.Requests.Physicianid,
                  //    RequestorPhoneNumber = req.Join.Requests.Phonenumber
                  //})
                  .Where(p => priceList.Contains(p.Requests.Status))
                  .Select(req => new ViewDashboardList()
                  {
                      RequestClientid = req.Requestclients.Requestclientid,
                      Requestid = req.Requests.Requestid,
                      RequestTypeID = req.Requests.Requesttypeid,
                      Requestor = req.Requests.Firstname + " " + req.Requests.Lastname,
                      PatientName = req.Requestclients.Firstname + " " + req.Requestclients.Lastname,
                      //Dob = new DateTime((int)req.Requestclients.Intyear, DateTime.ParseExact(req.Requestclients.Strmonth, "MMMM", new CultureInfo("en-US")).Month, (int)req.Requestclients.Intdate),
                      RequestedDate = req.Requests.Createddate,
                      PhoneNumber = req.Requestclients.Phonenumber,
                      Address = req.Requestclients.Address + "," + req.Requestclients.Street + "," + req.Requestclients.City + "," + req.Requestclients.State + "," + req.Requestclients.Zipcode,
                      Notes = req.Requestclients.Notes,
                      ProviderID = req.Requests.Physicianid,
                      RegionID = req.Requestclients.Regionid,
                      RequestorPhoneNumber = req.Requests.Phonenumber
                  })
                  .ToListAsync(); 
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
                    // BirthDate = new DateTime((int)requestclients.Intyear, DateTime.ParseExact(requestclients.Strmonth, "MMMM", new CultureInfo("en-US")).Month, (int)requestclients.Intdate),
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

                requestclient.Firstname = viewcase.FirstName;
                requestclient.Lastname = viewcase.LastName;
                requestclient.Phonenumber = viewcase.PhoneNumber;
                requestclient.Email = viewcase.Email;
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
