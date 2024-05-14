using AdminHalloDoc.Models;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminHalloDoc.ChatHub
{
    public class ChatHubController : Controller
    {
        #region Constructor
        private readonly IChatRepository _chatRepository;
        private readonly IViewActionRepository _viewActionRepository;
        public ChatHubController(IChatRepository chatRepository, IViewActionRepository viewActionRepository)
        {
            _chatRepository = chatRepository;
            _viewActionRepository = viewActionRepository;
        }

        #endregion

        #region _Chatbox
        public async Task<IActionResult> _Chatbox(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            v.ReceiverType = "Provider";

            string s = ConnectedUsers.myConnectedUsers.Where(r => r.SenderAspId == v.PhysicianAspId).FirstOrDefault().ConnectionId;
            if (s != null)
            {
                v.PhysicianConnectionId = ConnectedUsers.myConnectedUsers.Where(r => r.SenderAspId == v.PhysicianAspId).FirstOrDefault().ConnectionId;
            }
            return PartialView("../AdminViews/ViewAction/_modelS/_chatbox", v);
        }
        #endregion

        #region _ChatboxforPatient
        public async Task<IActionResult> _ChatboxforPatient(int? requestid)
        {
            var v = await _viewActionRepository.GetRequestDetails(requestid);
            string s = ConnectedUsers.myConnectedUsers.Where(r => r.SenderAspId == v.PatientAspId).FirstOrDefault().ConnectionId;
            v.ProviderId = v.PatientId;
            v.PhysicianName = v.PatientName;
            v.ReceiverType = "Patient";
            if (s != null)
            {
                //this PhysicianConnectionId work as PatientConnectionId
                v.PhysicianConnectionId = ConnectedUsers.myConnectedUsers.Where(r => r.SenderAspId == v.PatientAspId).FirstOrDefault().ConnectionId;
            }
            return PartialView("../AdminViews/ViewAction/_modelS/_chatbox", v);
        }
        #endregion

    }
}
