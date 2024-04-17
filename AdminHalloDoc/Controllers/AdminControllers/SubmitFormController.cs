using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

using Rotativa.AspNetCore;
using ViewAsPdf = Rotativa.AspNetCore.ViewAsPdf;

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
        public async Task<IActionResult> CloseCase(string? id)
        {
            TempData["Status"] = TempData["Status"];
            ViewDocuments v = await _viewActionRepository.GetDocumentByRequest(id.Decode());
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

        #region Conclude_Care
        public async Task<IActionResult> ConcludeCare(string? id)
        {
            TempData["Status"] = TempData["Status"];
            ViewDocuments v = await _viewActionRepository.GetDocumentByRequest(id.Decode());
            return View("../AdminViews/ViewAction/ConcludeCare", v);
        }

        #region Conclude_Care_Changge
        public async Task<IActionResult> ConcludeCareChangge(int id)
        {
            ViewEncounter v = _viewActionRepository.GetEncounterDetailsByRequestID(id);
            if (v.Isfinalize == false)
            {
                TempData["Status"] = "Encounter Form Is Not Finalize !";
                return View("../AdminViews/ViewAction/ConcludeCare", v);
            }

            if (await _viewActionRepository.CloseCase(id))
            {

                TempData["Status"] = "Close Request Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region ChangeNotes

        [HttpPost]
        public async Task<IActionResult> ChangeNotes(int? RequestID, string? adminnotes, string? physiciannotes)
        {
            ViewEncounter v = _viewActionRepository.GetEncounterDetailsByRequestID((int)RequestID);
            if (v.Isfinalize == null || v.Isfinalize == false)
            {
                TempData["Status"] = "Form Is Not Finalize";
                return RedirectToAction("ConcludeCare", new { id = RequestID.Encode() });
            }
                bool result = _viewNotesRepository.PutNotes(adminnotes, physiciannotes, RequestID, CV.ID());

                if ( await _viewActionRepository.CancelCaseByProvider((int)RequestID))
                {
                    TempData["Status"] = "Change Successfully..!";
                    return Redirect("~/Physician/DashBoard");
                }
                else
                {
                    TempData["Status"] = "Not Close State";
                    return RedirectToAction("ConcludeCare", new { id = RequestID.Encode() });
                }
           

        }

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

            return RedirectToAction("CloseCase", new { id = viewdocuments.RequestID.Encode() });
        }
        #endregion

        #region Encounter

        #region Encounter_View
        public async Task<IActionResult> Encounter(string id)
        {

            ViewEncounter v =  _viewActionRepository.GetEncounterDetailsByRequestID((int)id.Decode());
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

            return RedirectToAction("Encounter", new { id = model.Requesid.Encode() });

        }

        #region ACTION-Conculde
        public IActionResult Conculde(int requestId, int providerId)
        {
                bool final = _viewActionRepository.Conculde(requestId, providerId);
                if (final)
                {
                    TempData["Status"] = "Case Is Conculde";
                    if (CV.role() == "Provider")
                    {
                        return Redirect("~/Physician/DashBoard");
                    }
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    TempData["Status"] = "Case Is not Finalized";
                return Redirect("~/Physician/DashBoard");
            }


        }
        #endregion

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
                    if (CV.role() == "Provider")
                    {
                        return Redirect("~/Physician/DashBoard");
                    }
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    TempData["Status"] = "Case Is not Finalized";
                    return View("../AdminViews/ViewAction/Encounter", model);
                }

            } 
            else
            {
                TempData["Status"] = "Case Is not Finalized";
                return View("../AdminViews/ViewAction/Encounter", model);
            }
             
        }
        #endregion

        public IActionResult generatePDF(string id)
        {
            var FormDetails = _viewActionRepository.GetEncounterDetailsByRequestID((int)id.Decode());
            return new ViewAsPdf("../AdminViews/ViewAction/EncounterPdf", FormDetails);
        }

        #endregion

    }
}
