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
            // Log the error
            var errorId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError($"Error occurred with ID: {errorId}");

            // You can customize the view model or directly pass the error message to the view
            var errorViewModel = new ErrorViewModel { RequestId = errorId, ErrorMessage = "An unexpected error occurred." };

            // You can return a specific view based on the error type
            return View("../PatientViews/Home/CustomErrorView", errorViewModel);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}


