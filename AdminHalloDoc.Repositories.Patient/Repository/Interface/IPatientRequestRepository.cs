using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Patient.Repository.Interface
{
    public interface IPatientRequestRepository
    {
        Task<bool> PatientCreateRequest(ViewPatientCreateRequest viewpatientcreaterequest);
    }
}
