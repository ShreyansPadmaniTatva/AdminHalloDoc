using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
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
        public Task<ViewDocuments> GetDocumentByRequest(int? id);
        public Boolean SaveDoc(int Requestid, IFormFile file);
        public Task<List<Physician>> ProviderbyRegion(int? regionid);
         Task<Boolean> AssignProvider(int RequestId, int ProviderId, string notes);
        Task<ViewActions> GetRequestDetails(int? id);
        Task<bool> TransferToProvider(ViewActions v);
        Task<bool> CancelCase(ViewActions v);
        Task<bool> BlockCase(ViewActions v);
        Task<bool> AssignPhysician(ViewActions v);
    }
}
