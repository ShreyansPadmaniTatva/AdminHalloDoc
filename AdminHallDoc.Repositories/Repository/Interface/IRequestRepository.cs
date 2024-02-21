using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewsModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRequestRepository
	{
        Task<List<RegionComboBox>> RegionComboBox();
        Task<int> CountNewRequest();
        Task<int> CountPandingRequest();
        Task<int> CountActiveRequest();
        Task<int> CountConcludeRequest();
        Task<int> CountToCloseRequest();
        Task<int> CountUnPaidRequest();
        Task<List<ViewDashboardList>> GetContactAsync(string status);
    }
}
