using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class RoleAccessRepository : IRoleAccessRepository
    {
        #region Constructor
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public RoleAccessRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region GetRoleAccessDetails
        public async Task<List<Role>> GetRoleAccessDetails()
        {

            List<Role> v = await _context.Roles.ToListAsync();

            //List<Regions> regions = new List<Regions>();

            //regions = await _context.Adminregions
            //      .Where(r => r.Adminid == UserId)
            //      .Select(req => new Regions()
            //      {
            //          regionid = req.Regionid
            //      })
            //      .ToListAsync();

            //v.Regionids = regions;
            return v;

        }
        #endregion

        #region GetMenusByAccount
        public async Task<List<AdminHalloDoc.Entities.Models.Menu>> GetMenusByAccount(short Accounttype)
        {
            if (Accounttype == 1)
            {
                return await _context.Menus.ToListAsync();
            }
            else
            {
                return await _context.Menus.Where(r => r.Accounttype == Accounttype).ToListAsync();
            }
        }
        #endregion

        #region GetRoleByMenus
        public async Task<ViewRoleByMenu> GetRoleByMenus(int roleid)
        {
            return await _context.Roles
                        .Where(r => r.Roleid == roleid)
                        .Select( r => new ViewRoleByMenu{
                            Accounttype = r.Accounttype,
                            Createdby = r.Createdby,
                            Roleid = r.Roleid,
                            Name = r.Name,
                            Isdeleted = r.Isdeleted
                        })
                        .FirstOrDefaultAsync();
        }
        #endregion

        #region PostRoleMenu
        public async Task<bool> PostRoleMenu(ViewRoleByMenu role ,string Menusid,string ID)
        {
            try
            {
               Role check = await _context.Roles.Where(r => r.Name == role.Name).FirstOrDefaultAsync();
                if (check == null && role != null && Menusid !=null)
                {
                    
                    Role r = new Role();
                    r.Name = role.Name;
                    r.Accounttype = role.Accounttype;
                    r.Createdby = ID;
                    r.Createddate = DateTime.Now;
                    r.Isdeleted = new System.Collections.BitArray(1);
                    r.Isdeleted[0] =false;
                    _context.Roles.Add(r);
                    _context.SaveChanges();

                    List<int> priceList = Menusid.Split(',').Select(int.Parse).ToList();
                    foreach (var item in priceList)
                    {
                        Rolemenu ar = new Rolemenu();
                        ar.Roleid = r.Roleid;
                        ar.Menuid = item;
                        _context.Rolemenus.Add(ar);

                    }
                        _context.SaveChanges();
                    return true;
                }else
                {
                    return false;
                }
            }catch (Exception ex)
            {
                return false;
            }
               
        }
        #endregion

        #region PutRoleMenu
        public async Task<bool> PutRoleMenu(ViewRoleByMenu role, string Menusid, string ID)
        {
            try
            {
                Role check = await _context.Roles.Where(r => r.Roleid == role.Roleid).FirstOrDefaultAsync();
                if (check != null && role != null && Menusid != null)
                {
                    check.Name = role.Name;
                    check.Accounttype = role.Accounttype;
                    check.Modifiedby = ID;
                    check.Modifieddate = DateTime.Now;
                    _context.Roles.Update(check);
                    _context.SaveChanges();


                    List<int> regions = await CheckMenuByRole(check.Roleid);

                    List<int> priceList = Menusid.Split(',').Select(int.Parse).ToList();

                    foreach (var item in priceList)
                    {
                        if (regions.Contains(item))
                        {
                            regions.Remove(item);
                        }
                        else
                        {
                            Rolemenu ar = new Rolemenu();
                            ar.Menuid = item;
                            ar.Roleid = check.Roleid;
                            _context.Rolemenus.Update(ar);
                            await _context.SaveChangesAsync();
                            regions.Remove(item);

                        }
                    }
                    if (regions.Count > 0)
                    {
                        foreach (var item in regions)
                        {
                            Rolemenu ar = await _context.Rolemenus.Where(r => r.Roleid == check.Roleid && r.Menuid == item).FirstAsync();
                            _context.Rolemenus.Remove(ar);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region CheckMenuByRole
        public async Task<List<int>> CheckMenuByRole(int roleid)
        {
            return await _context.Rolemenus
                        .Where(r => r.Roleid == roleid)
                        .Select( r => r.Menuid)
                        .ToListAsync();
        }
        #endregion

        #region GetProfileAll
        public async Task<List<ViewUserAcces>> GetAllUserDetails(int? User)
        {
            IQueryable<ViewUserAcces> query =
                from user in _context.Aspnetusers
                join admin in _context.Admins on user.Id equals admin.Aspnetuserid into adminGroup
                from admin in adminGroup.DefaultIfEmpty()
                join physician in _context.Physicians on user.Id equals physician.Aspnetuserid into physicianGroup
                from physician in physicianGroup.DefaultIfEmpty()
                where (admin != null || physician != null) &&
                      (admin.Isdeleted == new BitArray(1) || physician.Isdeleted == new BitArray(1))
                select new ViewUserAcces
                {
                    UserName = user.Username,
                    FirstName = admin != null ? admin.Firstname : (physician != null ? physician.Firstname : null) ?? "-",
                    isAdmin = admin != null,
                    UserID = admin != null ? admin.Adminid : (physician != null ? physician.Physicianid : null),
                    accounttype = admin != null ? 2 : (physician != null ? 3 : null),
                    status = admin != null ? admin.Status : (physician != null ? physician.Status : null),
                    Mobile = admin != null ? admin.Mobile : (physician != null ? physician.Mobile : null) ?? "-",
                    OpenRequest = physician != null ? _context.Requests.Count(r => r.Physicianid == physician.Physicianid) : 0
                };

            if (User.HasValue)
            {
                switch (User.Value)
                {
                    case 2: // Admin data
                        query = query.Where(u => u.isAdmin);
                        break;
                    case 3: // Provider data
                        query = query.Where(u => !u.isAdmin);
                        break;

                }
            }

            return await query.ToListAsync();
        }
        //public async Task<List<ViewUserAcces>> GetAllUserDetails(int? User)
        //{


        //    List<ViewUserAcces> v = await (
        //                                 from user in _context.Aspnetusers
        //                                 join admin in _context.Admins on user.Id equals admin.Aspnetuserid into adminGroup
        //                                 from admin in adminGroup.DefaultIfEmpty()
        //                                 join physician in _context.Physicians on user.Id equals physician.Aspnetuserid into physicianGroup
        //                                 from physician in physicianGroup.DefaultIfEmpty()
        //                                 where (admin != null || physician != null) && (admin.Isdeleted==new BitArray(1) || physician.Isdeleted == new BitArray(1))

        //                                 select new ViewUserAcces
        //                                 {
        //                                     UserName = user.Username,
        //                                     FirstName = admin != null ? admin.Firstname : (physician != null ? physician.Firstname : null),
        //                                     isAdmin = admin != null ? true : false,
        //                                     UserID = admin != null ? admin.Adminid : (physician != null ? physician.Physicianid : null),
        //                                     accounttype = admin != null ? 2 : (physician != null ? 3 : null),
        //                                     status = admin != null ? admin.Status : (physician != null ? physician.Status : null),
        //                                     Mobile = admin != null ? admin.Mobile : (physician!=null ? physician.Mobile : null),
        //                                 }
        //                             ).ToListAsync();
        //    return v;

        //}


        #endregion

    }
}
