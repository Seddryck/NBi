using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Report
{
    public class ReportingCommand
    {
        public string Text { get; set; } = string.Empty;
        public CommandType CommandType { get; set; }
    }
}
