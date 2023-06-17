using System;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace WebApi.Controllers
{
    public class TestController : ApiController
    {
        [HttpPost("testmail")]
        public ActionResult TestMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("noreply", "tdscompliance@reproservices.in"));
            message.To.Add(new MailboxAddress("REpro", "karthi@leansys.in"));
            message.Subject = "Test Email";
            message.Body = new TextPart("html")
            {
                Text = "Test email sent successfully."
            };
            var client = new SmtpClient();
            try
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.zeptomail.in", 465, false);
                client.Authenticate("tdscompliance@reproservices.in", "PHtE6r1ZQ+3qgjUr8UUItqWwEsOmMY8sq7tkfgVFto0TCvMHTU0Er9l/lDTh/ksrA6JLRfPOzNpo5L6VsL3Rd2m4YW1MCGqyqK3sx/VYSPOZsbq6x00as14dcE3ZUIbsetVp3SXWu9mX");
                client.Send(message);
                client.Disconnect(true);
                return Ok(true);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Ok(true);
        }

        [HttpPost("testmail1")]
        public ActionResult TestMail1()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("noreply", "tdscompliance@reproservices.in"));
            message.To.Add(new MailboxAddress("REpro", "karthi@leansys.in"));
            message.Subject = "Test Email";
            message.Body = new TextPart("html")
            {
                Text = "Test email sent successfully."
            };
            var client = new SmtpClient();
            try
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.zeptomail.in", 587, false);
                client.Authenticate("emailapikey", "PHtE6r1ZQ+3qgjUr8UUItqWwEsOmMY8sq7tkfgVFto0TCvMHTU0Er9l/lDTh/ksrA6JLRfPOzNpo5L6VsL3Rd2m4YW1MCGqyqK3sx/VYSPOZsbq6x00as14dcE3ZUIbsetVp3SXWu9mX");
                client.Send(message);
                client.Disconnect(true);
                return Ok(true);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Ok(true);
        }

        [HttpPost("testmail2")]
        public ActionResult TestMail2()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("noreply", "tdscompliance@reproservices.in"));
            message.To.Add(new MailboxAddress("REpro", "karthi@leansys.in"));
            message.Subject = "Test Email";
            message.Body = new TextPart("html")
            {
                Text = "Test email sent successfully."
            };
            var client = new SmtpClient();
            try
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.zeptomail.in", 587, false);
                client.Authenticate("emailappsmtp.3401b18c79604901", "dxLxrCEK7Pn2");
                client.Send(message);
                client.Disconnect(true);
                return Ok(true);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Ok(true);
        }
    }
}
