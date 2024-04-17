
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Net;
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
        public async Task<bool> SendMail(String To, String Subject, String Body)
        {
            try
            {
                Body = "Mail to :"+ To+"<br/>"+Body;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", From));
                message.To.Add(new MailboxAddress("", "petoco8642@iliken.com"));
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

        #region SendMail
        public async Task<bool> SendMailAsync(string To, string Subject, string Body, List<string> Attachments)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("", From));
            message.To.Add(new MailboxAddress("", "pehek11482@fashlend.com"));
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

        //public async Task<bool> SendMailAsync(String To, String Subject, String Body, List<string> Attachments)
        //{
        //    MimeMessage message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("", From));
        //    message.To.Add(new MailboxAddress("", To));
        //    message.Subject = Subject;
        //    message.Body = new TextPart("html")
        //    {
        //        Text = Body
        //    };



        //    if (Attachments != null)
        //    {
        //        foreach (string attachment in Attachments)
        //        {
        //            if (attachment != null)
        //            {
        //                if (File.Exists(attachment))
        //                {
        //                    // If file found, Send  it
        //                    message.Attachments.Add(new Attachment(attachment));
        //                }
        //            }
        //        }
        //    }

        //    message.Body = Body;

        //    using (var client = new MailKit.Net.Smtp.SmtpClient())
        //    {
        //        await client.ConnectAsync(SmtpServer, Port, false);
        //        await client.AuthenticateAsync(UserName, Password);
        //        await client.SendAsync(message);
        //        await client.DisconnectAsync(true);
        //    }

        //    return true;
        //}

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
