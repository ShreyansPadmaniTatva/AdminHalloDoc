using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Models;
using AdminHalloDoc.Models.CV;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    [CheckAccess]
    public class Profile : Controller
    {
        #region Configuration
        private readonly ApplicationDbContext _context;

        public Profile(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            var UsersProfile = _context.Users
                                .Where(r => r.Userid == Convert.ToInt32(CV.UserID()))
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
                                    Birthdate = new DateTime((int)r.Intyear, Convert.ToInt32(r.Strmonth.Trim()),(int)r.Intdate),
                                 })
                                .FirstOrDefault();

            return View(UsersProfile);
        }
        #endregion

        #region Put
        public async Task<IActionResult> Put(ViewUserProfile userprofile)
        {
            try
            {
                User userToUpdate = await _context.Users.FindAsync(userprofile.Userid);
              
                userToUpdate.Firstname = userprofile.Firstname;
                    userToUpdate.Lastname = userprofile.Lastname;
                    userToUpdate.Mobile = userprofile.Mobile;
                    userToUpdate.Email = userprofile.Email;
                    userToUpdate.State = userprofile.State;
                    userToUpdate.Street = userprofile.Street;
                    userToUpdate.City = userprofile.City;
                    userToUpdate.Zipcode = userprofile.Zipcode;
                    userToUpdate.Intdate = userprofile.Birthdate.Day;
                    userToUpdate.Intyear = userprofile.Birthdate.Year;
                userToUpdate.Strmonth = userprofile.Birthdate.Month.ToString();
                userToUpdate.Modifiedby = userprofile.Createdby;
                userToUpdate.Modifieddate = DateTime.Now;
                _context.Update(userToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userprofile.Userid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        private bool UserExists(object id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
