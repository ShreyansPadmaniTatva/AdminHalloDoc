using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminHalloDoc.Entities.ViewModel.AdminViewModel.ViewAdminProfile;
using static AdminHalloDoc.Entities.ViewModel.Constant;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using Org.BouncyCastle.Utilities.Net;
using System.Net.Sockets;
using IPAddress = System.Net.IPAddress;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Diagnostics;
using System.Web.Helpers;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class PhysicianRepository : IPhysicianRepository
    {
        #region Constructor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public PhysicianRepository(ApplicationDbContext context, EmailConfiguration emailConfig, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailConfig = emailConfig;
            this.httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Find_Location_Physician
        public async Task<List<PhysicianLocation>> FindPhysicianLocation()
        {


            List<PhysicianLocation> pl = await _context.Physicianlocations
                                 .Join(
                                     _context.Physicians,
                                     pl => pl.Physicianid,
                                     r => r.Physicianid,
                                     (pl, r) => new { PhysicianLocation = pl, Physician = r }
                                 )
                                 .OrderByDescending(x => x.Physician.Firstname)
                                 .Select(result => new PhysicianLocation
                                 {
                                     locationid = result.PhysicianLocation.Locationid,
                                     longitude = result.PhysicianLocation.Longitude,
                                     latitude = result.PhysicianLocation.Latitude,
                                     physicianname = result.PhysicianLocation.Physicianname,
                                    photo = result.Physician.Photo,
                                    physicianid = result.Physician.Physicianid,
                                     createddate = result.PhysicianLocation.Createddate,
                                     address = result.PhysicianLocation.Address

                                 })
                                 .ToListAsync();

            return pl;

        }
        #endregion

        #region PhysicianAll
        public async Task<List<Physicians>> PhysicianAll()
        {

            var currentDateTime = DateTime.Now;
            TimeOnly currentTimeOfDay = TimeOnly.FromDateTime(DateTime.Now);
            List<Physicians> pl = await (from r in _context.Physicians
                                         join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into aspGroup
                                         from nof in aspGroup.DefaultIfEmpty()
                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()
                                         where r.Isdeleted == new BitArray(1)
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
                                            Email = r.Email,
                                           onCallStatus = (from s in _context.Shifts
                                                          join sd in _context.Shiftdetails on s.Shiftid equals sd.Shiftid into sdGroup
                                                          where s.Physicianid == r.Physicianid
                                                          from shiftDetail in sdGroup.Where(sd => sd.Shiftdate.Date == currentDateTime.Date &&
                                                                                                    s.Physicianid == r.Physicianid &&
                                                                                                   sd.Starttime <= currentTimeOfDay &&
                                                                                                   currentTimeOfDay <= sd.Endtime)
                                                          select shiftDetail).FirstOrDefault() == null ? 0:1 

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
                                             Status = r.Status,
                                             

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
                        var ar =  _context.Physiciannotifications.Where(r => r.Physicianid == item.Key).FirstOrDefault();
                        if (ar != null)
                        {
                            ar.Isnotificationstopped[0] = item.Value ;
                            _context.Physiciannotifications.Update(ar);
                             _context.SaveChanges();
                        }
                        else
                        {
                            Physiciannotification pn = new Physiciannotification();
                            pn.Physicianid = item.Key;
                            pn.Isnotificationstopped = new BitArray(1);
                            pn.Isnotificationstopped[0] = item.Value;
                            _context.Physiciannotifications.Add(pn);
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

        #region Physician_Add
        public async Task<bool> PhysicianAddEdit(Physicians physiciandata,string AdminId)
        {
            try
            {
                if (physiciandata.UserName!=null && physiciandata.PassWord != null)
                {
                    // ASP_User
                    var Aspnetuser = new Aspnetuser();
                    var hasher = new PasswordHasher<string>();
                    Aspnetuser.Id = Guid.NewGuid().ToString();
                    Aspnetuser.Username = physiciandata.UserName;
                    Aspnetuser.Passwordhash = hasher.HashPassword(null, physiciandata.PassWord);
                    Aspnetuser.Email = physiciandata.Email;
                    Aspnetuser.CreatedDate = DateTime.Now;
                    _context.Aspnetusers.Add(Aspnetuser);
                     _context.SaveChanges();

                    //aspnet_user_roles
                    var aspnetuserroles = new Aspnetuserrole();
                    aspnetuserroles.Userid = Aspnetuser.Id;
                    aspnetuserroles.Roleid = "Provider";
                    _context.Aspnetuserroles.Add(aspnetuserroles);
                    _context.SaveChanges();

                    // Physician
                    var Physician = new Physician();
                    Physician.Aspnetuserid = Aspnetuser.Id;
                    Physician.Firstname = physiciandata.Firstname;
                    Physician.Lastname = physiciandata.Lastname;
                    Physician.Status = 2;
                    Physician.Roleid = physiciandata.Roleid;
                    Physician.Email = physiciandata.Email;
                    Physician.Mobile = physiciandata.Mobile;
                    Physician.Medicallicense = physiciandata.Medicallicense;
                    Physician.Npinumber = physiciandata.Npinumber;
                    Physician.Syncemailaddress = physiciandata.Syncemailaddress;
                    Physician.Address1 = physiciandata.Address1;
                    Physician.Address2 = physiciandata.Address2;
                    Physician.City = physiciandata.City;
                    Physician.Zip = physiciandata.Zipcode;
                    Physician.Altphone = physiciandata.Altphone;
                    Physician.Businessname = physiciandata.Businessname;
                    Physician.Businesswebsite = physiciandata.Businesswebsite;
                    Physician.Createddate = DateTime.Now;
                    Physician.Createdby = AdminId;
                    Physician.Regionid = physiciandata.Regionid;

                    Physician.Isagreementdoc = new BitArray(1);
                    Physician.Isbackgrounddoc = new BitArray(1);
                    Physician.Isnondisclosuredoc = new BitArray(1);
                    Physician.Islicensedoc = new BitArray(1);
                    Physician.Istrainingdoc = new BitArray(1);
                    Physician.Isdeleted = new BitArray(1);

                    Physician.Isagreementdoc[0] = physiciandata.Isagreementdoc;
                    Physician.Isbackgrounddoc[0] = physiciandata.Isbackgrounddoc;
                    Physician.Isnondisclosuredoc[0] = physiciandata.Isnondisclosuredoc;
                    Physician.Islicensedoc[0] = physiciandata.Islicensedoc;
                    Physician.Istrainingdoc[0] = physiciandata.Istrainingdoc;
                    Physician.Isdeleted[0] = false;
                    Physician.Adminnotes = physiciandata.Adminnotes;


                    Physician.Photo = physiciandata.PhotoFile != null ? Physician.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-Photo."+ Path.GetExtension(physiciandata.PhotoFile.FileName).Trim('.') : null; 
                    Physician.Signature = physiciandata.SignatureFile != null ? Physician.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-Signature.png" : null;

                   

                    _context.Physicians.Add(Physician);
                     _context.SaveChanges();

                    CM.UploadProviderDoc(physiciandata.Agreementdoc, Physician.Physicianid, "Agreementdoc.pdf");
                    CM.UploadProviderDoc(physiciandata.BackGrounddoc, Physician.Physicianid, "BackGrounddoc.pdf");
                    CM.UploadProviderDoc(physiciandata.NonDisclosuredoc, Physician.Physicianid, "NonDisclosuredoc.pdf");
                    CM.UploadProviderDoc(physiciandata.Licensedoc, Physician.Physicianid, "Agreementdoc.pdf");
                    CM.UploadProviderDoc(physiciandata.Trainingdoc, Physician.Physicianid, "Trainingdoc.pdf");

                    CM.UploadProviderDoc(physiciandata.SignatureFile, Physician.Physicianid, Physician.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-Signature.png");
                    CM.UploadProviderDoc(physiciandata.PhotoFile, Physician.Physicianid, Physician.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-Photo."+ Path.GetExtension(physiciandata.PhotoFile.FileName).Trim('.'));

                    // Physician_region
                    List<int> priceList = physiciandata.Regionsid.Split(',').Select(int.Parse).ToList();
                    foreach (var item in priceList)
                    {
                            Physicianregion ar = new Physicianregion();
                            ar.Regionid = item;
                            ar.Physicianid = (int)Physician.Physicianid;
                            _context.Physicianregions.Add(ar);
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

        #region GetPhysicianById
        public async Task<Physicians> GetPhysicianById(int id)
        {


                 Physicians pl = await (from r in _context.Physicians
                                        join Aspnetuser in _context.Aspnetusers
                                        on r.Aspnetuserid equals Aspnetuser.Id into aspGroup
                                        from asp in aspGroup.DefaultIfEmpty()
                                        join Notifications in _context.Physiciannotifications
                                         on r.Physicianid equals Notifications.Physicianid into PhyNGroup
                                         from nof in PhyNGroup.DefaultIfEmpty()
                                         join role in _context.Roles
                                         on r.Roleid equals role.Roleid into roleGroup
                                         from roles in roleGroup.DefaultIfEmpty()
                                         where r.Physicianid == id
                                         select new Physicians
                                         {
                                             UserName = asp.Username,
                                             Roleid = r.Roleid,
                                             Status = r.Status,
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
                                             Firstname = r.Firstname,
                                             Lastname = r.Lastname,
                                             notification = nof.Isnotificationstopped,
                                             role = roles.Name,
                                             Email = r.Email,
                                             Photo = r.Photo,
                                             Signature = r.Signature,
                                             Isagreementdoc = r.Isagreementdoc[0],
                                             Isnondisclosuredoc = r.Isnondisclosuredoc[0],
                                             Isbackgrounddoc = r.Isbackgrounddoc[0],
                                             Islicensedoc = r.Islicensedoc[0],
                                             Istrainingdoc = r.Istrainingdoc[0],
                                             Medicallicense = r.Medicallicense,
                                            Npinumber = r.Npinumber,
                                            Syncemailaddress = r.Syncemailaddress,
                                            Zipcode = r.Zip,
                                            Regionid = r.Regionid

                                        })
                                        .FirstOrDefaultAsync();

            List<AdminHalloDoc.Entities.ViewModel.AdminViewModel.Physicians.Regions> regions = new List<AdminHalloDoc.Entities.ViewModel.AdminViewModel.Physicians.Regions>();

            regions =  _context.Physicianregions
                  .Where(r => r.Physicianid == pl.Physicianid)
                  .Select(req => new AdminHalloDoc.Entities.ViewModel.AdminViewModel.Physicians.Regions()
                  {
                      regionid = req.Regionid
                  })
                  .ToList();

            pl.Regionids = regions;

            return pl;

        }
        #endregion

        #region Put_Profile_Physician

        #region SavePhysicianInfo
        public async Task<bool> SavePhysicianInfo(Physicians vm)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == vm.Physicianid)
                        .FirstOrDefaultAsync();
                    Aspnetuser U = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Id == DataForChange.Aspnetuserid);

                    if (DataForChange != null)
                    {

                        U.Username = vm.UserName;
                        DataForChange.Status = vm.Status;
                        DataForChange.Roleid = vm.Roleid;


                        _context.Physicians.Update(DataForChange);
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

        #region Change_Password
        public async Task<bool> ChangePasswordAsync(string password, int Physicianid)
        {
            var hasher = new PasswordHasher<string>();


            var req = await _context.Physicians
                .Where(W => W.Physicianid == Physicianid)
                    .FirstOrDefaultAsync();


            if (req != null)
            {
            var U = await _context.Aspnetusers.Where(m => m.Id == req.Aspnetuserid).FirstOrDefaultAsync();
                U.Passwordhash = hasher.HashPassword(null, password);
                _context.Aspnetusers.Update(U);
                _context.SaveChanges();
                return true;
            }
            return false;

        }
        #endregion

        #region EditAdminInfo
        public async Task<bool> EditAdminInfo(Physicians vm)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == vm.Physicianid)
                        .FirstOrDefaultAsync();


                    if (DataForChange != null)
                    {


                        DataForChange.Firstname = vm.Firstname;
                        DataForChange.Lastname = vm.Lastname;
                        DataForChange.Email = vm.Email;
                        DataForChange.Mobile = vm.Mobile;
                        DataForChange.Medicallicense = vm.Medicallicense;
                        DataForChange.Npinumber = vm.Npinumber;
                        DataForChange.Syncemailaddress = vm.Syncemailaddress;



                        _context.Physicians.Update(DataForChange);
                        _context.SaveChanges();

                        List<int> priceList = vm.Regionsid.Split(',').Select(int.Parse).ToList();




                        foreach (var dataitem2 in priceList)
                        {
                            var data = _context.Physicianregions.FirstOrDefault(e => e.Physicianid == vm.Physicianid && e.Regionid == dataitem2);
                            if (data != null)
                            {

                            }
                            else
                            {
                                Physicianregion adr = new Physicianregion
                                {
                                    Physicianid = DataForChange.Physicianid,
                                    Regionid = dataitem2
                                };

                                _context.Physicianregions.Add(adr);
                                _context.SaveChanges();
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

        #region DeletePhysician
        public async Task<bool> DeletePhysician(int PhysicianID, string AdminID)
        {
            try
            {
                BitArray bt = new BitArray(1);
                bt.Set(0, true);
                if (PhysicianID == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == PhysicianID)
                        .FirstOrDefaultAsync();


                    if (DataForChange != null)
                    {


                        DataForChange.Isdeleted = bt;
                        DataForChange.Isdeleted[0] = true;
                        DataForChange.Modifieddate = DateTime.Now;
                        DataForChange.Modifiedby = AdminID;
                        _context.Physicians.Update(DataForChange);



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

        #region EditMailBilling
        public async Task<bool> EditMailBilling(Physicians vm, string AdminId)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == vm.Physicianid)
                        .FirstOrDefaultAsync();


                    if (DataForChange != null)
                    {


                        DataForChange.Address1 = vm.Address1;
                        DataForChange.City = vm.City;
                        DataForChange.Regionid = vm.Regionid;
                        DataForChange.Zip = vm.Zipcode;
                        DataForChange.Altphone = vm.Altphone;
                        DataForChange.Modifiedby = AdminId;
                        DataForChange.Modifieddate = DateTime.Now;




                        _context.Physicians.Update(DataForChange);

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

        #region EditProviderProfile
        public async Task<bool> EditProviderProfile(Physicians vm, string AdminId)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == vm.Physicianid)
                        .FirstOrDefaultAsync();


                    if (DataForChange != null)
                    {
                        if (vm.PhotoFile != null)
                        {
                            DataForChange.Photo = vm.PhotoFile != null ? vm.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmm") + "-Photo." + Path.GetExtension(vm.PhotoFile.FileName).Trim('.') : null;
                            CM.UploadProviderDoc(vm.PhotoFile, (int)vm.Physicianid, vm.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmm") + "-Photo." + Path.GetExtension(vm.PhotoFile.FileName).Trim('.'));

                        }
                        if (vm.SignatureFile != null)
                        {
                            DataForChange.Signature = vm.SignatureFile != null ? vm.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmm") + "-Signature.png" : null;
                            CM.UploadProviderDoc(vm.SignatureFile, (int)vm.Physicianid, vm.Firstname + "-" + DateTime.Now.ToString("yyyyMMddhhmm") + "-Signature.png");
                        }



                        DataForChange.Businessname = vm.Businessname;
                        DataForChange.Businesswebsite = vm.Businesswebsite;
                        DataForChange.Modifiedby = AdminId;
                        DataForChange.Adminnotes = vm.Adminnotes;
                        DataForChange.Modifieddate = DateTime.Now;
                        _context.Physicians.Update(DataForChange);

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

        #region EditProviderOnbording
        public async Task<bool> EditProviderOnbording(Physicians vm, string AdminId)
        {
            try
            {
                if (vm == null)
                {
                    return false;
                }
                else
                {
                    var DataForChange = await _context.Physicians
                        .Where(W => W.Physicianid == vm.Physicianid)
                        .FirstOrDefaultAsync();


                    if (DataForChange != null)
                    {


                        CM.UploadProviderDoc(vm.Agreementdoc, (int)vm.Physicianid, "Agreementdoc.pdf");
                        CM.UploadProviderDoc(vm.BackGrounddoc, (int)vm.Physicianid, "BackGrounddoc.pdf");
                        CM.UploadProviderDoc(vm.NonDisclosuredoc, (int)vm.Physicianid, "NonDisclosuredoc.pdf");
                        CM.UploadProviderDoc(vm.Licensedoc, (int)vm.Physicianid, "Agreementdoc.pdf");
                        CM.UploadProviderDoc(vm.Trainingdoc, (int)vm.Physicianid, "Trainingdoc.pdf");

                        DataForChange.Isagreementdoc = new BitArray(1);
                        DataForChange.Isbackgrounddoc = new BitArray(1);
                        DataForChange.Isnondisclosuredoc = new BitArray(1);
                        DataForChange.Islicensedoc = new BitArray(1);
                        DataForChange.Istrainingdoc = new BitArray(1);

                        DataForChange.Isagreementdoc[0] = vm.Isagreementdoc;
                        DataForChange.Isbackgrounddoc[0] = vm.Isbackgrounddoc;
                        DataForChange.Isnondisclosuredoc[0] = vm.Isnondisclosuredoc;
                        DataForChange.Islicensedoc[0] = vm.Islicensedoc;
                        DataForChange.Istrainingdoc[0] = vm.Istrainingdoc;
                        DataForChange.Modifiedby = AdminId;
                        DataForChange.Modifieddate = DateTime.Now;

                        _context.Physicians.Update(DataForChange);

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

        #endregion

        #region PostLocation

        public async Task<bool> GetLocation(int PhysicianId)
        {
            try
            {
                // Retrieve physician location data
                Physicianlocation dataForChange = await _context.Physicianlocations
                    .Where(w => w.Physicianid == PhysicianId)
                    .FirstOrDefaultAsync();

                if (dataForChange == null)
                {
                    // Create a new Physicianlocation entity
                    Physicianlocation physicianLocation = new Physicianlocation
                    {
                        Physicianid = PhysicianId,
                        Createddate = DateTime.Now,
                        Physicianname = await _context.Physicians.Where(r => r.Physicianid == PhysicianId).Select(r => r.Firstname+" " + r.Lastname).FirstOrDefaultAsync()
                    };

                    var location = await GetCurrentLocationAsync();

                    physicianLocation.Latitude = location.latitude;
                            physicianLocation.Longitude = location.longitude;
                            physicianLocation.Address = await ReverseGeocodeAsync(location.latitude, location.longitude);

                            // Add new Physicianlocation entity to the context
                            await _context.Physicianlocations.AddAsync(physicianLocation);
                        
                    
                }
                else
                {
                    // Update existing Physicianlocation entity
                    dataForChange.Createddate = DateTime.Now;
                    var location = await GetCurrentLocationAsync();
                    dataForChange.Latitude = location.latitude;
                    dataForChange.Longitude = location.longitude;
                    dataForChange.Address = await ReverseGeocodeAsync(location.latitude, location.longitude);
                     _context.Physicianlocations.Update(dataForChange);
                }

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Optionally handle or log the exception
                return false;
            }
        }

        #region ReverseGeocodeAsync

        public async Task<string> ReverseGeocodeAsync(decimal latitude, decimal longitude)
        {
            // Construct the request URL
            string apiUrl = $"https://api.geoapify.com/v1/geocode/reverse?lat={latitude}&lon={longitude}&apiKey=5298b1dffb174ceb9d9d36e999e692aa";

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        dynamic jsonData = JObject.Parse(responseBody);
                        // Deserialize JSON into dynamic object

                        // Extract address information
                        string name = jsonData.features[0].properties.name;
                        string country = jsonData.features[0].properties.country;
                        string state = jsonData.features[0].properties.state;
                        string county = jsonData.features[0].properties.county;
                        string city = jsonData.features[0].properties.city;
                        string postcode = jsonData.features[0].properties.postcode;
                        string suburb = jsonData.features[0].properties.suburb;
                        string street = jsonData.features[0].properties.street;
                        string housenumber = jsonData.features[0].properties.housenumber;

                        

                        // Format the address
                        string address = $"{name}, {city}, {suburb}, {state}, {country} - {postcode}";


                        return address;
                    }
                    else
                    {
                        // Return null or throw an exception indicating failure
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Return null or throw an exception indicating failure
                    return null;
                }
            }
        }
        #endregion

        #region GetCurrentLocationAsync
        public async Task<(decimal latitude, decimal longitude)> GetCurrentLocationAsync()
        {
            Decimal latitude = 0;
            Decimal longitude = 0;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiKey = "3a5759a23c5b00";
                    string apiUrl = $"https://ipinfo.io?token={apiKey}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic ipInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                        string[] coordinates = ipInfo.loc.ToString().Split(',');
                         latitude = Convert.ToDecimal(coordinates[0]);
                         longitude = Convert.ToDecimal(coordinates[1]);

                       
                    }


                    return (latitude, longitude);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error fetching location: {ex.Message}");
                return (0, 0); // Default values
            }

            
            return (0, 0);
        }
        #endregion

        #endregion

        #region check_email_exist_For_Provider
        /// <summary>
        /// when new provider add  then check that mail is exit or not
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public List<Physician> isProviderEmailExist(string Email)
        {
            List<Physician> data = _context.Physicians.Where(e => e.Email.ToLower().Equals(Email.ToLower())).ToList();
            return data;
        }
        #endregion

        #region check_email_exist
        /// <summary>
        /// when new provider add  then check that mail is exit or not
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public List<AdminHalloDoc.Entities.Models.Admin> isAdminEmailExist(string Email)
        {
            List<AdminHalloDoc.Entities.Models.Admin> data = _context.Admins.Where(e => e.Email.ToLower().Equals(Email.ToLower())).ToList();
            return data;
        }
        #endregion

    }
}
