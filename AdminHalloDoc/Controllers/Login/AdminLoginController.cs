﻿using AdminHalloDoc.Entities.Data;
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
        private readonly IJwtService _jwtService;
        public AdminLoginController(ApplicationDbContext context, EmailConfiguration emailConfig, ILoginRepository loginRepository, IJwtService jwtService)
        {
            _context = context;
            _emailConfig = emailConfig;
            _loginRepository = loginRepository;
            _jwtService = jwtService;
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
                // SessionUtils.setLogginUser(HttpContext.Session, admin);

                var jwttoken = _jwtService.GenerateJWTAuthetication(admin);
                Response.Cookies.Append("jwt",jwttoken);

                if (admin.Role == "Patient")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Index", "AdminDashboard");
            }
            else
            {
                TempData["error"] = "InCorrect Id Or Pass";
                TempData["Status"] = "InCorrect Id Or Pass";
            }


            return RedirectToAction("Index");
        }
        #endregion

        #region Start_session
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");

            return RedirectToAction("Index");
        }
        #endregion


    }
}
