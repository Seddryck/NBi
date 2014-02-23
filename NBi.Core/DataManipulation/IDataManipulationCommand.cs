using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.DataManipulation
{
    public interface IDataManipulationCommand
    {
        string ConnectionString { get; set; }
    }
}
