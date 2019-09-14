using NBi.Core.Etl;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core.Decoration.DataEngineering
{
    public interface IEtlRunCommandArgs : IDataEngineeringCommandArgs
    {
        IEtlArgs Etl { get; }
    }
}
