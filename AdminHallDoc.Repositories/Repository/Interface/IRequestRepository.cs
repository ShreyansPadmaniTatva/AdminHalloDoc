using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRequestRepository
	{
        Task<List<RegionComboBox>> RegionComboBox();
        Task<List<CaseReasonComboBox>> CaseReasonComboBox();
        Task<int> CountNewRequest();
        Task<int> CountPandingRequest();
        Task<int> CountActiveRequest();
        Task<int> CountConcludeRequest();
        Task<int> CountToCloseRequest();
        Task<int> CountUnPaidRequest();
        Task<List<ViewDashboardList>> GetContactAsync(string status);
        Task<Viewcase> GetRequestDetails(int? Requestid);
        Task<Boolean> PutViewcase(Viewcase viewcase);
    }
}
