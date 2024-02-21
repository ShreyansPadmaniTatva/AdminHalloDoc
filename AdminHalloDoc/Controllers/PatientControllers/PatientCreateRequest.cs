using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Globalization;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientCreateRequest : Controller
    {
        #region Configuration
        private readonly ApplicationDbContext _context;
        public PatientCreateRequest(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region CheckEmailAsync
        [HttpPost]
        public async Task<IActionResult> CheckEmailAsync(string email)
        {
            string message;
            string Data = "0";
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
            //await _context.SaveChangesAsync();
            if (aspnetuser == null)
            {
                message = "False";

                Data = "0";
            }
            else
            {
                message = "success";
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Aspnetuserid == aspnetuser.Id.ToString());
                HttpContext.Session.SetString("UserName", aspnetuser.Username.ToString());
                HttpContext.Session.SetString("UserID", user.Userid.ToString());
                Data = user.Userid.ToString();
            }
            return Json(new
            {
                Data,
                Message = message,
            });
        }
        #endregion

        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            if (ModelState.IsValid)
            {
                var Aspnetuser = new Aspnetuser();
                var User = new User();
                var Request = new Request();
                var Requestclient = new Requestclient();
            
                if ( viewpatientcreaterequest.PassWord != null)
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
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewpatientcreaterequest.FirstName;
                Requestclient.Address = viewpatientcreaterequest.Street;
                Requestclient.Lastname = viewpatientcreaterequest.LastName;
                Requestclient.Email = viewpatientcreaterequest.Email;
                Requestclient.Phonenumber = viewpatientcreaterequest.PhoneNumber;

                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();


                viewpatientcreaterequest.UploadImage = CM.UploadDoc(viewpatientcreaterequest.UploadFile, Request.Requestid);

                var requestwisefile = new Requestwisefile
                    {
                        Requestid = Request.Requestid,
                        Filename = viewpatientcreaterequest.UploadImage,
                        Createddate = DateTime.Now,
                    };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                
            }
            else
            {
                return View("../PatientCreateRequest/Index", viewpatientcreaterequest);
            }
                return RedirectToAction("Index", "Dashboard");
            
        }
        #endregion
    }
}
