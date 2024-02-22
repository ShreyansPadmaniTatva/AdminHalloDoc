using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ViewActionController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        public ViewActionController(IRequestRepository requestRepository, IViewActionRepository viewActionRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
        }
        
        #region Save_Viewcase
        public async Task<IActionResult> SaveViewcase(Viewcase viewcase)
        {
           Boolean fr = await _requestRepository.PutViewcase(viewcase);

            return View("../AdminViews/ViewAction/Viewcase");
        }
        #endregion

        #region Send_Link
        public IActionResult SendLink(string firstname,string lastname, string email, string phonenumber)
        {
            _viewActionRepository.SendLink( firstname,  lastname,  email,  phonenumber);
            return View("../AdminViews/ViewAction/Viewcase");
        }
        #endregion

        #region UploadDoc_Files
        public IActionResult UploadDoc(int Requestid, IFormFile file)
        {

            _viewActionRepository.SaveDoc(Requestid, file);
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid });
        }
        #endregion

        
    }
}
