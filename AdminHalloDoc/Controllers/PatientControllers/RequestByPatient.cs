using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Models.CV;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class RequestByPatient : Controller
    {
        #region Configuration
        private readonly ApplicationDbContext _context;
        public RequestByPatient(ApplicationDbContext context)
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

        #region SubmitForSomeoneElse
        public IActionResult SubmitForSomeoneElse()
        {
           
            return View();
        }
        public IActionResult SubmitForMe()
        {
            var ViewPatientCreateRequest = _context.Users
                               .Where(r => r.Userid == Convert.ToInt32(CV.UserID()))
                               .Select(r => new ViewPatientCreateRequest
                               {
                                   FirstName = r.Firstname,
                                   LastName = r.Lastname,
                                   Email = r.Email,
                                   PhoneNumber = r.Mobile,
                                   BirthDate = new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()), (int)r.Intdate),
                               })
                               .FirstOrDefault();
            return View(ViewPatientCreateRequest);
        }
        #endregion

        #region PostMeAsync
        public async Task<IActionResult> PostMe(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            

                var Request = new Request();
                var Requestclient = new Requestclient();

                Request.Requesttypeid = 2;
                Request.Userid = Convert.ToInt32(CV.UserID());
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
                Requestclient.Latitude = viewpatientcreaterequest.latitude;
                Requestclient.Longitude = viewpatientcreaterequest.longitude;

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
                
         
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

        #region PostSomeoneElseAsync
        public async Task<IActionResult> PostSomeoneElse(ViewPatientCreateRequest viewpatientcreaterequest)
        {
            if (ModelState.IsValid)
            {
                var Request = new Request();
                var Requestclient = new Requestclient();

                Request.Requesttypeid = 2;
                Request.Userid = Convert.ToInt32(CV.UserID());
                Request.Firstname = viewpatientcreaterequest.FirstName;
                Request.Lastname = viewpatientcreaterequest.LastName;
                Request.Email = viewpatientcreaterequest.Email;
                Request.Phonenumber = viewpatientcreaterequest.PhoneNumber;
                Request.Relationname = viewpatientcreaterequest.Realtion;
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
                return View("../RequestByPatient/SubmitForSomeoneElse", viewpatientcreaterequest);
            }
            return View("../Dashboard/Index");
        }
        #endregion
    }
}
