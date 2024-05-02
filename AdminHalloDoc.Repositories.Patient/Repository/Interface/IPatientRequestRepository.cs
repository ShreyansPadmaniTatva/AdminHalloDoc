using AdminHalloDoc.Entities.ViewModel.PatientViewModel;

namespace AdminHalloDoc.Repositories.Patient.Repository.Interface
{
    public interface IPatientRequestRepository
    {
        Task<bool> PatientCreateRequest(ViewPatientCreateRequest viewpatientcreaterequest);
        Task<bool> PatientFamilyFriend(ViewPatientFamilyFriend viewpatientcreaterequest);
        Task<bool> PatientConcierge(ViewPatientConcierge viewdata);
        Task<bool> PatientBusiness(ViewPatientBusiness viewdata);
        Task<bool> PatientForMe(ViewPatientCreateRequest viewpatientcreaterequest, int UserId);
        Task<bool> PatientForSomeoneElse(ViewPatientCreateRequest viewpatientcreaterequest);
        bool IsEmailBlock(String Email);
    }
}
