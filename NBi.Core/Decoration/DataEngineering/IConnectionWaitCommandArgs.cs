using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering
{
    public interface IConnectionWaitCommandArgs : IDataEngineeringCommandArgs
    {
        int TimeOut { get; }
    }
}
