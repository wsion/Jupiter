using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.DataModel
{
    public class ImportSetting
    {
        public string Source { get; set; }
        public string DataLoadTableName { get; set; }
        public string TargetTableName { get; set; }
        public string Columns { get; set; }
        public string Directory { get; set; }
        public string ArchiveFolder { get; set; }
        public string FilePattern { get; set; }
    }
}
