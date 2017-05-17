using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Screeps.Notify
{
    public class SendMail
    {
        public string SmtpServer { get; set; }
        public MailAddress Sender { get; set; }
        public List<string> Recipient { get; set; }
        public bool EnableSsl { get; set; }
        private SmtpClient smtp = null;

        public SendMail(string smtpServer, string sender, string senderDisplayName = null)
        {
            Sender = new MailAddress(sender, senderDisplayName);
            Recipient = new List<string>();
            SmtpServer = smtpServer;
            smtp = new SmtpClient(SmtpServer);
            smtp.EnableSsl = EnableSsl;
            if (EnableSsl) smtp.Port = 443;
        }
        public SendMail(string smtpServer, string smtpUsername, string smtpPassword, string sender, string senderDisplayName = null) : 
            this(smtpServer, sender, senderDisplayName)
        {
            smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        }

        public bool Send(string subject, string message)
        {
            if (string.IsNullOrEmpty(SmtpServer) || Sender == null || Recipient == null || Recipient.Count == 0)
                return false;

            var mail = new MailMessage();
            mail.From = Sender;
            mail.Subject = subject;
            mail.IsBodyHtml = false;
            mail.Body = message;

            foreach (string recepient in Recipient)
            {
                mail.To.Add(recepient);
            }

            smtp.Send(mail);

            return true;
        }
    }
}
