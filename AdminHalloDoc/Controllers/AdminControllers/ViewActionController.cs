using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Twilio.Types;
using System.Diagnostics.Metrics;
using AdminHalloDoc.Models.CV;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ViewActionController : Controller
    {
        #region Constructor
        private readonly IRequestRepository _requestRepository;
        private readonly IViewActionRepository _viewActionRepository;
        public ViewActionController(IRequestRepository requestRepository, IViewActionRepository viewActionRepository)
        {

            _requestRepository = requestRepository;
            _viewActionRepository = viewActionRepository;
        }
        #endregion

        #region Save_Viewcase
        public async Task<IActionResult> SaveViewcase(Viewcase viewcase)
        {
           
            if (await _requestRepository.PutViewcase(viewcase))
            {

                TempData["Status"] = "Update Data Successfully..!";
            }
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return RedirectToAction("Viewcase", "AdminDashboard", new { id = viewcase.RequesClientid.Encode()});
        }
        #endregion

        #region Send_Link
        public async Task<IActionResult> SendLink(string firstname,string lastname, string email, string phonenumber)
        {

            if (await _viewActionRepository.SendLink( firstname,  lastname,  email,  phonenumber))
            {
                
                TempData["Status"] = "Link Send In mail Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard"); 
        }
        #endregion

        #region UploadDoc_Files
        public IActionResult UploadDoc(int? Requestid, IFormFile file)
        {

            if (_viewActionRepository.SaveDoc((int)Requestid, file))
            {

                TempData["Status"] = "Upload File Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid.Encode() });
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

        #region _EncounterPdf
        public async Task<IActionResult> _EncounterPdf(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            return PartialView("../AdminViews/ViewAction/_models/_encounterpdf", v);
        }
        #endregion

        #region _EncounterModel
        public async Task<IActionResult> _EncounterModel(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/ViewAction/_models/_encountermodel", v);
        }


        #region _EncounterModelPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EncounterModelPost(ViewActions v, string encounter)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            v.EncounterState = Convert.ToInt32(encounter);

            if (await _viewActionRepository.EncounterModel(v))
            {
                TempData["Status"] = "Accept Request Successfully..!";
            }

            return Redirect("~/Physician/DashBoard");
        }
        #endregion

        #endregion

        #region _AcceptRequestProvider
        public async Task<IActionResult> _AcceptRequest(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/ViewAction/_models/_acceptrequest", v);
        }


        #region _AcceptRequestProviderPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AcceptRequestPost(ViewActions v)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.AcceptPhysician(v))
            {
                TempData["Status"] = "Accept Request Successfully..!";
            }

            return Redirect("~/Physician/DashBoaswsrd");
        }
        #endregion

        #endregion

        #region _TransfertoAdmin
        public async Task<IActionResult> _TransfertoAdmin(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/ViewAction/_models/_transfertoadmin", v);
        }


        #region __TransfertoAdminPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _TransfertoAdminPost(ViewActions v)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.TransfertoAdmin(v))
            {
                TempData["Status"] = "Accept Request Successfully..!";
            }

            return Redirect("~/Physician/DashBoard");
        }
        #endregion

        #endregion

        #region _TransferToProvider
        public async Task<IActionResult> _TransferToProvider(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/ViewAction/_modelS/_transferrequest", v);
        }
        

        #region _TransferToProviderPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _TransferToProviderPost(ViewActions v)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.TransferToProvider(v))
            {
                TempData["Status"] = "Transfer Provider Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region _Cancelcase
        public async Task<IActionResult> _Cancelcase(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            return PartialView("../AdminViews/ViewAction/_modelS/_cancelcase",v);
        }

        #region _CaseReasonPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CaseReasonPost(ViewActions v,string ReasonTag)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.CancelCase(v, ReasonTag))
            {
                TempData["Status"] = "Cancel Request Provider Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region _ClearCase
        
        public async Task<IActionResult> _ClearCase(int RequestId)
        {
            if (await _viewActionRepository.ClearCase(RequestId))
            {
                TempData["Status"] = "Clear Request Successfully..!";
            }
            return Json(true);
        }
        #endregion

        #region _Blockcase
        public async Task<IActionResult> _Blockcase(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.CaseReasonComboBox = await _requestRepository.CaseReasonComboBox();
            return PartialView("../AdminViews/ViewAction/_modelS/_blockcase", v);
        }
       

        #region _BlockcasePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _BlockcasePost(ViewActions v)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.BlockCase(v))
            {
                TempData["Status"] = "Block  Request  Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region _AssignPhysician
        public async Task<IActionResult> _AssignPhysician(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return PartialView("../AdminViews/ViewAction/_modelS/_assign_case", v);
        }
        

        #region _AssignPhysicianPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AssignPhysicianPost(ViewActions v)
        {
            v.AdminId = Convert.ToInt32(CV.UserID());
            if (await _viewActionRepository.AssignPhysician(v))
            {
                TempData["Status"] = "Assign  Physician  Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

        #endregion

        #region ViewOrder
        public async Task<IActionResult> ViewOrder(int? id)
        {
            ViewBag.VenderTypeComboBox = await _requestRepository.VenderTypeComboBox();
            return View("../AdminViews/ViewAction/ViewOrder");
        }
        #endregion

        #region DeleteOnesFile
        public async Task<IActionResult> DeleteFile(int? id,int? Requestid)
        {
            if (await _viewActionRepository.DeleteDocumentByRequest(id.ToString()))
            {

                TempData["Status"] = "Delete File Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid.Encode() });
        }
        #endregion

        #region AllFilesDelete
        public async Task<IActionResult> AllFilesDelete(string deleteids, int? Requestid)
        {
            if (await _viewActionRepository.DeleteDocumentByRequest(deleteids))
            {

                TempData["Status"] = "Delete File Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid.Encode() });
        }
        #endregion

        #region SendFilEmail
        public async Task<IActionResult> SendFilEmail(string mailids, int? Requestid, string email)
        {
            if(await _viewActionRepository.SendFilEmail(mailids, (int)Requestid , email))
            {

                TempData["Status"] = "Send File in Mail Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid.Encode() });
        }
        #endregion

        #region ViewAdminCreateRequest
        public async Task<IActionResult> ViewAdminCreateRequest()
        {
            ViewBag.RegionComboBox = await _requestRepository.RegionComboBox();
            return View("../AdminViews/ViewAction/CreateRequest");
        }
        #endregion

        #region ViewAdminCreateRequest_Post
        public async Task<IActionResult> ViewAdminCreateRequestPost(ViewAdminCreateRequest vr)
        {
            if (CV.role() == "Admin")
            {
                if (_viewActionRepository.SubmitCreateRequest(vr, CV.ID(),null))
                {
                    TempData["Status"] = "Add  Request  Successfully..!";
                }
                return RedirectToAction("Index", "AdminDashboard");
            }
            else
            {
                if (_viewActionRepository.SubmitCreateRequest(vr, CV.ID(),Convert.ToInt32(CV.UserID())))
                {
                    TempData["Status"] = "Add  Request  Successfully..!";
                }
                return Redirect("/Physician/DashBoard");
            }
           
            
        }
        #endregion

        #region UploadDocFromConculde_care
        [HttpPost]
        public async Task<IActionResult> UploadDocFromConculde(int? Requestid, IFormFile file)
        {
            if (_viewActionRepository.SaveDoc((int)Requestid, file))
            {

                TempData["Status"] = "Upload File Successfully..!";
            }
            return RedirectToAction("ConcludeCare", "SubmitForm", new { id = Requestid.Encode() });
        }
        #endregion

        #region SendEmail_ForRequestSupport
        [HttpPost]
        public async Task<IActionResult> SendEmailForRequestSupport(string? Notes)
        {
            if (await _viewActionRepository.SendEmailForRequestSupport(Notes, Convert.ToInt32( CV.UserID())))
            {

                TempData["Status"] = "Msg Sent Successfully..!";
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion


        #region SendEmail_Provider_To_Admin
        public async Task<IActionResult> EditPhysicianMyProfilerequestAsync(int Physicianid, string notes, string Email, string Firstname)
        {
            // _providersRepository.EditPhysicianMyProfile(model, CV.AspNetUserID());
            var Subject = "Edit Profile";
            var Body = "<html><body> " + notes + " </body></html>"; ;
            var to = "tatva.dotnet.niyatikaneriya@outlook.com";

            Emaillogdata elog = new Emaillogdata();
            elog.Emailtemplate = Body;
            elog.Subjectname = " for your request";
            elog.Emailid = Email;
            elog.Createdate = DateTime.Now;
            elog.Sentdate = DateTime.Now;
            elog.Physicianid = Physicianid;
            elog.Action = 11;
            elog.Recipient = Firstname;
            elog.Roleid = 3;
            elog.Senttries = 1;

            await _requestRepository.EmailLog(elog);

            return Redirect("~/Physician/Profile");
        }
        #endregion
    }
}
