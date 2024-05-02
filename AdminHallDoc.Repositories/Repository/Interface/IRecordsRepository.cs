using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRecordsRepository
    {
        Task<RecordsModel> GetRequestsbyfilterForRecords(RecordsModel rm);
        Task<RecordsModel> Patienthistorybyfilter(RecordsModel rm);
        Task<PaginatedViewModel> PatientRecord(int UserId, PaginatedViewModel data);
        Task<RecordsModel> EmailLogs(RecordsModel rm);
        Task<RecordsModel> SMSLogs(RecordsModel rm);
        Task<RecordsModel> BlockHistory(RecordsModel rm);
        Task<bool> UnBlock(int RequestID, string id);
        bool Delete(int RequestID, string id);
    }
}
