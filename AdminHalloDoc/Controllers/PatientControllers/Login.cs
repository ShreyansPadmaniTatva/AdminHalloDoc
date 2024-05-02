using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class Login : Controller
    {
        #region Configuration
        private readonly EmailConfiguration _emailConfig;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IRequestRepository _requestRepository;
        private readonly ILoginRepository _loginRepository;
        public Login(ApplicationDbContext context, EmailConfiguration emailConfig, IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor, ILoginRepository loginRepository)
        {
            _context = context;
            _emailConfig = emailConfig;
            _requestRepository = requestRepository;
            this.httpContextAccessor = httpContextAccessor;
            _loginRepository = loginRepository;
        }
        #endregion


        #region Login

        public IActionResult Index()
        {
            return View();
        }

        #region Start_session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAccessLoginAsync(Aspnetuser aspNetUser)
        {
            var user = await _context.Aspnetusers.FirstOrDefaultAsync(u => u.Username == aspNetUser.Username);
            if (user != null)
            {
                var hasher = new PasswordHasher<string>();
                PasswordVerificationResult result = hasher.VerifyHashedPassword(null, user.Passwordhash, aspNetUser.Passwordhash);
                if (result != PasswordVerificationResult.Success)
                {
                    ViewData["error"] = "Invalid Id Pass";
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetString("UserName", user.Username.ToString());
                    var U = await _context.Users.FirstOrDefaultAsync(m => m.Aspnetuserid == user.Id.ToString());
                    HttpContext.Session.SetString("UserID", U.Userid.ToString());
                    return RedirectToAction("Index", "Dashboard");
                }
            }


            return RedirectToAction(nameof(Index));
        }
        #endregion

        #endregion

        #region Forgot_Password
        public IActionResult ForgotPassword()
        {
            return View();
        }

        #region SendMail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetEmail(string Email)
        {
            if (await CheckregisterdAsync(Email))
            {
                var hostName = httpContextAccessor.HttpContext.Request.Host.Host;
                var port = httpContextAccessor.HttpContext.Request.Host.Port;
                var resetPasswordUrl = $"https://{hostName}:{port}/Login/ResetPassword?Datetime={_emailConfig.Encode(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"))}&email={_emailConfig.Encode(Email)}";
                var Subject = "Change Password";
                var Body = $" <html><body> <h2>Welcome to Our Healthcare Platform!</h2>     " +
                    $"                           <p>Dear Patient ,</p>" +
                    $"                             <ol>" +
                    $"                            <li>Click the following link to Reset Pass:</li>" +
                    $" <li>your reset password link is <a href='{resetPasswordUrl}'>Click here</a></li>" +
                    $"</ol>" +
                    $"</body></html>";

                Emaillogdata elog = new Emaillogdata();
                elog.Emailtemplate = Body;
                elog.Subjectname = Subject;
                elog.Emailid = Email;
                elog.Createdate = DateTime.Now;
                elog.Sentdate = DateTime.Now;
                elog.Action = 5;
                elog.Recipient = Email;
                elog.Roleid = 4;
                await _requestRepository.EmailLog(elog);
                //_emailConfig.SendMail(Email, Subject, Body);

                ViewData["EmailCheck"] = "Your Reset Link Send In Your Mail";
            }
            else
            {
                ViewData["EmailCheck"] = "Your Email Is not registered";
                return View("ForgotPassword");
            }
            return View("Index");
        }
        #endregion

        #region Help_functions
        public async Task<bool> CheckregisterdAsync(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!string.IsNullOrEmpty(email) && Regex.IsMatch(email, pattern))
            {

                var U = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
                if (U != null)
                {
                    return true;
                }
            }

            return false;
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool AspnetuserExists(string id)
        {
            return (_context.Aspnetusers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion

        #endregion

        #region Logout
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "SubmitRequest");
        }
        #endregion

        #region Reset_Password
        #region ResetPassWord

        public async Task<IActionResult> ResetPassWord(string? Datetime, string? email)
        {
            string Decodee = _emailConfig.Decode(email);
            DateTime s = DateTime.ParseExact(_emailConfig.Decode(Datetime), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            TimeSpan dif = DateTime.Now - s;
            if (!_loginRepository.IsPasswordModify(Decodee))
            {
                ViewBag.URl = "Pass Is Change Only Ones.";
                return View("../SendAgreement/404");
            }
            if (dif.Days == 0 && dif.Hours < 24)
            {

                ViewBag.email = Decodee;
                return View();

            }
            else
            {
                ViewBag.URl = "Url is expaier";
            }
            return View("../SendAgreement/404");
        }
        #endregion

        #region ResetPassWord_SavePassAsync
        public async Task<IActionResult> SavePassAsync(string ConfirmPassword, string Password, string Email)
        {
            if (Password != null)
            {
                if (ConfirmPassword != Password)
                {
                    ViewData["error"] = "Pass is Mismatch";
                    return View("ResetPassWord");
                }
                try
                {
                    Aspnetuser U = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == Email);
                    var hasher = new PasswordHasher<string>();
                    U.Passwordhash = hasher.HashPassword(null, Password);
                    U.Modifieddate = DateTime.Now;
                    _context.Update(U);
                    await _context.SaveChangesAsync();
                    ViewData["error"] = "Pass is Upadated";
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return View("Index");
        }
        #endregion
        #endregion

        #region Create_Account

        public IActionResult CreateAccount()
        {

            return View();
        }


        public async Task<IActionResult> CreatNewAccont(string Email, string Password)
        {
            try
            {
                Guid id = Guid.NewGuid();
                var hasher = new PasswordHasher<string>();
                Aspnetuser aspnetuser = new Aspnetuser
                {
                    Id = id.ToString(),
                    Email = Email,
                    Passwordhash = hasher.HashPassword(null, Password),
                    Username = Email,
                    CreatedDate = DateTime.Now,
                };
                _context.Aspnetusers.Add(aspnetuser);
                await _context.SaveChangesAsync();
                var U = await _context.Requestclients.FirstOrDefaultAsync(m => m.Email == Email);
                var User = new User
                {
                    Aspnetuserid = aspnetuser.Id,
                    Firstname = U.Firstname,
                    Lastname = U.Lastname,
                    Mobile = U.Phonenumber,
                    Intdate = U.Intdate,
                    Intyear = U.Intyear,
                    Strmonth = U.Strmonth,
                    Email = Email,
                    Createdby = aspnetuser.Id,
                    Createddate = DateTime.Now,
                    Isrequestwithemail = new BitArray(1),
                };
                _context.Users.Add(User);
                await _context.SaveChangesAsync();

                var aspnetuserroles = new Aspnetuserrole();
                aspnetuserroles.Userid = User.Aspnetuserid;
                aspnetuserroles.Roleid = "Patient";
                _context.Aspnetuserroles.Add(aspnetuserroles);
                _context.SaveChanges();

                var rc = _context.Requestclients.Where(e => e.Email == Email).ToList();

                foreach (var r in rc)
                {
                    _context.Requests.Where(n => n.Requestid == r.Requestid)
                   .ExecuteUpdate(s => s.SetProperty(
                       n => n.Userid,
                       n => User.Userid));
                }
                if (rc.Count > 0)
                {
                    User.Intdate = rc[0].Intdate;
                    User.Intyear = rc[0].Intyear;
                    User.Strmonth = rc[0].Strmonth;
                    _context.Users.Update(User);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");
        }


        #endregion
    }
}
