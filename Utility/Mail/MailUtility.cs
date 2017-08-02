using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Jupiter.Utility
{
    public class MailUtility
    {
        private MailConfig config;

        public MailUtility()
        {
            config = XmlUtility.DeserializeFromFile<MailConfig>("mail.xml");
        }

        public SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Host = config.Host;
            client.Port = config.Port;

            if (config.RequireCredentials)
            {
                client.Credentials = new System.Net.NetworkCredential(config.UserName, config.Password);
            }
            return client;
        }

        public void SendEmail(string recipients, string subject, string body)
        {
            SmtpClient client = GetSmtpClient();

            using (MailMessage mm = new MailMessage(config.Sender, recipients))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                client.Send(mm);
            }
        }
    }
}
