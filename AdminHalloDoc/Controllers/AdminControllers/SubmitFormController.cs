using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        #region Close_Case
        public async Task<IActionResult> CloseCase(int? id)
        {
            ViewDocuments v = await _viewActionRepository.GetDocumentByRequest(id);
            return View("../AdminViews/ViewAction/CloseCase", v);
        }

        #region Close_Case_Changge
        public async Task<IActionResult> CloseCaseChangge(int id)
        {
            if (await _viewActionRepository.CloseCase(id))
            {

                TempData["Status"] = "Close Request Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region Upadte_Request
        public async Task<IActionResult> UpadteRequest(ViewDocuments viewdocuments)
        {
            Viewcase viewcase = new Viewcase();
            viewcase.Email= viewdocuments.Email;
            viewcase.PhoneNumber= viewdocuments.PhoneNumber;
            viewcase.RequesClientid= viewdocuments.RequesClientid;
            if (await _requestRepository.PutViewcase(viewcase))
            {

                TempData["Status"] = "Save Successfully..!";
            }

            return RedirectToAction("CloseCase", new { id = viewdocuments.RequestID });
        }
        #endregion

        #region Encounter

        #region Encounter_View
        public async Task<IActionResult> Encounter(int id)
        {

            ViewEncounter v =  _viewActionRepository.GetEncounterDetailsByRequestID(id);
            return View("../AdminViews/ViewAction/Encounter",v);
        }
        #endregion


        public IActionResult EncounterEdit(ViewEncounter model)
        {

            

            bool data = _viewActionRepository.EditEncounterDetails(model, CV.ID());
            if (data)
            {


                TempData["Status"] = "Encounter Changes Saved..";
            }
            else
            {
                TempData["Status"] =  "Encounter Changes Not Saved";
            }

            return RedirectToAction("Encounter", new { id = model.Requesid });

        }

        #region ACTION-FINALIZE
        public IActionResult Finalize(ViewEncounter model)
        {
            bool data = _viewActionRepository.EditEncounterDetails(model, CV.ID());
            if (data)
            {
                bool final = _viewActionRepository.CaseFinalized(model, CV.ID());
                if (final)
                {
                    TempData["Status"] = "Case Is Finalized";
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    TempData["Status"] = "Case Is not Finalized";
                    return View("../AdminSite/Action/Encounter", model);
                }

            }
            else
            {
                TempData["Status"] = "Case Is not Finalized";
                return View("../AdminSite/Action/Encounter", model);
            }

        }
        #endregion

#endregion

    }
}
