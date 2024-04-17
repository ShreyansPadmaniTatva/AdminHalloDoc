using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Patient.Repository
{
    public class PatientDashboardRepository : IPatientDashboardRepository
    {
        #region Contractor
        private readonly ApplicationDbContext _context;
        private readonly EmailConfiguration _email;
        public PatientDashboardRepository(ApplicationDbContext context, EmailConfiguration email)
        {
            _context = context;
            _email = email;
        }
        #endregion
        
        #region DashboardData
        public List<ViewPatientDashboard> DashboardData(int UserID)
        {
            BitArray bt = new BitArray(1);
            bt.Set(0, false); 
            List<ViewPatientDashboard> result = _context.Requests
                         .Where(r => r.Userid == UserID && r.Isdeleted == bt)
                         .OrderByDescending(x => x.Createddate)
                         .Select(r => new ViewPatientDashboard
                         {
                             Requestid = r.Requestid,
                             Createddate = r.Createddate,
                             Status = r.Status,
                             FileCount = _context.Requestwisefiles.Count(f => f.Requestid == r.Requestid && f.Isdeleted == bt)
                         })
                         .ToList();
            return result;
        }
        #endregion

        #region Profile
        public ViewUserProfile Profile(int UserID)
        {
            BitArray bt = new BitArray(1);
            bt.Set(0, false);
            var UsersProfile = _context.Users
                               .Where(r => r.Userid == UserID && r.Isdeleted == bt)
                               .Select(r => new ViewUserProfile
                               {
                                   Userid = r.Userid,
                                   Firstname = r.Firstname,
                                   Lastname = r.Lastname,
                                   Mobile = r.Mobile,
                                   Email = r.Email,
                                   Street = r.Street,
                                   State = r.State,
                                   City = r.City,
                                   Zipcode = r.Zipcode,
                                   Birthdate = new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()), (int)r.Intdate) == null ? null  : new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()), (int)r.Intdate),
                               })
                               .FirstOrDefault();
            return UsersProfile;
        }
        #endregion

        #region ProfileEdit
        public async Task<bool> ProfileEdit(ViewUserProfile userprofile)
        {
            try
            {
                User userToUpdate = await _context.Users.FindAsync(userprofile.Userid);
                if (userToUpdate != null)
                {
                    userToUpdate.Firstname = userprofile.Firstname;
                    userToUpdate.Lastname = userprofile.Lastname;
                    userToUpdate.Mobile = userprofile.Mobile;
                    userToUpdate.Email = userprofile.Email;
                    userToUpdate.State = userprofile.State;
                    userToUpdate.Street = userprofile.Street;
                    userToUpdate.City = userprofile.City;
                    userToUpdate.Zipcode = userprofile.Zipcode;
                    userToUpdate.Intdate = userprofile.Birthdate.Value.Day;
                    userToUpdate.Intyear = userprofile.Birthdate.Value.Year;
                    userToUpdate.Strmonth = userprofile.Birthdate.Value.Month.ToString();
                    userToUpdate.Modifiedby = userprofile.Createdby;
                    userToUpdate.Modifieddate = DateTime.Now;
                    _context.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region Documentsinfo
        public List<ViewPatientDashboard> Documentsinfo(int Requestid)
        {
            BitArray bt = new BitArray(1);
            bt.Set(0, false);
            var result = _context.Requestwisefiles
                        .Where(r => r.Requestid == Requestid && r.Isdeleted == bt)
                        .OrderByDescending(x => x.Createddate)
                        .Select(r => new ViewPatientDashboard
                        {
                            Requestid = r.Requestid,
                            Createddate = r.Createddate,
                            Filename = r.Filename

                        })
                        .ToList();
            return result;
        }
        #endregion

        #region UploadDoc
        public async Task<bool> UploadDoc(int Requestid, IFormFile file)
        {
            try
            {
                string UploadDoc;
                if (file != null)
                {

                    UploadDoc = CM.UploadDoc(file, Requestid);

                    var requestwisefile = new Requestwisefile
                    {
                        Requestid = Requestid,
                        Filename = UploadDoc,
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(new[] { false }),
                    };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion
    }
}
