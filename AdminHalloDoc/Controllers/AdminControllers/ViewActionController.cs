using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ViewActionController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        public ViewActionController(IRequestRepository requestRepository)
        {

            _requestRepository = requestRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Save_Viewcase
        public async Task<IActionResult> SaveViewcase(Viewcase viewcase)
        {
           Boolean fr = await _requestRepository.PutViewcase(viewcase);

            return View("../AdminViews/ViewAction/Viewctgrgase");
        }
        #endregion
    }
}
