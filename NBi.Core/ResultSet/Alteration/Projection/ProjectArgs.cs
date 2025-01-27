using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Projection;

public class ProjectArgs : IProjectionArgs
{
    public IEnumerable<IColumnIdentifier> Identifiers { get; set; }

    public ProjectArgs(IEnumerable<IColumnIdentifier> identifiers)
        => Identifiers = identifiers;
}
