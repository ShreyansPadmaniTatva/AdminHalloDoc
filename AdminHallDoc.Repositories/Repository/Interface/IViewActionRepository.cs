﻿using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using Microsoft.AspNetCore.Http;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IViewActionRepository
    {
        public bool Conculde(int Requesid, int id);
        Task<Boolean> SendLink(string firstname, string lastname, string email, string phonenumber);
        public Task<ViewDocuments> GetDocumentByRequest(int? id);
        public Boolean SaveDoc(int Requestid, IFormFile file);
        public Task<List<Physician>> ProviderbyRegion(int? regionid);
        Task<Boolean> AssignProvider(int RequestId, int ProviderId, string notes);
        Task<ViewActions> GetRequestDetails(int? id);
        Task<bool> TransferToProvider(ViewActions v);
        Task<bool> CancelCase(ViewActions v, string ReasonTag);
        Task<bool> CancelCaseByProvider(int RequestID);
        Task<bool> BlockCase(ViewActions v);
        Task<bool> AcceptPhysician(ViewActions v);
        Task<bool> AssignPhysician(ViewActions v);
        Task<bool> EncounterModel(ViewActions v);
        Task<bool> TransfertoAdmin(ViewActions v);
        Task<bool> DeleteDocumentByRequest(string ids);
        Task<bool> SendFilEmail(string ids, int Requestid, string SendFilEmail);
        Task<Boolean> SendAgreement(ViewActions v);
        Boolean SendAgreement_accept(int RequestID);
        Boolean SendAgreement_Reject(int RequestID, string Notes);
        Task<bool> ClearCase(int RequestID);
        Task<bool> CloseCase(int RequestID);
        ViewEncounter GetEncounterDetailsByRequestID(int RequestID);
        bool EditEncounterDetails(ViewEncounter Data, string id);
        bool CaseFinalized(ViewEncounter model, string id);
        bool SubmitCreateRequest(ViewAdminCreateRequest model, string Id, int? UserId);
        Task<bool> SendEmailForRequestSupport(string notes, int AdminId);
    }
}
