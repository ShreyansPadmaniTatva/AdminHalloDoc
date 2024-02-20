using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;

namespace AdminHalloDoc.Repositories.Repository.Interface
{
	public interface IRequestRepository
	{
        Task<int> CountNewRequest();
        Task<int> CountPandingRequest();
        Task<int> CountActiveRequest();
        Task<int> CountConcludeRequest();
        Task<int> CountToCloseRequest();
        Task<int> CountUnPaidRequest();
        Task<List<ViewDashboardList>> GetContactAsync(string status);
    }
}
