using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Projection;

public class ProjectAwayArgs : ProjectArgs
{
    public ProjectAwayArgs(IEnumerable<IColumnIdentifier> identifiers)
        : base(identifiers) { }
}
