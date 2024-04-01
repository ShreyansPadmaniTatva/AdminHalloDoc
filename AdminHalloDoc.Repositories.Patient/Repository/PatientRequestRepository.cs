using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Patient.Repository
{
    public class PatientRequestRepository : IPatientRequestRepository
    {
        #region Contractor
        private readonly ApplicationDbContext _context;
        private readonly EmailConfiguration _email;
        public PatientRequestRepository(ApplicationDbContext context, EmailConfiguration email)
        {
            _context = context;
            _email = email;
        }
        #endregion

        #region PatientCreateRequest
        public async Task<bool> PatientCreateRequest(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            try
            {
                var Aspnetuser = new Aspnetuser();
                var User = new User();
                var Request = new Request();
                var Requestclient = new Requestclient();

                if (viewpatientcreaterequest.PassWord != null)
                {
                    // Aspnetuser
                    var hasher = new PasswordHasher<string>();
                    Aspnetuser.Id = Guid.NewGuid().ToString();
                    Aspnetuser.Username = viewpatientcreaterequest.Email;
                    Aspnetuser.Passwordhash = hasher.HashPassword(null, viewpatientcreaterequest.PassWord);
                    Aspnetuser.Email = viewpatientcreaterequest.Email;
                    Aspnetuser.CreatedDate = DateTime.Now;
                    _context.Aspnetusers.Add(Aspnetuser);
                    await _context.SaveChangesAsync();

                    User.Aspnetuserid = Aspnetuser.Id;
                    User.Firstname = viewpatientcreaterequest.FirstName;
                    User.Lastname = viewpatientcreaterequest.LastName;
                    User.Email = viewpatientcreaterequest.Email;
                    User.Createdby = Aspnetuser.Id;
                    User.Createddate = DateTime.Now;
                    User.Intdate = viewpatientcreaterequest.BirthDate.Day;
                    User.Intyear = viewpatientcreaterequest.BirthDate.Year;
                    User.Strmonth = viewpatientcreaterequest.BirthDate.Month.ToString();
                    User.Isdeleted = new BitArray(1);
                    User.Isdeleted[0] = false;
                    _context.Users.Add(User);
                    await _context.SaveChangesAsync();

                    Request.Userid = User.Userid;
                }
                else
                {
                    Request.Userid = viewpatientcreaterequest.UserId;
                }

                Request.Requesttypeid = 2;

                Request.Firstname = viewpatientcreaterequest.FirstName;
                Request.Lastname = viewpatientcreaterequest.LastName;
                Request.Email = viewpatientcreaterequest.Email;
                Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Createddate = DateTime.Now;
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewpatientcreaterequest.FirstName;
                Requestclient.Address = viewpatientcreaterequest.Street;
                Requestclient.Lastname = viewpatientcreaterequest.LastName;
                Requestclient.Email = viewpatientcreaterequest.Email;
                Requestclient.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Requestclient.Street = viewpatientcreaterequest.Street;
                Requestclient.City = viewpatientcreaterequest.City;
                Requestclient.State = viewpatientcreaterequest.State;
                Requestclient.Intdate = viewpatientcreaterequest.BirthDate.Day;
                Requestclient.Intyear = viewpatientcreaterequest.BirthDate.Year;
                Requestclient.Strmonth = viewpatientcreaterequest.BirthDate.Month.ToString();
                Requestclient.Address = viewpatientcreaterequest.Street + "," + viewpatientcreaterequest.City + "," + viewpatientcreaterequest.State + "," + viewpatientcreaterequest.ZipCode;
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();


                viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewpatientcreaterequest.UploadImage,
                    Createddate = DateTime.Now,
                    Isdeleted = new BitArray(1)
            };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

    }
}
