using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jupiter.DataModel
{
    public class ClientImportJob
    {
        public string ApiUrl { get; set; }
        public string JobName { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public string ParameterPrefix { get; set; }
        public string Query { get; set; }
    }

    [Serializable, XmlRoot("Jobs")]
    public class ClientImportJobs
    {
        [XmlElement("ImportJob")]
        public List<ClientImportJob> Items { get; set; }
    }
}
