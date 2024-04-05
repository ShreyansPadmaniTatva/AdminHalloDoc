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
        Task<bool> ChangePasswordAsync(string password, int AdminId);
        Task<bool> EditAdminProfileAsync(ViewAdminProfile vm);
        Task<bool> EditBillingInfoAsync(ViewAdminProfile vm);
        Task<bool> SaveAdminInfo(ViewAdminProfile vm);
        Task<bool> AdminPost(ViewAdminProfile admindata, string AdminId);
        bool IsUsernameAvailable(string username);
    }
}
