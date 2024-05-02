using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using static AdminHalloDoc.Repositories.Admin.Repository.LoginRepository;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<UserInfo> CheckAccessLogin(Aspnetuser aspNetUser);
        List<MenuItem> SetSubMenu(int? roleid, int menusub, List<MenuItem> s);
        List<MenuItem> SetMenu(int? roleid);
        bool IsPasswordModify(string? email);
    }
}
