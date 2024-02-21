using AdminHalloDoc.Entities;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class PatientBusinessController : Controller
    {
        #region Configuration
        public ApplicationDbContext _context;
        private readonly EmailConfiguration _emailConfig;
        public PatientBusinessController(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Post
        public async Task<IActionResult> Post(ViewPatientBusiness viewdata)
        {
            if (ModelState.IsValid)
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
                int id2 = Request.Requestid;

                Requestclient.Requestid = Request.Requestid;
                Requestclient.Firstname = viewdata.FirstName;
                Requestclient.Lastname = viewdata.LastName;
                Requestclient.Email = viewdata.Email;
                Requestclient.Phonenumber = viewdata.PhoneNumber;
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                Requestbusiness.Requestid = id2;
                Requestbusiness.Businessid = id1;

                _context.Requestbusinesses.Add(Requestbusiness);
                await _context.SaveChangesAsync();

                _emailConfig.SendMail(viewdata.Email, "Your Request For patient Account is crearted please register with the link https://localhost:44376/Login/CreateAccount ", "New Patient Account Creation");
            }
            else
            {
                return View("../PatientBusiness/Index", viewdata);
            }

            return RedirectToAction("Index", "SubmitRequest");

        }
        #endregion
    }
}
