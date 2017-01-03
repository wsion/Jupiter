using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jupiter.Utility
{
    [Serializable, XmlRoot("Mail")]
    public class MailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool RequireCredentials { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
    }
}
