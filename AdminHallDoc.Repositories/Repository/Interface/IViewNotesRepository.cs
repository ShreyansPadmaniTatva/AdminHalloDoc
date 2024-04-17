using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IViewNotesRepository
    {
        Task<ViewNotesModel> GetNotesByRequest(int id);
         bool PutNotes(string? adminnotes, string? physiciannotes, int? RequestID, string Id);
        Task<List<VenderComboBox>> FindVenderByVenderType(int? id);
        Task<ViewOrder> FindVenderByVenderID(int? id);
        Task<List<ViewVendorList>> GetPartnersByProfession(int? regionId, string? searchvender);
        Task<bool> SaveViewOrder(ViewOrder viewOrder);
        Task<bool> SavePartner(Healthprofessional SavePartner);
        Task<Healthprofessional> GetPartnerById(int? venderId);
        Task<bool> DeletePartnerById(int? venderId);
        List<Healthprofessional> isBusinessNameExist(string businessName);
        List<Healthprofessional> isEmailExist(string Email);
    }
}
