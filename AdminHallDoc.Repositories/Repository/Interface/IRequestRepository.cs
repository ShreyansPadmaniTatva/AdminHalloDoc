using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRequestRepository
	{
        Task<List<VenderTypeComboBox>> VenderTypeComboBox();
        Task<List<RegionComboBox>> RegionComboBox();
        Task<List<RegionComboBox>> RegionComboBox(int UserId);
        Task<List<CaseReasonComboBox>> CaseReasonComboBox();
        Task<List<UserRoleCombobox>> UserRoleComboBox();
        Task<List<UserRoleCombobox>> UserRoleComboBox(int accounttype);
        PaginatedViewModel Indexdata(int ProviderId);
        Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data);
        Task<PaginatedViewModel> GetContactAsync(string status, PaginatedViewModel data, int ProviderId);
        Task<Viewcase> GetRequestDetails(int? Requestid);
        Task<Boolean> PutViewcase(Viewcase viewcase);
        Task<bool> EmailLog(Emaillogdata elog);
        Task<bool> SMSLog(SMSLogsData elog);
        Task<bool> EmailLogForShift(Emaillogdata elog, DateTime EndDate, DateTime StartDate);
        Task<List<Physicians>> ProviderComboBox();
    }
}
