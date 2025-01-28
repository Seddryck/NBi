using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering;

public interface IDataEngineeringCommandArgs : IDecorationCommandArgs
{
    string ConnectionString { get; }
}
