﻿using AdminHalloDoc.Entities.Data;
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
                     _context.SaveChanges();

                    // Physician
                    var Physician = new Physician();
                    Physician.Aspnetuserid = Aspnetuser.Id;
                    Physician.Firstname = physiciandata.Firstname;
                    Physician.Lastname = physiciandata.Lastname;
                    Physician.Status = physiciandata.Status;
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

                    Physician.Isagreementdoc[0] = physiciandata.Isagreementdoc;
                    Physician.Isbackgrounddoc[0] = physiciandata.Isbackgrounddoc;
                    Physician.Isnondisclosuredoc[0] = physiciandata.Isnondisclosuredoc;
                    Physician.Islicensedoc[0] = physiciandata.Islicensedoc;
                    Physician.Istrainingdoc[0] = physiciandata.Istrainingdoc;
                    Physician.Adminnotes = physiciandata.Adminnotes;


                    Physician.Photo = physiciandata.PhotoFile != null ? Physician.Firstname + " - " + DateTime.Now.ToString("yyyyMMddhhmmss") + " - Photo."+ Path.GetExtension(physiciandata.PhotoFile.FileName).Trim('.') : null; 
                    Physician.Signature = physiciandata.SignatureFile != null ? Physician.Firstname + " - " + DateTime.Now.ToString("yyyyMMddhhmmss") + " - Signature.png" : null;

                   

                    _context.Physicians.Add(Physician);
                     _context.SaveChanges();

                    CM.UploadProviderDoc(physiciandata.Agreementdoc, Physician.Physicianid, "Agreementdoc.pdf");
                    CM.UploadProviderDoc(physiciandata.BackGrounddoc, Physician.Physicianid, "BackGrounddoc.pdf");
                    CM.UploadProviderDoc(physiciandata.NonDisclosuredoc, Physician.Physicianid, "NonDisclosuredoc.pdf");
                    CM.UploadProviderDoc(physiciandata.Licensedoc, Physician.Physicianid, "Agreementdoc.pdf");
                    CM.UploadProviderDoc(physiciandata.Trainingdoc, Physician.Physicianid, "Trainingdoc.pdf");

                    CM.UploadProviderDoc(physiciandata.SignatureFile, Physician.Physicianid, Physician.Firstname + " - " + DateTime.Now.ToString("yyyyMMddhhmmss") + " - Signature.png");
                    CM.UploadProviderDoc(physiciandata.PhotoFile, Physician.Physicianid, Physician.Firstname + " - " + DateTime.Now.ToString("yyyyMMddhhmmss") + " - Photo."+ Path.GetExtension(physiciandata.PhotoFile.FileName).Trim('.'));

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
                                             Istrainingdoc = r.Istrainingdoc[0]

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

        #region Put_Profile

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

        //#region Edit_Admin_ProfileAsync

        //public async Task<bool> EditAdminProfileAsync(ViewAdminProfile vm)
        //{
        //    try
        //    {
        //        if (vm == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {

        //            var DataForChange = await _context.Admins
        //                     .Where(W => W.Adminid == vm.AdminId)
        //                     .FirstOrDefaultAsync();

        //            if (DataForChange != null)
        //            {

        //                DataForChange.Email = vm.Email;
        //                DataForChange.Firstname = vm.Firstname;
        //                DataForChange.Lastname = vm.Lastname;
        //                DataForChange.Mobile = vm.Mobile;


        //                _context.Admins.Update(DataForChange);
        //                _context.SaveChanges();


        //                List<int> regions = await _context.Adminregions
        //                   .Where(r => r.Adminid == vm.AdminId)
        //                   .Select(req => req.Regionid)
        //                   .ToListAsync();

        //                List<int> priceList = vm.Regionsid.Split(',').Select(int.Parse).ToList();
        //                foreach (var item in priceList)
        //                {
        //                    if (regions.Contains(item))
        //                    {
        //                        regions.Remove(item);
        //                    }
        //                    else
        //                    {
        //                        Adminregion ar = new Adminregion();
        //                        ar.Regionid = item;
        //                        ar.Adminid = (int)vm.AdminId;
        //                        _context.Adminregions.Update(ar);
        //                        await _context.SaveChangesAsync();
        //                        regions.Remove(item);

        //                    }
        //                }
        //                if (regions.Count > 0)
        //                {
        //                    foreach (var item in regions)
        //                    {
        //                        Adminregion ar = await _context.Adminregions.Where(r => r.Adminid == vm.AdminId && r.Regionid == item).FirstAsync();
        //                        _context.Adminregions.Remove(ar);
        //                        await _context.SaveChangesAsync();
        //                    }
        //                }

        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        //#endregion

        //#region Edit_Billing_InfoAsync
        //public async Task<bool> EditBillingInfoAsync(ViewAdminProfile vm)
        //{
        //    try
        //    {
        //        if (vm == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            var DataForChange = await _context.Admins
        //                .Where(W => W.Adminid == vm.AdminId)
        //                .FirstOrDefaultAsync();

        //            if (DataForChange != null)
        //            {

        //                DataForChange.Address1 = vm.Address1;
        //                DataForChange.Address2 = vm.Address2;
        //                DataForChange.City = vm.City;
        //                DataForChange.Mobile = vm.Mobile;


        //                _context.Admins.Update(DataForChange);
        //                _context.SaveChanges();


        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //#endregion

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

        #endregion


    }
}
