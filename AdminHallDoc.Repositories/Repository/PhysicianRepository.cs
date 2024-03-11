using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        #region PhysicianAll
        public async Task<List<Physicians>> PhysicianAll()
        {


            List<Physicians> pl = await (from r in _context.Physicians
                                         join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into aspGroup
                                         from nof in aspGroup.DefaultIfEmpty()
                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()
                                         select  new Physicians{
                                             notificationid = nof.Id,
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
                                            notification = nof.Isnotificationstopped,
                                            role = roles.Name,
                                            Status = r.Status,
                                            Email = r.Email

                                        })
                                        .ToListAsync();

            return pl;

        }
        #endregion

        #region PhysicianByRegion
        public async Task<List<Physicians>> PhysicianByRegion(int? region)
        {


            List<Physicians> pl = await (
                                        from pr in _context.Physicianregions

                                        join ph in _context.Physicians
                                         on pr.Physicianid equals ph.Physicianid into rGroup
                                        from r in rGroup.DefaultIfEmpty()

                                        join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into aspGroup
                                         from nof in aspGroup.DefaultIfEmpty()

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
                                             notification = nof.Isnotificationstopped,
                                             role = roles.Name,
                                             Status = r.Status

                                         })
                                        .ToListAsync();


            return pl;

        }
        #endregion

        #region Change_Notification_Physician

        public async Task<bool> ChangeNotificationPhysician(Dictionary<int, bool> changedValuesDict)
        {
            try
            {
                if (changedValuesDict == null)
                {
                    return false;
                }
                else
                {
                    

                    foreach (var item in changedValuesDict)
                    {
                        var ar =  _context.Physiciannotifications.Find(item.Key);
                        if (ar != null)
                        {
                            ar.Isnotificationstopped[0] = item.Value ;
                            _context.Physiciannotifications.Update(ar);
                             _context.SaveChanges();
                        }
                    }

                        return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion

        #region PhysicianAddEdit
        public async Task<bool> PhysicianAddEdit(Physicians physiciandata,string AdminId)
        {
            try
            {
                if (physiciandata.UserName!=null && physiciandata.PassWord != null)
                {
                    var Aspnetuser = new Aspnetuser();
                    var hasher = new PasswordHasher<string>();
                    Aspnetuser.Id = Guid.NewGuid().ToString();
                    Aspnetuser.Username = physiciandata.UserName;
                    Aspnetuser.Passwordhash = hasher.HashPassword(null, physiciandata.PassWord);
                    Aspnetuser.Email = physiciandata.Email;
                    Aspnetuser.CreatedDate = DateTime.Now;
                    _context.Aspnetusers.Add(Aspnetuser);
                    await _context.SaveChangesAsync();

                    var Physician = new Physician();
                    // Aspnetuser
                    Physician.Aspnetuserid = Aspnetuser.Id;
                    Physician.Firstname = physiciandata.Firstname;
                    Physician.Lastname = physiciandata.Lastname;
                    Physician.Status = physiciandata.Status;
                    Physician.Roleid = physiciandata.Roleid;
                    Physician.Email = physiciandata.Address1;
                    Physician.Mobile = physiciandata.Address1;
                    Physician.Medicallicense = physiciandata.Address1;
                    Physician.Npinumber = physiciandata.Address1;
                    Physician.Syncemailaddress = physiciandata.Address1;
                    Physician.Address1 = physiciandata.Address1;
                    Physician.Address2 = physiciandata.Address1;
                    Physician.City = physiciandata.Address1;
                    Physician.Zip = physiciandata.Zipcode;
                    Physician.Altphone = physiciandata.Altphone;
                    Physician.Businessname = physiciandata.Businessname;
                    Physician.Businesswebsite = physiciandata.Businesswebsite;
                    Physician.Createddate = DateTime.Now;
                    Physician.Createdby = AdminId;

                    _context.Physicians.Add(Physician);
                    await _context.SaveChangesAsync();

                    
                    CM.UploadProviderDoc(physiciandata.PhotoFile, Physician.Physicianid);
                    CM.UploadProviderDoc(physiciandata.SignatureFile, Physician.Physicianid);
                }

                else
                {

                }
              

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        #endregion

    }
}
