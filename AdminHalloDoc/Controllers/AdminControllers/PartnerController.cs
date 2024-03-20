using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class PartnerController : Controller
    {
        #region Constoter
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        private readonly IViewNotesRepository _viewNotesRepository;
        private readonly IMyProfileRepository _myProfileRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly EmailConfiguration _emailconfig;
        public PartnerController(IPhysicianRepository physicianRepository, IMyProfileRepository myProfileRepository, IRequestRepository requestRepository, IViewActionRepository viewActionRepository, IViewNotesRepository viewNotesRepository, EmailConfiguration emailConfiguration)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
            _viewNotesRepository = viewNotesRepository;
            _myProfileRepository = myProfileRepository;
            _physicianRepository = physicianRepository;
            _emailconfig = emailConfiguration;
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../AdminViews/Partner/Index");
        }


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResult(int? professionId)
        {
            List<Healthprofessional> r =await _viewNotesRepository.GetPartnersByProfession(professionId);
            return PartialView("../AdminViews/Partner/_List", r);
        }
        #endregion

        public async Task<IActionResult> PartnerAddEditAsync(int? venderId)
        {
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            if(venderId == null)
            {
                return View("../AdminViews/Partner/PartnerAddEdit");

            }

            return View("../AdminViews/Partner/PartnerAddEdit");
        }
        public async Task<IActionResult> SavePartnerAsync(Healthprofessional v)
        {
           
            await _viewNotesRepository.SavePartner(v);
            return RedirectToAction("Index");
        }
    }
}
