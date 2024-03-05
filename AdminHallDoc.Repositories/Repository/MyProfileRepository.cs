using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto;
using System;
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
                                            AdminId = r.Adminid,
                                            UserName = asp.Username,
                                            Password = asp.Passwordhash,
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

        #region Put_Profile
        public async Task<bool> PutProfileDetails(ViewAdminProfile v)
        {
            var req = await _context.Admins
                .Where(W => W.Adminid == v.AdminId)
                    .FirstOrDefaultAsync();
            if (req != null)
            {
                req.Firstname = v.Firstname;
                req.Lastname = v.Lastname;
                req.Email = v.Email;
                req.Status = v.Status;
                req.Mobile = v.Mobile;
                req.Address1 = v.Address1;
                req.Address2 = v.Address2;
                req.City = v.City;
                req.Zip = v.Zipcode;
                req.Altphone = v.AltMobile;
                _context.Admins.Update(req);
                await _context.SaveChangesAsync();
            }

            List<int>  regions = await _context.Adminregions
               .Where(r => r.Adminid == v.AdminId)
               .Select(req => req.Regionid)
               .ToListAsync();

            List<int> priceList = v.Regionsid.Split(',').Select(int.Parse).ToList();
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
                    ar.Adminid = (int)v.AdminId;
                    _context.Adminregions.Update(ar);
                    await _context.SaveChangesAsync();
                    regions.Remove(item);

                }
            }
            if (regions.Count > 0)
            {
                foreach (var item in regions)
                {
                   Adminregion ar =  await _context.Adminregions.Where(r => r.Adminid == v.AdminId && r.Regionid == item).FirstAsync();
                    _context.Adminregions.Remove(ar);
                    await _context.SaveChangesAsync();
                }
            }

            return true;

        }
        #endregion
    }
}
