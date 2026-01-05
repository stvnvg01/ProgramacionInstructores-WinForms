// EmailSender.cs
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProgramacionInstructores
{
    public class EmailSender
    {
        private readonly EmailSettings _cfg;
        public EmailSender(EmailSettings cfg) { _cfg = cfg; }

        // Opción A: síncrono (.NET 4.5 100% estable)
        public void Send(string to, string subject, string htmlBody, string attachmentPath = null)
        {
            using (var msg = BuildMessage(to, subject, htmlBody, attachmentPath))
            using (var smtp = BuildClient())
            {
                smtp.Send(msg);
            }
        }

        // Opción B: asíncrono (si lo prefieres con Task)
        public Task SendAsync(string to, string subject, string htmlBody, string attachmentPath = null)
        {
            return Task.Run(() =>
            {
                using (var msg = BuildMessage(to, subject, htmlBody, attachmentPath))
                using (var smtp = BuildClient())
                {
                    smtp.Send(msg); // síncrono dentro de Task
                }
            });
        }

        private MailMessage BuildMessage(string to, string subject, string htmlBody, string attachmentPath)
        {
            var msg = new MailMessage();
            msg.From = new MailAddress(
                _cfg.FromAddress,
                string.IsNullOrWhiteSpace(_cfg.FromDisplay) ? _cfg.FromAddress : _cfg.FromDisplay
            );
            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = htmlBody;
            msg.IsBodyHtml = true;
            if (!string.IsNullOrWhiteSpace(attachmentPath))
                msg.Attachments.Add(new Attachment(attachmentPath));
            return msg;
        }

        private SmtpClient BuildClient()
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_cfg.FromAddress, _cfg.AppPassword);
            return smtp;
        }
    }
}
