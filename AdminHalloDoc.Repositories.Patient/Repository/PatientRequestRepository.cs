using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailConfiguration _email;
        private readonly IRequestRepository _requestRepository;
        public PatientRequestRepository(ApplicationDbContext context, EmailConfiguration email, IHttpContextAccessor httpContextAccessor, IRequestRepository requestRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
            _email = email;
            _requestRepository = requestRepository;
        }
        #endregion

        #region Crate_ConFirmationNumber
        /// <summary>
        /// Create Unique Confirmation Number with State LastNAme FirstName  The first 2 characters will represent the
        ///   region abbreviation, then next 4 numbers will represent the date
        ///   of created date, then next 2 characters will represent first 2
        ///   characters of last-name, then next 2 characters will represent
        ///   first 2 characters of first-name, then next 4 digits is representing
        ///   how many requests are done in same day.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="lastname"></param>
        /// <param name="firstname"></param>
        /// <returns></returns>
        public int GetCountOfTodayRequests()
        {
            var currentDate = DateTime.Now.Date;

            return _context.Requests.Where(u => u.Createddate.Date == currentDate).Count();
        }

        public string GetConfirmationNumber(string state, string lastname, string firstname)
        {
            state = (state.Length >= 2) ? state.Substring(0, 2).ToUpperInvariant() : state.PadRight(2, 'X');
            lastname = (lastname.Length >= 2) ? lastname.Substring(0, 2).ToUpperInvariant() : lastname.PadRight(2, 'X');
            firstname = (firstname.Length >= 2) ? firstname.Substring(0, 2).ToUpperInvariant() : firstname.PadRight(2, 'X');


            string Region = state.Substring(0, 2).ToUpperInvariant();

            string NameAbbr = lastname.Substring(0, 2).ToUpperInvariant() + firstname.Substring(0, 2).ToUpperInvariant();

            DateTime requestDateTime = DateTime.Now;

            string datePart = requestDateTime.ToString("ddMMyy");

            int requestsCount = GetCountOfTodayRequests() + 1;

            string newRequestCount = requestsCount.ToString("D4");

            string ConfirmationNumber = Region + datePart + NameAbbr + newRequestCount;

            return ConfirmationNumber;

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

                    //aspnet_user_roles
                    var aspnetuserroles = new Aspnetuserrole();
                    aspnetuserroles.Userid = Aspnetuser.Id;
                    aspnetuserroles.Roleid = "Patient";
                    _context.Aspnetuserroles.Add(aspnetuserroles);
                    _context.SaveChanges();

                    //User Table
                    User.Aspnetuserid = Aspnetuser.Id;
                    User.Firstname = viewpatientcreaterequest.FirstName;
                    User.Lastname = viewpatientcreaterequest.LastName;
                    User.Mobile = viewpatientcreaterequest.PhoneNumber;
                    User.City = viewpatientcreaterequest.City;
                    User.Regionid = viewpatientcreaterequest.RegionId;
                    User.Zipcode = viewpatientcreaterequest.ZipCode;
                    User.State = viewpatientcreaterequest.State;
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

                //Request
                Request.Requesttypeid = 2;
                Request.Confirmationnumber = GetConfirmationNumber(viewpatientcreaterequest.State, viewpatientcreaterequest.FirstName, viewpatientcreaterequest.LastName);
                Request.Firstname = viewpatientcreaterequest.FirstName;
                Request.Lastname = viewpatientcreaterequest.LastName;
                Request.Email = viewpatientcreaterequest.Email;
                Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Createddate = DateTime.Now;
                Request.Status = 1;
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();

                //Request Client
                Requestclient.Requestid = Request.Requestid;
                Requestclient.Location = viewpatientcreaterequest.Symptoms;
                Requestclient.Firstname = viewpatientcreaterequest.FirstName;
                Requestclient.Address = viewpatientcreaterequest.Street;
                Requestclient.Lastname = viewpatientcreaterequest.LastName;
                Requestclient.Email = viewpatientcreaterequest.Email;
                Requestclient.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Requestclient.Street = viewpatientcreaterequest.Street;
                Requestclient.Notes = viewpatientcreaterequest.Symptoms;
                Requestclient.City = viewpatientcreaterequest.City;
                Requestclient.Regionid = viewpatientcreaterequest.RegionId;
                Requestclient.Zipcode = viewpatientcreaterequest.ZipCode;

                Requestclient.State = viewpatientcreaterequest.State;
                Requestclient.Intdate = viewpatientcreaterequest.BirthDate.Day;
                Requestclient.Intyear = viewpatientcreaterequest.BirthDate.Year;
                Requestclient.Strmonth = viewpatientcreaterequest.BirthDate.Month.ToString();
                Requestclient.Address = viewpatientcreaterequest.Street + "," + viewpatientcreaterequest.City + "," + viewpatientcreaterequest.State + "," + viewpatientcreaterequest.ZipCode;
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();


                if (viewpatientcreaterequest.UploadFile != null)
                {
                    viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                    var requestwisefile = new Requestwisefile
                    {
                        Requestid = Request.Requestid,
                        Filename = viewpatientcreaterequest.UploadImage,
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(new[] { false})
                };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region PatientFamilyFriend
        public async Task<bool> PatientFamilyFriend(ViewPatientFamilyFriend viewpatientcreaterequest)
        {
            try
            {
                var Request = new Request();
                var Requestclient = new Requestclient();
                
                    Request.Requesttypeid = 3;
                    Request.Status = 1;
                    Request.Confirmationnumber = GetConfirmationNumber(viewpatientcreaterequest.State, viewpatientcreaterequest.FirstName, viewpatientcreaterequest.LastName);
                    Request.Firstname = viewpatientcreaterequest.FirstName;
                    Request.Lastname = viewpatientcreaterequest.LastName;
                    Request.Email = viewpatientcreaterequest.Email;
                    Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                    Request.Relationname = viewpatientcreaterequest.FF_RelationWithPatient;
                    Request.Isurgentemailsent = new BitArray(1);
                    Request.Isdeleted = new BitArray(1);
                    Request.Isdeleted[0] = false;
                    Request.Createddate = DateTime.Now;
                    _context.Requests.Add(Request);
                    await _context.SaveChangesAsync();

                    Requestclient.Requestid = Request.Requestid;
                Requestclient.Location = viewpatientcreaterequest.Symptoms;
                Requestclient.Firstname = viewpatientcreaterequest.FirstName;
                    Requestclient.Street = viewpatientcreaterequest.Street;
                    Requestclient.City = viewpatientcreaterequest.City;
                    Requestclient.State = viewpatientcreaterequest.State;
                    Requestclient.Address = viewpatientcreaterequest.Street + "," + viewpatientcreaterequest.City + "," + viewpatientcreaterequest.State + "," + viewpatientcreaterequest.ZipCode;
                    Requestclient.Lastname = viewpatientcreaterequest.LastName;
                Requestclient.Notes = viewpatientcreaterequest.Symptoms;
                Requestclient.Regionid = viewpatientcreaterequest.RegionId;
                Requestclient.Zipcode = viewpatientcreaterequest.ZipCode;

                Requestclient.Intdate = viewpatientcreaterequest.BirthDate.Value.Day;
                    Requestclient.Intyear = viewpatientcreaterequest.BirthDate.Value.Year;
                    Requestclient.Strmonth = viewpatientcreaterequest.BirthDate.Value.Month.ToString();
                    Requestclient.Email = viewpatientcreaterequest.Email;
                    Requestclient.Phonenumber = viewpatientcreaterequest.PhoneNumber;

                    _context.Requestclients.Add(Requestclient);
                    await _context.SaveChangesAsync();
                     if (viewpatientcreaterequest.UploadFile != null)
                     {
                         viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                         var requestwisefile = new Requestwisefile
                         {
                             Requestid = Request.Requestid,
                             Filename = viewpatientcreaterequest.UploadImage,
                             Createddate = DateTime.Now,
                             Isdeleted = new BitArray(new[] { false})
                         };
                         _context.Requestwisefiles.Add(requestwisefile);
                         _context.SaveChanges();
                     }
                   
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region PatientConcierge
        public async Task<bool> PatientConcierge(ViewPatientConcierge viewdata)
        {
            try
            {
                var Concierge = new Concierge();

                var Request = new Request();
                var Requestclient = new Requestclient();
                var Requestconcierge = new Requestconcierge();

                Concierge.Conciergename = viewdata.CON_FirstName + viewdata.CON_LastName;
                Concierge.Street = viewdata.CON_Street;
                Concierge.City = viewdata.CON_City;
                Concierge.State = viewdata.CON_State;
                Concierge.Zipcode = viewdata.CON_ZipCode;
                Concierge.Regionid = 1;
                Concierge.Createddate = DateTime.Now;
                _context.Concierges.Add(Concierge);
                await _context.SaveChangesAsync();
                int id1 = Concierge.Conciergeid;


                Request.Requesttypeid = 4;
                Request.Status = 1;
                Request.Confirmationnumber = GetConfirmationNumber(viewdata.CON_State, viewdata.FirstName, viewdata.LastName);
                Request.Firstname = viewdata.FirstName;
                Request.Lastname = viewdata.LastName;
                Request.Email = viewdata.Email;
                Request.Phonenumber = viewdata.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                Request.Createddate = DateTime.Now;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();
                int id2 = Request.Requestid;

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Location = viewdata.Symptoms;
                Requestclient.Firstname = viewdata.FirstName;
                Requestclient.Lastname = viewdata.LastName;
                Requestclient.Notes = viewdata.Symptoms;
                Requestclient.Email = viewdata.Email;
                Requestclient.Phonenumber = viewdata.PhoneNumber;
                Requestclient.Street = viewdata.CON_Street;
                Requestclient.City = viewdata.CON_City;
                Requestclient.Regionid = viewdata.RegionId;
                Requestclient.State = viewdata.CON_State;
                Requestclient.Address = viewdata.CON_Street + "," + viewdata.CON_City + "," + viewdata.CON_State + "," + viewdata.CON_ZipCode;
                Requestclient.Intdate = viewdata.BirthDate.Value.Day;
                Requestclient.Zipcode = viewdata.CON_ZipCode;

                Requestclient.Intyear = viewdata.BirthDate.Value.Year;
                Requestclient.Strmonth = viewdata.BirthDate.Value.Month.ToString();
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                Requestconcierge.Requestid = id2;
                Requestconcierge.Conciergeid = id1;

                _context.Requestconcierges.Add(Requestconcierge);
                await _context.SaveChangesAsync();

                var d = httpContextAccessor.HttpContext.Request.Host;
                //var res = _context.Requestclients.FirstOrDefault(e => e.Requestid == v.RequestID);
                string emailContent = @"
                                <!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                 <meta charset=""UTF-8"">
                                 <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                 <title>Patient Agreement</title>
                                </head>
                                <body>
                                 <div style=""background-color: #f5f5f5; padding: 20px;"">
                                 <h2>Welcome to Our Healthcare Platform!</h2>
                                <p>Dear Patient ,</p>
                                <ol>
                                    <li>Click the following link to Agreement:</li>
                                     <p><a target='_blank' href=""https://" + d + @"/Login/CreateAccount"" > Patient Agreement Submit That</a></p>
                                    <li>Follow the on-screen instructions to complete the registration process.</li>
                                </ol>
                                <p>If you have any questions or need further assistance, please don't hesitate to contact us.</p>
                                <p>Thank you,</p>
                                <p>The Healthcare Team</p>
                                </div>
                                </body>
                                </html>
                                ";
                Emaillogdata elog = new Emaillogdata();
                elog.Emailtemplate = emailContent;
                elog.Subjectname = " New Patient Account Creation";
                elog.Emailid = viewdata.Email;
                elog.Createdate = DateTime.Now;
                elog.Sentdate = DateTime.Now;
                elog.Recipient = Request.Firstname + ' ' + Request.Lastname;
                elog.Requestid = id2;
               
                elog.Action = 6;
                elog.Roleid = 4;
                await _requestRepository.EmailLog(elog);

                //_email.SendMail(viewdata.Email, "Your Request For patient Account is crearted please register with the link <a href='https://localhost:44376/Login/CreateAccount' class='btn btn info' > Creat Account  </a> ", "New Patient Account Creation");


                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region PatientBusiness
        public async Task<bool> PatientBusiness(ViewPatientBusiness viewdata)
        {
            try
            {
                var Business = new Business();
                var Request = new Request();
                var Requestclient = new Requestclient();
                var Requestbusiness = new Requestbusiness();

                Business.Name = viewdata.BUP_FirstName + viewdata.BUP_LastName;

                Business.Createddate = DateTime.Now;
                _context.Businesses.Add(Business);
                await _context.SaveChangesAsync();
                int id1 = Business.Businessid;


                Request.Requesttypeid = 1;
                Request.Status = 1;
                Request.Confirmationnumber = GetConfirmationNumber(viewdata.State, viewdata.FirstName, viewdata.LastName);
                Request.Firstname = viewdata.FirstName;
                Request.Lastname = viewdata.LastName;
                Request.Email = viewdata.Email;
                Request.Phonenumber = viewdata.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                Request.Createddate = DateTime.Now;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();
                int id2 = Request.Requestid;

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewdata.FirstName;
                Requestclient.Lastname = viewdata.LastName;
                Requestclient.Email = viewdata.Email;
                Requestclient.Location = viewdata.Symptoms;
                Requestclient.Phonenumber = viewdata.PhoneNumber;
                Requestclient.Regionid = viewdata.RegionId;
                Requestclient.Notes = viewdata.Symptoms;
                Requestclient.Street = viewdata.Street;
                Requestclient.City = viewdata.City;
                Requestclient.Zipcode = viewdata.ZipCode;

                Requestclient.State = viewdata.State;
                Requestclient.Address = viewdata.Street + "," + viewdata.City + "," + viewdata.State + "," + viewdata.ZipCode;
                Requestclient.Intdate = viewdata.BirthDate.Day;
                Requestclient.Intyear = viewdata.BirthDate.Year;
                Requestclient.Strmonth = viewdata.BirthDate.Month.ToString();
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                Requestbusiness.Requestid = id2;
                Requestbusiness.Businessid = id1;

                _context.Requestbusinesses.Add(Requestbusiness);
                await _context.SaveChangesAsync();

                var d = httpContextAccessor.HttpContext.Request.Host;
                //var res = _context.Requestclients.FirstOrDefault(e => e.Requestid == v.RequestID);
                string emailContent = @"
                                <!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                 <meta charset=""UTF-8"">
                                 <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                 <title>Patient Agreement</title>
                                </head>
                                <body>
                                 <div style=""background-color: #f5f5f5; padding: 20px;"">
                                 <h2>Welcome to Our Healthcare Platform!</h2>
                                <p>Dear Patient ,</p>
                                <ol>
                                    <li>Click the following link to Agreement:</li>
                                     <p><a target='_blank' href=""https://" + d + @"/Login/CreateAccount"" > Patient Agreement Submit That</a></p>
                                    <li>Follow the on-screen instructions to complete the registration process.</li>
                                </ol>
                                <p>If you have any questions or need further assistance, please don't hesitate to contact us.</p>
                                <p>Thank you,</p>
                                <p>The Healthcare Team</p>
                                </div>
                                </body>
                                </html>
                                ";
                Emaillogdata elog = new Emaillogdata();
                elog.Emailtemplate = emailContent;
                elog.Subjectname = " New Patient Account Creation";
                elog.Emailid = viewdata.Email;
                elog.Createdate = DateTime.Now;
                elog.Sentdate = DateTime.Now;
                elog.Recipient = Request.Firstname + ' ' + Request.Lastname;

                elog.Requestid = id2;

                elog.Action = 6;
                elog.Roleid = 4;
                await _requestRepository.EmailLog(elog);
                //_email.SendMail(viewdata.Email, "Your Request For patient Account is crearted please register with the link https://localhost:44376/Login/CreateAccount ", "New Patient Account Creation");

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region PatientForMe
        public async Task<bool> PatientForMe(ViewPatientCreateRequest viewpatientcreaterequest,int UserId)
        {
            try
            {
                var Request = new Request();
                var Requestclient = new Requestclient();

                Request.Requesttypeid = 2;
                Request.Status = 1;
                Request.Confirmationnumber = GetConfirmationNumber(viewpatientcreaterequest.State, viewpatientcreaterequest.FirstName, viewpatientcreaterequest.LastName);
                Request.Userid = UserId;
                Request.Firstname = viewpatientcreaterequest.FirstName;
                Request.Lastname = viewpatientcreaterequest.LastName;
                Request.Email = viewpatientcreaterequest.Email;
                Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                Request.Createddate = DateTime.Now;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewpatientcreaterequest.FirstName;
                Requestclient.Address = viewpatientcreaterequest.Street;
                Requestclient.Lastname = viewpatientcreaterequest.LastName;
                Requestclient.Email = viewpatientcreaterequest.Email;
                Requestclient.Location = viewpatientcreaterequest.Symptoms;
                Requestclient.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Requestclient.Latitude = viewpatientcreaterequest.latitude;
                Requestclient.Regionid = viewpatientcreaterequest.RegionId;
                Requestclient.Longitude = viewpatientcreaterequest.longitude;
                Requestclient.Notes = viewpatientcreaterequest.Symptoms;
                Requestclient.Street = viewpatientcreaterequest.Street;
                Requestclient.City = viewpatientcreaterequest.City;
                Requestclient.Zipcode = viewpatientcreaterequest.ZipCode;

                Requestclient.State = viewpatientcreaterequest.State;
                Requestclient.Intdate = viewpatientcreaterequest.BirthDate.Day;
                Requestclient.Intyear = viewpatientcreaterequest.BirthDate.Year;
                Requestclient.Strmonth = viewpatientcreaterequest.BirthDate.Month.ToString();
                Requestclient.Address = viewpatientcreaterequest.Street + "," + viewpatientcreaterequest.City + "," + viewpatientcreaterequest.State + "," + viewpatientcreaterequest.ZipCode;

                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                if (viewpatientcreaterequest.UploadFile != null)
                {
                    viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                    var requestwisefile = new Requestwisefile
                    {
                        Requestid = Request.Requestid,
                        Filename = viewpatientcreaterequest.UploadImage,
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(new[] { false})
                    };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {

                return false;

            }
        }
        #endregion

        #region PatientForSomeoneElse
        public async Task<bool> PatientForSomeoneElse(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            try
            {
                var Request = new Request();
                var Requestclient = new Requestclient();

                Request.Requesttypeid = 2;
                Request.Status = 1;
                Request.Confirmationnumber = GetConfirmationNumber(viewpatientcreaterequest.State, viewpatientcreaterequest.FirstName, viewpatientcreaterequest.LastName);
                Request.Firstname = viewpatientcreaterequest.FirstName;
                Request.Lastname = viewpatientcreaterequest.LastName;
                Request.Email = viewpatientcreaterequest.Email;
                Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Request.Relationname = viewpatientcreaterequest.Realtion;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Isdeleted = new BitArray(1);
                Request.Isdeleted[0] = false;
                Request.Createddate = DateTime.Now;
                Request.Relationname = viewpatientcreaterequest.Realtion;
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
                Requestclient.Location = viewpatientcreaterequest.Symptoms;
                Requestclient.Regionid = viewpatientcreaterequest.RegionId;
                Requestclient.Zipcode = viewpatientcreaterequest.ZipCode;
                Requestclient.State = viewpatientcreaterequest.State;
                Requestclient.Notes = viewpatientcreaterequest.Symptoms;
                Requestclient.Intdate = viewpatientcreaterequest.BirthDate.Day;
                Requestclient.Intyear = viewpatientcreaterequest.BirthDate.Year;
                Requestclient.Strmonth = viewpatientcreaterequest.BirthDate.Month.ToString();
                Requestclient.Address = viewpatientcreaterequest.Street + "," + viewpatientcreaterequest.City + "," + viewpatientcreaterequest.State + "," + viewpatientcreaterequest.ZipCode;

                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();
                var d = httpContextAccessor.HttpContext.Request.Host;
                //var res = _context.Requestclients.FirstOrDefault(e => e.Requestid == v.RequestID);
                string emailContent = @"
                                <!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                 <meta charset=""UTF-8"">
                                 <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                 <title>Patient Agreement</title>
                                </head>
                                <body>
                                 <div style=""background-color: #f5f5f5; padding: 20px;"">
                                 <h2>Welcome to Our Healthcare Platform!</h2>
                                <p>Dear Patient ,</p>
                                <ol>
                                    <li>Click the following link to Agreement:</li>
                                     <p><a target='_blank' href=""https://" + d + @"/Login/CreateAccount"" > Patient Agreement Submit That</a></p>
                                    <li>Follow the on-screen instructions to complete the registration process.</li>
                                </ol>
                                <p>If you have any questions or need further assistance, please don't hesitate to contact us.</p>
                                <p>Thank you,</p>
                                <p>The Healthcare Team</p>
                                </div>
                                </body>
                                </html>
                                ";
                Emaillogdata elog = new Emaillogdata();
                elog.Emailtemplate = emailContent;
                elog.Subjectname = " New Patient Account Creation";
                elog.Emailid = viewpatientcreaterequest.Email;
                elog.Createdate = DateTime.Now;
                elog.Sentdate = DateTime.Now;
                elog.Recipient = Request.Firstname + ' ' + Request.Lastname;
                elog.Requestid = Request.Requestid;
                elog.Action = 6;
                elog.Roleid = 4;
                await _requestRepository.EmailLog(elog);

                //_email.SendMail(viewpatientcreaterequest.Email, "Your Request For patient Account is crearted please register with the link https://localhost:44376/Login/CreateAccount ", "New Patient Account Creation");

                if (viewpatientcreaterequest.UploadFile != null)
                {
                    viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                    var requestwisefile = new Requestwisefile
                    {
                        Requestid = Request.Requestid,
                        Filename = viewpatientcreaterequest.UploadImage,
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(new[] { false})
                    };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                }
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
