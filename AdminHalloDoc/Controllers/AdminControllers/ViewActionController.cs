﻿using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

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

            if (_viewActionRepository.SaveDoc(Requestid, file))
            {

                TempData["Status"] = "Upload File Successfully..!";
            }
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
        public async Task<IActionResult> _CaseReasonPost(ViewActions v)
        {
            if (await _viewActionRepository.CancelCase(v))
            {
                TempData["Status"] = "Cancel Request Provider Successfully..!";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }
        #endregion

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
        public async Task<IActionResult> DeleteFile(int? id,int Requestid)
        {
            if (await _viewActionRepository.DeleteDocumentByRequest(id.ToString()))
            {

                TempData["Status"] = "Delete File Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid });
        }
        #endregion

        #region AllFilesDelete
        public async Task<IActionResult> AllFilesDelete(string deleteids, int Requestid)
        {
            if (await _viewActionRepository.DeleteDocumentByRequest(deleteids))
            {

                TempData["Status"] = "Delete File Successfully..!";
            }
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid });
        }
        #endregion


        #region SendFilEmail
        public async Task<IActionResult> SendFilEmail(string deleteids, int Requestid)
        {
            await _viewActionRepository.SendFilEmail(deleteids);
            return RedirectToAction("ViewUpload", "AdminDashboard", new { id = Requestid });
        }
        #endregion
    }
}
