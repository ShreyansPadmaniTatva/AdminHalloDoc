using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class ViewActionRepository : IViewActionRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public ViewActionRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region GetDocumentByRequest
        public async Task<List<ViewPatientDashboard>> GetDocumentByRequest(int? id)
        {

            return _context.Requestwisefiles
                        .Where(r => r.Requestid == id)
                        .OrderByDescending(x => x.Createddate)
                        .Select(r => new ViewPatientDashboard
                        {
                            Requestid = r.Requestid,
                            Createddate = r.Createddate,
                            Filename = r.Filename

                        })
                        .ToList(); 
        }
        #endregion

        #region Send_Link
        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber)
        {
            var baseUrl = httpContextAccessor.HttpContext?.Request.Host;

            _emailConfig.SendMail(email, "Add New Request", firstname +" "+ lastname +" "+ phonenumber+"  " +baseUrl+ "/SubmitRequest");

            return true;
        }
        #endregion

        #region Save_Document
        public Boolean SaveDoc(int Requestid, IFormFile file)
        {
            string UploadDoc = CM.UploadDoc(file, Requestid);

            var requestwisefile = new Requestwisefile
            {
                Requestid = Requestid,
                Filename = UploadDoc,
                Createddate = DateTime.Now,
            };
            _context.Requestwisefiles.Add(requestwisefile);
            _context.SaveChanges();

            return true;
        }
        #endregion

        #region Provider_By_Region
        public async Task<List<Physician>> ProviderbyRegion(int? regionid)
        {
            var result = await _context.Physicians
                        .Where(r => r.Regionid == regionid)
                        .OrderByDescending(x => x.Createddate)
                        .ToListAsync();

            return result;
        }
        #endregion

        #region Assign_Provider
        public async Task<Boolean> AssignProvider(int RequestId, int ProviderId, string notes)
        {
            
                var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == (int)RequestId);
                request.Physicianid = ProviderId;
                _context.Requests.Update(request);
                _context.SaveChanges();

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = RequestId;
                rsl.Physicianid = ProviderId;
                rsl.Notes = notes;
                rsl.Createddate = DateTime.Now;
                rsl.Status = 1;
                _context.Requeststatuslogs.Update(rsl);
                _context.SaveChanges();

                return true;
           
            
        }
        #endregion

        #region Transfer_Provider
        public async Task<Boolean> TransferToProvider(int RequestId, int ProviderId, string notes, int TransferToProviderId)
        {

            var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == (int)RequestId);
            request.Physicianid = ProviderId;
            _context.Requests.Update(request);
            _context.SaveChanges();

            Requeststatuslog rsl = new Requeststatuslog();
            rsl.Requestid = RequestId;
            rsl.Physicianid = ProviderId;
            rsl.Transtophysicianid = TransferToProviderId;
            rsl.Notes = notes;
            rsl.Createddate = DateTime.Now;
            rsl.Status = 1;
            _context.Requeststatuslogs.Update(rsl);
            _context.SaveChanges();

            return true;


        }
        #endregion
    }
}
