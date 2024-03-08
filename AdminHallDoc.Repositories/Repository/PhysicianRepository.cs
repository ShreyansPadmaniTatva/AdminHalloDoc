using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.AdminViewModel.ViewAdminProfile;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class PhysicianRepository : IPhysicianRepository
    {
        #region Constructor
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public PhysicianRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region Find_Location_Physician
        public async Task<List<PhysicianLocation>> FindPhysicianLocation()
        {
            

            List<PhysicianLocation> pl =await _context.Physicianlocations
                                    .OrderByDescending(x => x.Physicianname)
                        .Select(r => new PhysicianLocation
                        {
                            locationid = r.Locationid,
                            longitude = r.Longitude,
                            latitude = r.Latitude,
                            physicianname = r.Physicianname

                        }).ToListAsync();
            return pl;

        }
        #endregion

        #region Find_Location_Physician
        public async Task<List<Physicians>> PhysicianAll()
        {


            List<Physicians> pl = await (from r in _context.Physicians
                                         join Aspnetuser in _context.Physiciannotifications
                                         on r.Physicianid equals Aspnetuser.Physicianid into aspGroup
                                         from asp in aspGroup.DefaultIfEmpty()
                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()
                                         select  new Physicians{
                                            Createddate = r.Createddate,
                                            Physicianid = r.Physicianid,
                                            Address1 = r.Address1,
                                            Address2 = r.Address2,
                                            Adminnotes = r.Adminnotes,
                                            Altphone = r.Altphone,
                                            Businessname = r.Businessname,
                                            Businesswebsite = r.Businesswebsite,
                                            City = r.City,
                                            Firstname = r.Firstname, Lastname = r.Lastname,
                                            notification = asp.Isnotificationstopped,
                                            role = roles.Name,
                                            Status = r.Status,
                                            Email = r.Email

                                        })
                                        .ToListAsync();

            return pl;

        }
        #endregion

        #region Find_Location_Physician
        public async Task<List<Physicians>> PhysicianByRegion(int? region)
        {


            List<Physicians> pl = await (
                                        from pr in _context.Physicianregions

                                        join ph in _context.Physicians
                                         on pr.Physicianid equals ph.Physicianid into rGroup
                                        from r in rGroup.DefaultIfEmpty()

                                        join Aspnetuser in _context.Physiciannotifications
                                         on r.Physicianid equals Aspnetuser.Physicianid into aspGroup
                                         from asp in aspGroup.DefaultIfEmpty()

                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()

                                         where pr.Regionid == region
                                         select new Physicians
                                         {
                                             Createddate = r.Createddate,
                                             Physicianid = r.Physicianid,
                                             Address1 = r.Address1,
                                             Address2 = r.Address2,
                                             Adminnotes = r.Adminnotes,
                                             Altphone = r.Altphone,
                                             Businessname = r.Businessname,
                                             Businesswebsite = r.Businesswebsite,
                                             City = r.City,
                                             Firstname = r.Firstname,
                                             Lastname = r.Lastname,
                                             notification = asp.Isnotificationstopped,
                                             role = roles.Name,
                                             Status = r.Status

                                         })
                                        .ToListAsync();


            return pl;

        }
        #endregion
    }
}
