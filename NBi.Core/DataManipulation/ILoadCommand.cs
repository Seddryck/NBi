using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.DataManipulation
{
    public interface ILoadCommand : IDataManipulationCommand
    {
        string TableName { get; set; }
        string FileName { get; set; }
    }
}
