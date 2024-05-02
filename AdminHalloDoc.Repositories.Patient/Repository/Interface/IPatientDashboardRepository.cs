using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using Microsoft.AspNetCore.Http;

namespace AdminHalloDoc.Repositories.Patient.Repository.Interface
{
    public interface IPatientDashboardRepository
    {
        List<ViewPatientDashboard> DashboardData(int UserID);
        Task<bool> ProfileEdit(ViewUserProfile userprofile);
        ViewUserProfile Profile(int UserID);
        List<ViewPatientDashboard> Documentsinfo(int Requestid);
        Task<bool> UploadDoc(int Requestid, IFormFile file);
    }
}
