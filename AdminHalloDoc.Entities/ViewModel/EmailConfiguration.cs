
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Net;
using System.Text;
//using System.Net.Mail;
//using System.Net;
//using Microsoft.AspNetCore.Http;

namespace AdminHalloDoc.Entities.ViewModel
{

    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        #region SendMail
        /// <summary>
        /// Only For Pass The Msg And Subject To Admin Or Provider Or User
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public async Task<bool> SendMail(String To, String Subject, String Body)
        {
            //return true;
            try
            {
                Body = "Mail to :"+ To+"<br/>"+Body;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", From));
                message.To.Add(new MailboxAddress("", "dasete8625@haislot.com"));
                message.Subject = Subject;
                message.Body = new TextPart("html")
                {
                    Text = Body
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(SmtpServer, Port, false);
                    await client.AuthenticateAsync(UserName, Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region SendMail_Shift
        /// <summary>
        /// Pass Body With Shift Detalis And Associated With google Cal
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public async Task<bool> SendMailWithShift(String To, String Subject, String Body,DateTime StartDate  , DateTime EndDate)
        {
            try
            {
                Body = "Mail to: " + To + "<br/>" + Body;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", From));
                message.To.Add(new MailboxAddress("", "shreyanspadmani.me@gmail.com"));
                message.Subject = Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = Body;

                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");
                str.AppendLine("PRODID:-//GeO");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");
                str.AppendLine("BEGIN:VEVENT");
                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", StartDate));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", EndDate));
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:{0}", Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", Body));
                str.AppendLine(string.Format("SUMMARY;ENCODING=QUOTED-PRINTABLE:{0}", Subject));
                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");

                System.Net.Mime.ContentType type = new System.Net.Mime.ContentType("text/calendar");
                type.Parameters.Add("method", "REQUEST");
                type.Parameters.Add("name", "shift.ics");

                var calendarAttachment = new MimePart()
                {

                    Content = new MimeContent(new MemoryStream(Encoding.UTF8.GetBytes(str.ToString()))),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "shift.ics"
                };
                var contentType = new ContentType(type.MediaType,type.MediaType);
                contentType.Parameters.Add("name", "shift.ics");
                builder.Attachments.Add(calendarAttachment);
                message.Body = builder.ToMessageBody();

                // Send message using MailKit
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(SmtpServer, Port, false);
                    await client.AuthenticateAsync(UserName, Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        //#region SendMail
        //public Boolean SendMail(String To, String Subject, String Body)
        //{
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        (sender, certificate, chain, sslPolicyErrors) => true;

        //    //send mail
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(From);
        //    message.Subject = Subject;
        //    message.To.Add(new MailAddress(To));
        //    message.Body = Body;
        //    message.IsBodyHtml = true;

        //    if ("C:\\Users\\pca176\\Documents\\AdminHalloDoc\\AdminHalloDoc\\wwwroot\\Upload\\60\\htmltable (41)-20240228061436.xls" != null)
        //    {
        //        message.Attachments.Add(new Attachment("C:\\Users\\pca176\\Documents\\AdminHalloDoc\\AdminHalloDoc\\wwwroot\\Upload\\60\\htmltable (41)-20240228061436.xls"));
        //    }
        //    //if (Attachments != null)
        //    //{
        //    //    foreach (IFormFile attachment in Attachments)
        //    //    {
        //    //        string fileName = Path.GetFileName(attachment.FileName);
        //    //        message.Attachments.Add(new Attachment(attachment.OpenReadStream(), fileName));
        //    //    }
        //    //}
        //    message.Body = Body + message.Attachments.ToString();

        //    using (var smtpClient = new SmtpClient(SmtpServer))
        //    {
        //        smtpClient.Port = Port;
        //        smtpClient.Credentials = new NetworkCredential(UserName, Password);
        //        smtpClient.EnableSsl = true;

        //        smtpClient.Send(message);
        //    }
        //    return true;
        //}
        //#endregion

        #region SendMail_Attachments
        /// <summary>
        /// Pass Body With Attachments Multiple Attachments
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="Attachments"></param>
        /// <returns></returns>
        public async Task<bool> SendMailAsync(string To, string Subject, string Body, List<string> Attachments)
        {
            //return true;
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("", From));
            message.To.Add(new MailboxAddress("", "dasete8625@haislot.com"));
            message.Subject = Subject;

            // Create the multipart/mixed container to hold the message body and attachments
            var multipart = new Multipart("mixed");

            // Create HTML body part
            var bodyPart = new TextPart("html")
            {
                Text = Body
            };
            multipart.Add(bodyPart);

            if (Attachments != null)
            {
                foreach (string attachmentPath in Attachments)
                {
                    if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
                    {
                        // Create MimePart for attachment
                        var attachment = new MimePart()
                        {
                            Content = new MimeContent(File.OpenRead(attachmentPath), ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Path.GetFileName(attachmentPath)
                        };

                        // Add attachment to multipart container
                        multipart.Add(attachment);
                    }
                }
            }

            // Set the message body to the multipart container
            message.Body = multipart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, Port, false);
                await client.AuthenticateAsync(UserName, Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }

        #endregion

        #region Encode_Decode
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }
        public string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
        #endregion
    }

}
