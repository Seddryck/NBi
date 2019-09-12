using NBi.Core.Etl;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering
{
    public interface IEtlRunCommandArgs : IDataEngineeringCommandArgs
    {
        string Version { get; }
        IEtlArgs Etl { get; }
    }
}
