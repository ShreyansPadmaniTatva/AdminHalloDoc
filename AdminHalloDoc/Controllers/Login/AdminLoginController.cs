using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Controllers.Login
{
    public class AdminLoginController : Controller
    {
        #region Configuration
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        private readonly ILoginRepository _loginRepository;
        public AdminLoginController(ApplicationDbContext context, EmailConfiguration emailConfig, ILoginRepository loginRepository)
        {
            _context = context;
            _emailConfig = emailConfig;
            _loginRepository = loginRepository;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Start_session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAccessLogin(Aspnetuser aspNetUser)
        {
            UserInfo admin = await _loginRepository.CheckAccessLogin(aspNetUser);
            if (admin != null)
            {
                TempData["error"] = "Correct";
                SessionUtils.setLogginUser(HttpContext.Session, admin);

                return RedirectToAction("Index", "AdminDashboard");
            }
            else
            {
                TempData["error"] = "InCorrect Id Or Pass";
            }


            return RedirectToAction("Index");
        }
        #endregion
    }
}
