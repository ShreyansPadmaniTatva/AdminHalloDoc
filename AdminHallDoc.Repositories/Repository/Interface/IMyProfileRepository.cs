using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

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
