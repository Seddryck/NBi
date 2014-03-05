using System;
using System.Linq;

namespace NBi.Core.DataManipulation
{
    interface IDataManipulationFactory
    {
        IDecorationCommandImplementation Get(IDataManipulationCommand command);
    }
}
