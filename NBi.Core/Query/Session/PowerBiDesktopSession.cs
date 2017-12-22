using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    class PowerBiDesktopSession : AdomdSession
    {
        public PowerBiDesktopSession(string connectionString)
            : base(connectionString)
        { }
    }
}
