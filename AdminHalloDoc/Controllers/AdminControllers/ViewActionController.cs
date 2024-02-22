using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
    }
}
