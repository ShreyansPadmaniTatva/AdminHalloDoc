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
            ViewData["Status"]  = await _requestRepository.PutViewcase(viewcase);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../AdminViews/ViewAction/Viewcase", viewcase);
        }
        #endregion

        #region Send_Link
        public IActionResult SendLink(string firstname,string lastname, string email, string phonenumber)
        {
            if (_viewActionRepository.SendLink( firstname,  lastname,  email,  phonenumber))
            {
                
                TempData["Status"] = "Link Send In mail Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard"); 
        }
        #endregion

        #region UploadDoc_Files
        public IActionResult UploadDoc(int Requestid, IFormFile file)
        {

            _viewActionRepository.SaveDoc(Requestid, file);
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid });
        }
        #endregion


        #region AssignProvider
        public async Task<IActionResult> AssignProvider(int requestid, int ProviderId, string Notes)
        {
           if( await _viewActionRepository.AssignProvider(requestid, ProviderId, Notes))
            {
                TempData["Status"] = "Assign Provider Successfully..!";
            }
            

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #region TransferToProvider
        public async Task<IActionResult> TransferToProvider(int requestid, int ProviderId, string Notes, int TProviderId)
        {
            if (await _viewActionRepository.TransferToProvider(requestid, ProviderId, Notes, TProviderId))
            {
                TempData["Status"] = "Transfer Provider Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        # endregion


    }
}
