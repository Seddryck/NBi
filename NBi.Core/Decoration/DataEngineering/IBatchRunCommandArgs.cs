using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering
{
    public interface IBatchRunCommandArgs : IDataEngineeringCommandArgs
    {
        IScalarResolver<string> Name { get; set; }
        IScalarResolver<string> Path { get; }
        IScalarResolver<string> Version { get; }
    }
}
