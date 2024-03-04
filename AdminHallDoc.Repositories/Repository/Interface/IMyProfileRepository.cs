using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IMyProfileRepository
    {
        Task<ViewAdminProfile> GetProfileDetails(int UserId);
        Task<bool> PutProfileDetails(ViewAdminProfile v);
    }
}
