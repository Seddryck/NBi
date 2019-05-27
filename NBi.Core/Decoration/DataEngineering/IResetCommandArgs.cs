using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering
{
    public interface IResetCommandArgs : IDataEngineeringCommandArgs
    {
        IScalarResolver<string> TableName { get; set; }
    }
}
