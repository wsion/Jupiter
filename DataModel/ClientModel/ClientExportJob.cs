using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jupiter.DataModel
{
    public class ClientExportJob
    {
        public int ID { get; set; }
        public string Source { get; set; }
        public string SourceName { get; set; }
        public string Prefix { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
    }

    [Serializable, XmlRoot("Jobs")]
    public class ClientExportJobs
    {
        [XmlElement("Job")]
        public List<ClientExportJob> Items { get; set; }
    }
}
