using AdminHalloDoc.Entities.Models;

namespace AdminHalloDoc.Repositories.Repository.Interface
{
	public interface IRequestRepository
	{
        Task<int> CountNewRequest();
        Task<List<Request>> GetContactAsync();
    }
}
