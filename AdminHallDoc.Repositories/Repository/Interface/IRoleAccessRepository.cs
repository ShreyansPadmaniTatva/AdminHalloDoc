using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IRoleAccessRepository
    {
        Task<List<Role>> GetRoleAccessDetails();
        Task<List<Menu>> GetMenusByAccount(short Accounttype);
        Task<bool> PostRoleMenu(ViewRoleByMenu role, string Menusid, string ID);
        Task<ViewRoleByMenu> GetRoleByMenus(int roleid);
        Task<List<int>> CheckMenuByRole(int roleid);
        Task<bool> PutRoleMenu(ViewRoleByMenu role, string Menusid, string ID);
        Task<List<ViewUserAcces>> GetAllUserDetails();
    }
}
