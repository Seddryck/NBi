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

        List<EtlParameter> Parameters { get; }
    }
}
