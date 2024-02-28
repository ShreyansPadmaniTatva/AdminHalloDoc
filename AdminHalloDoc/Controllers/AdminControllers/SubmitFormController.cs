using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class SubmitFormController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        public SubmitFormController(IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
        }
        #endregion

        #region FindVender
        public async Task<IActionResult> FindVender(int? VenderType)
        {
           var v = await _viewNotesRepository.FindVenderByVenderType(VenderType);
            return Json(v);
        }
        #endregion

        #region Get_Order
        public async Task<IActionResult> GetOrder(int? Venderid)
        {
            var v = await _viewNotesRepository.FindVenderByVenderID(Venderid);
            return Json(v);
        }
        #endregion

        #region SaveViewOrder
        [HttpPost]
        public async Task<IActionResult> SaveViewOrder(ViewOrder vieworder)
        {
            if (await _viewNotesRepository.SaveViewOrder(vieworder))
            {
                TempData["Status"] = "Request Orderd save Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion
    }
}
