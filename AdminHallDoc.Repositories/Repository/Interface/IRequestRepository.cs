using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRequestRepository
	{
        Task<List<VenderTypeComboBox>> VenderTypeComboBox();
        Task<List<RegionComboBox>> RegionComboBox();
        Task<List<CaseReasonComboBox>> CaseReasonComboBox();
        Task<List<UserRoleCombobox>> UserRoleComboBox();
        PaginatedViewModel Indexdata(int ProviderId);
        Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data);
        Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data, int ProviderId);
        Task<Viewcase> GetRequestDetails(int? Requestid);
        Task<Boolean> PutViewcase(Viewcase viewcase);
    }
}
