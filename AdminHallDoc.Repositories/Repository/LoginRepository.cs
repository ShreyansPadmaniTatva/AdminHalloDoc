using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class LoginRepository : ILoginRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public LoginRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region Constructor
        public async Task<UserInfo> CheckAccessLogin(Aspnetuser aspNetUser)
        {
            var user = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Username == aspNetUser.Username);


            UserInfo admin = null;
            if (user != null)
            {
                var hasher = new PasswordHasher<string>();
                PasswordVerificationResult result = hasher.VerifyHashedPassword(null, user.Passwordhash, aspNetUser.Passwordhash);
                if (result != PasswordVerificationResult.Success)
                {
                   
                    return admin;
                }
                else
                {
                     admin = await (from ad in _context.Admins
                                    join r in _context.Roles
                                    on ad.Roleid equals r.Roleid into adminGroup
                                    from role in adminGroup.DefaultIfEmpty()
                                    where ad.Aspnetuserid == user.Id 
                                    select new UserInfo
                                    {
                                        Username = ad.Firstname,
                                        FirstName = ad.Firstname ?? string.Empty,
                                        LastName = ad.Lastname ?? string.Empty,
                                        Role = role.Name,
                                        UserId = ad.Adminid
                                    }).FirstOrDefaultAsync();
                    return admin;
                }
            }
            else
            {
                return admin;
            }
        }
        #endregion

    }
}
