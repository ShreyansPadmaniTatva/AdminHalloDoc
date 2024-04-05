using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
