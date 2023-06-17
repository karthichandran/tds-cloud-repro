using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

namespace ReProServices.Infrastructure.Smtp
{
    public class NewEmailHelper
    {
        private string _host;
        private string _from;
        private string _alias;
        private string _password;
        private int _port;
        private string _user;
        public NewEmailHelper(IConfiguration iConfiguration)
        {
            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
                _alias = smtpSection.GetSection("Alias").Value;
                _user = smtpSection.GetSection("User").Value;
                _password = smtpSection.GetSection("Password").Value;
                _port = Convert.ToInt32(smtpSection.GetSection("port").Value);
            }
        }

        public bool SendEmail(EmailModel emailModel)
        {
            try
            {
                // using (SmtpClient client = new SmtpClient(_host,8889))
                using (SmtpClient client = new SmtpClient())
                {
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    client.Connect("smtp.zeptomail.in", 465, false);
                    client.Authenticate("tdscompliance@reproservices.in", "PHtE6r1ZQ+3qgjUr8UUItqWwEsOmMY8sq7tkfgVFto0TCvMHTU0Er9l/lDTh/ksrA6JLRfPOzNpo5L6VsL3Rd2m4YW1MCGqyqK3sx/VYSPOZsbq6x00as14dcE3ZUIbsetVp3SXWu9mX");

                    client.Disconnect(true);
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("REpro", _from));
                    message.To.Add(new MailboxAddress("REpro", emailModel.To));
                    message.Subject = emailModel.Subject;
                    //message.Body = new TextPart("html")
                    //{
                    //    Text = emailModel.Message
                    //};

                    var builder = new BodyBuilder();
                    builder.HtmlBody = emailModel.Message;

                    if (emailModel.attachments != null)
                    {
                        foreach (var attachment in emailModel.attachments)
                        {
                            builder.Attachments.Add(attachment.FileName, attachment.MemoryStream, ContentType.Parse(attachment.FileType));
                        }
                    }
                    message.Body = builder.ToMessageBody();
                    client.Send(message);
                    return true;
                }
            }
            catch (Exception e)
            {
                //throw; //todo log error message
                //return false;
                throw new ApplicationException(e.Message);
            }

        }


        public bool SendEmail(EmailModel emailModel, System.Net.Mail.LinkedResource res)
        {

            try
            {
                if (!string.IsNullOrEmpty(emailModel.From))
                {
                    _from = emailModel.From;
                }
                // using (SmtpClient client = new SmtpClient(_host,8889))
                using (SmtpClient client = new SmtpClient())
                {
                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    client.Connect("smtp.zeptomail.in", 465, false);
                    client.Authenticate("tdscompliance@reproservices.in", "PHtE6r1ZQ+3qgjUr8UUItqWwEsOmMY8sq7tkfgVFto0TCvMHTU0Er9l/lDTh/ksrA6JLRfPOzNpo5L6VsL3Rd2m4YW1MCGqyqK3sx/VYSPOZsbq6x00as14dcE3ZUIbsetVp3SXWu9mX");

                    client.Disconnect(true);
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("REpro", _from));
                    message.To.Add(new MailboxAddress("REpro", emailModel.To));
                    message.Subject = emailModel.Subject;
                    //message.Body = new TextPart("html")
                    //{
                    //    Text = emailModel.Message
                    //};

                    var builder = new BodyBuilder();
                    builder.HtmlBody = emailModel.Message;

                    if (emailModel.attachments != null)
                    {
                        foreach (var attachment in emailModel.attachments)
                        {
                            builder.Attachments.Add(attachment.FileName, attachment.MemoryStream, ContentType.Parse(attachment.FileType));
                        }
                    }

                    message.Body = builder.ToMessageBody();
                    client.Send(message);
                    return true;
                }
            }
            catch (Exception e)
            {
                //throw; //todo log error message
                throw new ApplicationException(e.Message);
                //return false;
            }

        }

        //public bool SendEmail(EmailModel emailModel, LinkedResource res)
        //{

        //    try
        //    {
        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("noreply", "noreply@reproservices.in"));
        //        message.To.Add(new MailboxAddress("REpro", "tdscompliance@reproservices.in"));
        //        message.Subject = "Test Email";
        //        message.Body = new TextPart("html")
        //        {
        //            Text = "Test email sent successfully."
        //        };
        //        var client = new MailKit.Net.Smtp.SmtpClient();
        //        try
        //        {
        //            client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        //            client.Connect("smtp.zeptomail.in", 587, true);
        //            client.Authenticate("emailapikey", "PHtE6r0EROrp3zV89RcGtvDrRcXxPIMp+ekyJAkS5okWD/QDGU1XrNh9wz+/rB8uU/ZAEv6amtk9sLicsOiGcW25NjkeX2qyqK3sx/VYSPOZsbq6x00euFwTfk3bUY7qdt5o1C3Xud7aNA==");
        //            client.Send(message);
        //            client.Disconnect(true);
        //        }
        //        catch (Exception e)
        //        {
        //            throw new ApplicationException(e.Message);
        //        }

        //        return false;
        //        //if (!string.IsNullOrEmpty(emailModel.From))
        //        //{
        //        //    _from = emailModel.From;
        //        //}
        //        //// using (SmtpClient client = new SmtpClient(_host,8889))
        //        //using (var client = new MailKit.Net.Smtp.SmtpClient())
        //        //{
        //        //    NetworkCredential NetCrd = new NetworkCredential(_user, _password, "reproservices.in");
        //        //    MailMessage mailMessage = new MailMessage();
        //        //    // mailMessage.From = new MailAddress(_from, _alias);

        //        //    //var imageResource = new LinkedResource(new MemoryStream(emailModel.FileBlob), "image/png") { ContentId = "added-image-id" };
        //        //    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailModel.Message, null, "text/html");
        //        //    htmlView.LinkedResources.Add(res);
        //        //    mailMessage.AlternateViews.Add(htmlView);

        //        //    mailMessage.From = new MailAddress(_from, _alias);
        //        //    mailMessage.BodyEncoding = Encoding.UTF8;
        //        //    mailMessage.To.Add(emailModel.To);

        //        //    if (!string.IsNullOrEmpty(emailModel.CC))
        //        //    {
        //        //        var cclist = emailModel.CC.Split(',');
        //        //        foreach (var cc in cclist)
        //        //        {
        //        //            if (!string.IsNullOrEmpty(cc))
        //        //            {
        //        //                MailAddress copy = new MailAddress(cc);
        //        //                mailMessage.CC.Add(copy);
        //        //            }
        //        //        }
        //        //    }
        //        //    mailMessage.Subject = emailModel.Subject;
        //        //    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;

        //        //    if (emailModel.attachments != null)
        //        //    {
        //        //        foreach (var attachment in emailModel.attachments)
        //        //        {
        //        //            MemoryStream stream = new MemoryStream(attachment.MemoryStream);
        //        //            mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.FileType));
        //        //        }
        //        //    }
        //        //    client.UseDefaultCredentials = false;
        //        //    client.Credentials = NetCrd;
        //        //    client.EnableSsl = true; // Node :this should be enabled for live service
        //        //    client.Send(mailMessage);
        //        //    return true;
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        //throw; //todo log error message
        //        throw new ApplicationException(e.Message);
        //        //return false;
        //    }
        //}



    }
}
