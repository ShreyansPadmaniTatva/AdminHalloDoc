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

        #region Partner_ViewPage
        [Route("Admin/Partner")]
        public async Task<IActionResult> Index()
        {
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            return View("../AdminViews/Partner/Index");
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _SearchResult(int? regionId,string? searchvender = null)
        {
            List<ViewVendorList> r =await _viewNotesRepository.GetPartnersByProfession(regionId, searchvender);
            return PartialView("../AdminViews/Partner/_List", r);
        }
        #endregion

        #region Partner_Add_Edit
        public async Task<IActionResult> PartnerAddEdit(int? id)
        {
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            if (id == null)
            {
                ViewData["Vender"] = "Add";
                return View("../AdminViews/Partner/PartnerAddEdit");

            }
            ViewData["Vender"] = "Edit";
            var v = await _viewNotesRepository.GetPartnerById(id);
            return View("../AdminViews/Partner/PartnerAddEdit",v);
        }
        #endregion

        #region SavePartnerAsync
        public async Task<IActionResult> SavePartnerAsync(Healthprofessional model)
        {
            TempData["Status"] = "Data Is Not Save.....";
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();

            if (ModelState.IsValid)
            {
                if (model.Vendorid == null)
                {
                    if (_viewNotesRepository.isBusinessNameExist(model.Vendorname).Count > 0)
                    {
                        ModelState.AddModelError("Vendorname", "Business name is Already Taken!! choose another one");
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }
                    if (_viewNotesRepository.isEmailExist(model.Email).Count > 0)
                    {
                        ModelState.AddModelError("Email", "Email is Already Taken!! choose another one");
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }

                    bool data = await _viewNotesRepository.SavePartner(model);
                    if (data)
                    {
                        TempData["Status"] = "Vendor added successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Status"] = "Vendor Not added";
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }
                }
                else
                {
                    if (_viewNotesRepository.isBusinessNameExist(model.Vendorname).Count >= 1 && _viewNotesRepository.isBusinessNameExist(model.Vendorname).Any(u => u.Vendorid != model.Vendorid))
                    {
                        ModelState.AddModelError("Vendorname", "Business name is Already Taken!! choose another one");
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }
                    if (_viewNotesRepository.isEmailExist(model.Email).Count >= 1 && _viewNotesRepository.isEmailExist(model.Email).Any(u => u.Vendorid != model.Vendorid))
                    {
                        ModelState.AddModelError("Email", "Email is Already Taken!! choose another one");
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }
                    bool data = await _viewNotesRepository.SavePartner(model);
                    if (data)
                    {
                        TempData["Status"] = "Vendor Edited successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Status"] = "Vendor Not Edited";
                        return View("../AdminViews/Partner/PartnerAddEdit", model);
                    }
                }
            }

            //if (_viewNotesRepository.isBusinessNameExist(v.Vendorname) > 0)
            //{
            //    ModelState.AddModelError("Vendorname", "Business name is Already Taken!! choose another one");
            //    return View("../AdminViews/Partner/PartnerAddEdit", v);
            //}
            //if (_viewNotesRepository.isEmailExist(v.Email) > 0)
            //{
            //    ModelState.AddModelError("Email", "Email is Already Taken!! choose another one");
            //    return View("../AdminViews/Partner/PartnerAddEdit", v);
            //}


            //if (ModelState.IsValid && await _viewNotesRepository.SavePartner(v))
            //{
            //    TempData["Status"] = "Data Is  Save.....!";
            //}
            //else
            //{
               
            //    return View("../AdminViews/Partner/PartnerAddEdit", v);
            //}

            return RedirectToAction("Index");
        }
        #endregion

        #region DeletePartner
        public async Task<IActionResult> DeletePartner(int? venderId)
        {
            if (await _viewNotesRepository.DeletePartnerById(venderId))
            {
                TempData["Status"] = "Partner Is Deleted  .....!";
            }
            else
            {
                TempData["Status"] = "Partner Is Not Deleted  .....!";
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
