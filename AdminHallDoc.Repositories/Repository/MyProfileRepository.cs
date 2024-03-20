using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.AdminViewModel.ViewAdminProfile;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class MyProfileRepository : IMyProfileRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public MyProfileRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region GetProfile
        public async Task<ViewAdminProfile> GetProfileDetails(int UserId)
        {

            ViewAdminProfile v = await (from r in _context.Admins
                                        join Aspnetuser in _context.Aspnetusers
                                        on r.Aspnetuserid equals Aspnetuser.Id into aspGroup
                                        from asp in aspGroup.DefaultIfEmpty()
                                        where r.Adminid == UserId
                                        select new ViewAdminProfile
                                        {
                                            
                                            Roleid = r.Roleid,
                                            AdminId = r.Adminid,
                                            UserName = asp.Username,
                                            Address1 = r.Address1,
                                            Address2 = r.Address2,
                                            AltMobile = r.Altphone,
                                            City = r.City,
                                            Aspnetuserid = r.Aspnetuserid,
                                            Createdby = r.Createdby,
                                            Email = r.Email,
                                            Createddate = r.Createddate,
                                            Mobile = r.Mobile,
                                            Modifiedby = r.Modifiedby,
                                            Modifieddate = r.Modifieddate,
                                            Regionid = r.Regionid,
                                            Lastname = r.Lastname,
                                            Firstname = r.Firstname,
                                            Status = r.Status,
                                            Zipcode = r.Zip
                                        }).FirstOrDefaultAsync();

            List<Regions> regions = new List<Regions>();

          regions =  await _context.Adminregions
                .Where(r => r.Adminid == UserId )
                .Select(req => new Regions()
            {
                regionid = req.Regionid
            })
                .ToListAsync();

            v.Regionids = regions;
            return v;

        }
        #endregion

        #region Admin_Add
        public async Task<bool> AdminPost(ViewAdminProfile admindata, string AdminId)
        {
            try
            {
                if (admindata.UserName != null && admindata.Password != null)
                {
                    //Aspnet_user
                    var Aspnetuser = new Aspnetuser();
                    var hasher = new PasswordHasher<string>();
                    Aspnetuser.Id = Guid.NewGuid().ToString();
                    Aspnetuser.Username = admindata.UserName;
                    Aspnetuser.Passwordhash = hasher.HashPassword(null, admindata.Password);
                    Aspnetuser.Email = admindata.Email;
                    Aspnetuser.CreatedDate = DateTime.Now;
                    _context.Aspnetusers.Add(Aspnetuser);
                    _context.SaveChanges();

                    //aspnet_user_roles
                    var aspnetuserroles = new Aspnetuserrole();
                    aspnetuserroles.Userid = Aspnetuser.Id;
                    aspnetuserroles.Roleid = "Admin";
                    _context.Aspnetuserroles.Add(aspnetuserroles);
                    _context.SaveChanges();

                    //Admin
                    var Admin = new AdminHalloDoc.Entities.Models.Admin();
                    Admin.Aspnetuserid = Aspnetuser.Id;
                    Admin.Firstname = admindata.Firstname;
                    Admin.Lastname = admindata.Lastname;
                    Admin.Status = admindata.Status;
                    Admin.Roleid = admindata.Roleid;
                    Admin.Email = admindata.Email;
                    Admin.Mobile = admindata.Mobile;
                    Admin.Isdeleted = new BitArray(1);
                    Admin.Isdeleted[0] = false;
                    Admin.Address1 = admindata.Address1;
                    Admin.Address2 = admindata.Address2;
                    Admin.City = admindata.City;
                    Admin.Zip = admindata.Zipcode;
                    Admin.Altphone = admindata.AltMobile;
                   
                    Admin.Createddate = DateTime.Now;
                    Admin.Createdby = AdminId;
                     //Admin.Regionid = admindata.Regionid;

                    _context.Admins.Add(Admin);
                    _context.SaveChanges();

                    //Admin_region
                    List<int> priceList = admindata.Regionsid.Split(',').Select(int.Parse).ToList();
                    foreach (var item in priceList)
                    {
                        Adminregion ar = new Adminregion();
                        ar.Regionid = item;
                        ar.Adminid = (int)Admin.Adminid;
                        _context.Adminregions.Add(ar);
                        _context.SaveChanges();

                    }


                }

                else
                {

                }


                return true;
            }
            catch (Exception e)
            {

            }
            return false;
        }
        #endregion

        #region Put_Profile

        #region SavePhysicianInfo
        public async Task<bool> SaveAdminInfo(ViewAdminProfile  vm)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Admins
                        .Where(W => W.Adminid == vm.AdminId)
                        .FirstOrDefaultAsync();
                    Aspnetuser U = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Id == DataForChange.Aspnetuserid);

                    if (DataForChange != null)
                    {

                        U.Username = vm.UserName;
                        DataForChange.Status = vm.Status;
                        DataForChange.Roleid = vm.Roleid;


                        _context.Admins.Update(DataForChange);
                        _context.Aspnetusers.Update(U);
                        _context.SaveChanges();


                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Edit_Admin_ProfileAsync

        public async Task<bool> EditAdminProfileAsync(ViewAdminProfile vm)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {

                    var DataForChange = await _context.Admins
                             .Where(W => W.Adminid == vm.AdminId)
                             .FirstOrDefaultAsync();

                    if (DataForChange != null)
                    {

                        DataForChange.Email = vm.Email;
                        DataForChange.Firstname = vm.Firstname;
                        DataForChange.Lastname = vm.Lastname;
                        DataForChange.Mobile = vm.Mobile;


                        _context.Admins.Update(DataForChange);
                        _context.SaveChanges();


                        List<int> regions = await _context.Adminregions
                           .Where(r => r.Adminid == vm.AdminId)
                           .Select(req => req.Regionid)
                           .ToListAsync();

                        List<int> priceList = vm.Regionsid.Split(',').Select(int.Parse).ToList();
                        foreach (var item in priceList)
                        {
                            if (regions.Contains(item))
                            {
                                regions.Remove(item);
                            }
                            else
                            {
                                Adminregion ar = new Adminregion();
                                ar.Regionid = item;
                                ar.Adminid = (int)vm.AdminId;
                                _context.Adminregions.Update(ar);
                                await _context.SaveChangesAsync();
                                regions.Remove(item);

                            }
                        }
                        if (regions.Count > 0)
                        {
                            foreach (var item in regions)
                            {
                                Adminregion ar = await _context.Adminregions.Where(r => r.Adminid == vm.AdminId && r.Regionid == item).FirstAsync();
                                _context.Adminregions.Remove(ar);
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
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion

        #region Edit_Billing_InfoAsync
        public async Task<bool> EditBillingInfoAsync(ViewAdminProfile vm)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Admins
                        .Where(W => W.Adminid == vm.AdminId)
                        .FirstOrDefaultAsync();
                    
                    if (DataForChange != null)
                    {

                        DataForChange.Address1 = vm.Address1;
                        DataForChange.Address2 = vm.Address2;
                        DataForChange.City = vm.City;
                        DataForChange.Mobile = vm.Mobile;


                        _context.Admins.Update(DataForChange);
                        _context.SaveChanges();


                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Change_Password
        public async Task<bool> ChangePasswordAsync(string password, int AdminId)
        {
            var hasher = new PasswordHasher<string>();


            var req = await _context.Admins
                .Where(W => W.Adminid == AdminId)
                    .FirstOrDefaultAsync();
            Aspnetuser U = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Id == req.Aspnetuserid);

            if (U != null)
            {
                 U.Passwordhash = hasher.HashPassword(null, password); 
                _context.Aspnetusers.Update(U);
                _context.SaveChanges();
                return true;
            }
            return false;

        }
        #endregion

        #endregion

    }
}
