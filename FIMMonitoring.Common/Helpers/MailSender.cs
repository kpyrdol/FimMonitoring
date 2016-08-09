using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace FIMMonitoring.Common.Helpers
{
    public class MailSender
    {
        public string MailFrom { get; set; }
        public string MailFromDisplayName { get; set; }
        public string MailFromPassword { get; set; }
        public string MailHost { get; set; }
        public int MailPort { get; set; }
        public bool MailSsl { get; set; }

        public void Send(MailViewModel mail)
        {
            var fromAddress = new MailAddress(MailFrom, MailFromDisplayName);
            var toAddress = new List<MailAddress>();
            if (!string.IsNullOrWhiteSpace(mail.To))
                mail.To.Split(',', ';').ToList().ForEach(p => toAddress.Add(new MailAddress(p.Trim())));
            var toCC = new List<MailAddress>();
            if (!string.IsNullOrWhiteSpace(mail.CC))
                mail.CC.Split(',', ';').ToList().ForEach(p => toCC.Add(new MailAddress(p.Trim())));

            var fromPassword = MailFromPassword;

            var smtp = new SmtpClient
            {
                Host = MailHost,
                Port = MailPort,
                EnableSsl = MailSsl,
                DeliveryFormat = SmtpDeliveryFormat.International,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = MailSsl,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };


            using (var message = new MailMessage
            {
                From = fromAddress,
                Subject = mail.Subject,
                Body = mail.Body,
                IsBodyHtml = true
            })
            {
                toCC.ForEach(p => message.CC.Add(p));
                toAddress.ForEach(p => message.To.Add(p));

                smtp.Send(message);
            }

        }
    }
}
