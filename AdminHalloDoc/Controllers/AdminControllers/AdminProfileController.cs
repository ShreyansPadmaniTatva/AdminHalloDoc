using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    [AdminAuth("Admin")]
    public class AdminProfileController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        public AdminProfileController(IMyProfileRepository myProfileRepository,IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            ViewAdminProfile p = await _myProfileRepository.GetProfileDetails(6);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return  View("../AdminViews/Profile/Index",p);
        }

        #region Update_Profile
        public async Task<IActionResult> UpdateProfile(ViewAdminProfile v)
        {
            if(ModelState.IsValid)
            {
                await _myProfileRepository.PutProfileDetails(v);
            }
            
            return RedirectToAction("Index");
        }
        #endregion
    }
}
