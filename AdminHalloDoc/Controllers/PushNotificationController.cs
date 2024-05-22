
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebPush;

namespace AdminHalloDoc.Controllers
{
    public class PushNotificationController : Controller
    {
        #region Constructor
        private readonly IPushNotificationRepository _pushNotificationRepository;
        private readonly IConfiguration Configuration;

        public PushNotificationController(IConfiguration Configuration, IPushNotificationRepository pushNotificationRepository)
        {
            this.Configuration = Configuration;
            _pushNotificationRepository = pushNotificationRepository;
        }
        #endregion
        //This method is called in dashboard page

        /// <summary>
        /// Post method - to get and store user for sending the post notification 
        /// </summary>
        /// <param name="client">Unique UserName - Which will be stored with notification credentials</param>
        /// <param name="endpoint">Value to send a push notification</param>
        /// <param name="p256dh">Value to identify the user to send a notification</param>
        /// <param name="auth">Value to indentify the user to send a notification</param>
        /// <returns>Store the Push Notification User data</returns>
        [HttpPost,Route("pushnotification/index",Name ="Pushnotification_Post")]
        public IActionResult Index(string endpoint, string p256dh, string auth)
        {
           string client = CV.ID();
            if (client == null)
            {
                return BadRequest("No Client Name parsed.");
            }
            var subscription = new PushSubscription(endpoint, p256dh, auth);
            _pushNotificationRepository.AddNotificationUserData(client, endpoint, p256dh, auth);
            return RedirectToRoute("Dashboard");
        }


        /// <summary>
        /// Send a notification post method
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="client">UserName if you want to send a notification to a specific user</param>
        /// <param name="endPoint">Specified user's endpoint</param>
        /// <param name="p256dh">Specified user's p256dh</param>
        /// <param name="auth">Specified user's auth</param>
        /// <returns></returns>
        [HttpPost, Route("pushnotification/sendnotification", Name = "Send_Notification")]
        public IActionResult Notify(string message, string aspid)
        {
            PushNotification PushNotification = _pushNotificationRepository.GetUserData(aspid);

            if (PushNotification != null)
            {
                PushSubscription subscription = new PushSubscription(PushNotification.EndPoint, PushNotification.P256dh, PushNotification.Auth);
                if (subscription == null)
                {
                    return BadRequest("Client was not found");
                }

                var subject = Configuration["Vapid:subject"];
                var publicKey = Configuration["Vapid:publicKey"];
                var privateKey = Configuration["Vapid:privateKey"];

                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var payload = JsonConvert.SerializeObject(new
                {
                    notification = new
                    {
                        title = "Hi ",//Notification title
                        body = message,
                        customData = new
                        {
                            url = "https://www.google.com",//Redirect url which will be called while click on the notification
                        }
                    }
                });

                var webPushClient = new WebPushClient();
                try
                {
                    var options = new Dictionary<string, object>
                    {
                        ["vapidDetails"] = vapidDetails
                    };
                    webPushClient.SendNotification(subscription, payload, options);//notification send method
                }
                catch (Exception exception)
                {
                    // Log error
                }
            }

            
          
            return Json(null);
        }
    }
}
