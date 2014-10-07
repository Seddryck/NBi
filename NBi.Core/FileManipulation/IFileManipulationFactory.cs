using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.FileManipulation
{
    interface IFileManipulationFactory
    {
        IDecorationCommandImplementation Get(IFileManipulationCommand command);
    }
}
