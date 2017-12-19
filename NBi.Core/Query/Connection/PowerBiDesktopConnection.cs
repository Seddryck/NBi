using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    class PowerBiDesktopConnection : OlapConnection
    {
        public PowerBiDesktopConnection(string connectionString)
            : base(connectionString)
        { }
    }
}
