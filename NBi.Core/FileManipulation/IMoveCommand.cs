using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.FileManipulation
{
    public interface IMoveCommand : IFileManipulationCommand
    {
        string OriginalFullPath { get; }
    }
}
