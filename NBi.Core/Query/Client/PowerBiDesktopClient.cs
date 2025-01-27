using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Client;

class PowerBiDesktopClient : AdomdClient
{
    public PowerBiDesktopClient(string connectionString)
        : base(connectionString)
    { }
}
