using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AdminHalloDoc.Controllers
{
    public class SendSmsController : Controller
    {
        private readonly SmsConfiguration _smsConfiguration;
        public SendSmsController(SmsConfiguration smsConfiguration) { 
            _smsConfiguration = smsConfiguration;
        }
        public IActionResult Index()
        {
            return View();  
        }
        public IActionResult SendSMS()
        {
            string sId = ""; // add Account from Twilio
            string authToken = ""; //add Auth Token from Twilio
            string fromPhoneNumber = "+120*******"; //add Twilio phone number

            TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);
            var message = MessageResource.Create(
                body: "Hi, there i am shreyans!!",
                from: new Twilio.Types.PhoneNumber(_smsConfiguration.Phonenumber),
                to: new Twilio.Types.PhoneNumber("+919537290206") //add receiver's phone number
            );
            Console.WriteLine(message.ErrorCode);
            return View(message);
        }
    }
}
