using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IViewNotesRepository
    {
        Task<ViewNotesModel> GetNotesByRequest(int? id);
        bool PutNotes(string? adminnotes, string? physiciannotes, int? RequestID);
    }
}
