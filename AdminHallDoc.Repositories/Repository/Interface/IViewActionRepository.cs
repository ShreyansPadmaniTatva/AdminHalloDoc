using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IViewActionRepository
    {
        public Boolean SendLink(string firstname, string lastname, string email, string phonenumber);
        public  Task<List<ViewPatientDashboard>> GetDocumentByRequest(int? id);
        public Boolean SaveDoc(int Requestid, IFormFile file);
        public Task<List<Physician>> ProviderbyRegion(int? regionid);
    }
}
