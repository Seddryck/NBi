using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Etl
{
    public interface IEtl: IExecutable
    {
        string Server { get; set; }
        string Path { get; set; }
        string Name { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        List<EtlParameter> Parameters { get; }

        string Catalog { get; set; }

        string Folder { get; set; }

        string Project { get; set; }

        bool Is32Bits { get; set; }
    }
}
