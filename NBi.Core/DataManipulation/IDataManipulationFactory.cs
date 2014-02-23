using System;
using System.Linq;

namespace NBi.Core.DataManipulation
{
    interface IDataManipulationFactory
    {
        IDataManipulationImplementation Get(IDataManipulationCommand command);
    }
}
