using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientFamilyFriend : Controller
    {
        #region Configuration
        private readonly ApplicationDbContext _context;

        public PatientFamilyFriend(ApplicationDbContext context)
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

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post(ViewPatientFamilyFriend viewdata)
        {

            var Request = new Request();
            var Requestclient = new Requestclient();
            if (ModelState.IsValid)
            {
                Request.Requesttypeid = 3;
                Request.Status = 1;
                Request.Firstname = viewdata.FirstName;
                Request.Lastname = viewdata.LastName;
                Request.Email = viewdata.Email;
                Request.Phonenumber = viewdata.PhoneNumber;
                Request.Isurgentemailsent = new BitArray(1);
                Request.Createddate = DateTime.Now;
                _context.Requests.Add(Request);
                await _context.SaveChangesAsync();

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewdata.FirstName;
                Requestclient.Address = viewdata.Street;
                Requestclient.Lastname = viewdata.LastName;
                Requestclient.Email = viewdata.Email;
                Requestclient.Phonenumber = viewdata.PhoneNumber;

                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                viewdata.UploadImage = CM.UploadDoc(viewdata.UploadFile, Request.Requestid);

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewdata.UploadImage,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }
            else
            {
                return View("../PatientFamilyFriend/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
