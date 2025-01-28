using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure;

public interface IStructureDiscoveryCommand
{
    CommandDescription Description { get; }
    IEnumerable<string> Execute();
}
