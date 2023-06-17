﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
//using MailKit.Net.Smtp;
//using MimeKit;


namespace ReProServices.Infrastructure.Smtp
{
    public class EmailHelper
    {
        private string _host;
        private string _from;
        private string _alias;
        private string _password;
        private string _user;
        private int _port;
        public EmailHelper(IConfiguration iConfiguration)
        {
            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
                _user = smtpSection.GetSection("User").Value;
                _alias = smtpSection.GetSection("Alias").Value;
                _password = smtpSection.GetSection("Password").Value;
                _port = Convert.ToInt32(smtpSection.GetSection("port").Value);
            }
        }

        private void ValidateCredentials()
        {
            if (string.IsNullOrEmpty(_host))
                _host = "smtp.zeptomail.in";

            if (_port == 0)
                _port = 587;

            if (string.IsNullOrEmpty(_from))
                _from = "tdscompliance@reproservices.in";

            if (string.IsNullOrEmpty(_user))
                _user = "emailapikey";

            if (string.IsNullOrEmpty(_password))
                _password = "PHtE6r0NF+/jjGN8pBQD5PW6FsT3Y40s/uw0KFMWttlCXqIKTE1WrdotkT+x/U9/A/NAFqSTyoM6ue6Y4OOELD3oPGpOXGqyqK3sx/VYSPOZsbq6x00ft1QbdEHVUoHsctVv1iXXutzTNA==";
         
        }

        public bool SendEmail(EmailModel emailModel)
        {
            try
            {
                ValidateCredentials();
                // using (SmtpClient client = new SmtpClient(_host,8889))
                // using (var clients = new MailKit.Net.Smtp.SmtpClient())
                using (SmtpClient client = new SmtpClient("smtp.zeptomail.in", 587))
                {
                    NetworkCredential NetCrd = new NetworkCredential("tdscompliance@reproservices.in", "PHtE6r0NF+/jjGN8pBQD5PW6FsT3Y40s/uw0KFMWttlCXqIKTE1WrdotkT+x/U9/A/NAFqSTyoM6ue6Y4OOELD3oPGpOXGqyqK3sx/VYSPOZsbq6x00ft1QbdEHVUoHsctVv1iXXutzTNA==");
                    MailMessage mailMessage = new MailMessage();
                    // mailMessage.From = new MailAddress(_from, _alias);

                    //var imageResource = new LinkedResource(new MemoryStream(emailModel.FileBlob), "image/png") { ContentId = "added-image-id" };
                    //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailModel.Message, null, "text/html");
                    //htmlView.LinkedResources.Add(imageResource);
                    //mailMessage.AlternateViews.Add(htmlView);
                    mailMessage.From = new MailAddress("tdscompliance@reproservices.in", _alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);
                    mailMessage.Body = emailModel.Message;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;

                    if (emailModel.attachments != null)
                    {
                        foreach (var attachment in emailModel.attachments)
                        {
                            MemoryStream stream = new MemoryStream(attachment.MemoryStream);
                            mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.FileType));
                        }
                    }
                    client.UseDefaultCredentials = false;
                    client.Credentials = NetCrd;
                    client.EnableSsl = true; // Node :this should be enabled for live service
                    client.Send(mailMessage);
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

        public bool SendEmail(EmailModel emailModel, LinkedResource res)
        {
            try
            {
                ValidateCredentials();

                if (!string.IsNullOrEmpty(emailModel.From))
                {
                    _from = emailModel.From;
                }
                //  using (SmtpClient client = new SmtpClient(_host, _port))
                // using (var client = new MailKit.Net.Smtp.SmtpClient())
                using (SmtpClient client = new SmtpClient("smtp.zeptomail.in", 587))
                {
                    NetworkCredential NetCrd = new NetworkCredential("tdscompliance@reproservices.in", "PHtE6r0NF+/jjGN8pBQD5PW6FsT3Y40s/uw0KFMWttlCXqIKTE1WrdotkT+x/U9/A/NAFqSTyoM6ue6Y4OOELD3oPGpOXGqyqK3sx/VYSPOZsbq6x00ft1QbdEHVUoHsctVv1iXXutzTNA==");
                    MailMessage mailMessage = new MailMessage();
                    // mailMessage.From = new MailAddress(_from, _alias);

                    //var imageResource = new LinkedResource(new MemoryStream(emailModel.FileBlob), "image/png") { ContentId = "added-image-id" };
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailModel.Message, null, "text/html");
                    htmlView.LinkedResources.Add(res);
                    mailMessage.AlternateViews.Add(htmlView);

                    mailMessage.From = new MailAddress("tdscompliance@reproservices.in", _alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);

                    if (!string.IsNullOrEmpty(emailModel.CC))
                    {
                        var cclist = emailModel.CC.Split(',');
                        foreach (var cc in cclist)
                        {
                            if (!string.IsNullOrEmpty(cc))
                            {
                                MailAddress copy = new MailAddress(cc);
                                mailMessage.CC.Add(copy);
                            }
                        }
                    }
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;

                    if (emailModel.attachments != null)
                    {
                        foreach (var attachment in emailModel.attachments)
                        {
                            MemoryStream stream = new MemoryStream(attachment.MemoryStream);
                            mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.FileType));
                        }
                    }
                    client.UseDefaultCredentials = false;
                    client.Credentials = NetCrd;
                    client.EnableSsl = true; // Node :this should be enabled for live service

                    client.Send(mailMessage);
                    return true;

                    //var clients = new MailKit.Net.Smtp.SmtpClient();
                    //var message = new MimeMessage();
                    //message.From.Add(new MailboxAddress("noreply", "tdscompliance@reproservices.in"));
                    //message.To.Add(new MailboxAddress("REpro", "karthi@leansys.in"));
                    //message.Subject = "Test Email";
                    //message.Body = new TextPart("html")
                    //{
                    //    Text = "Test email sent successfully."
                    //};

                    //try
                    //{
                    //    clients.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    //    clients.Connect("smtp.zeptomail.in", 587, false);
                    //    clients.Authenticate("emailapikey", "PHtE6r0EROrp3zV89RcGtvDrRcXxPIMp+ekyJAkS5okWD/QDGU1XrNh9wz+/rB8uU/ZAEv6amtk9sLicsOiGcW25NjkeX2qyqK3sx/VYSPOZsbq6x00euFwTfk3bUY7qdt5o1C3Xud7aNA==");
                    //   // clients.Authenticate(NetCrd);
                    //    clients.Send(message);
                    //    clients.Disconnect(true);
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.Write(e.Message);
                    //}
                    //return true;
                }
            }
            catch (Exception e)
            {
                //throw; //todo log error message
                //return false;
                throw new ApplicationException(e.Message);
            }

        }
    }
}
