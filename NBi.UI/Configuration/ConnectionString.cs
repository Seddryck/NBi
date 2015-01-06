using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Configuration
{
    public class ConnectionString
    {
        public ConnectionStringCollection.ConnectionDefinition Definition { get; set; }
        public string Value { get; set; }
    }
}
