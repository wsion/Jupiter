﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.DataModel
{
    public class ImportSetting
    {
        public string Source { get; set; }
        public string TableName { get; set; }
        public string Directory { get; set; }
        public string FilePattern { get; set; }
    }
}
