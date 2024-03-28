using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<UserInfo> CheckAccessLogin(Aspnetuser aspNetUser);
        Task<List<Menu>> ListMenuByRole(int? roleid);
        List<MenuItem> SetMenu(int? roleid);
    }
}
