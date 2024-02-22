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
        #region Configuration
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


        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber)
        {
            var baseUrl = httpContextAccessor.HttpContext?.Request.Host;

            _emailConfig.SendMail(email, "Add New Request", firstname +" "+ lastname +" "+ phonenumber+"  " +baseUrl+ "/SubmitRequest");

            return true;
        }

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
        public async Task<List<Physician>> ProviderbyRegion(int? regionid)
        {
            var result = await _context.Physicians
                        .Where(r => r.Regionid == regionid)
                        .OrderByDescending(x => x.Createddate)
                        .ToListAsync();

            return result;
        }

    }
}
