using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class RowHelper
    {
        public KeyCollection? Keys { get; set; }
        public IResultRow? DataRowObj { get; set; }
    }
}
