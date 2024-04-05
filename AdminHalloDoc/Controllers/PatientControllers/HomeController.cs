using AdminHalloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AdminHalloDoc.Controllers.PatientControllers
{
    public class HomeController : Controller
    {
        #region Configuration
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            
           return View("../PatientViews/Home/Index");
        }
        #endregion

        #region error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}


