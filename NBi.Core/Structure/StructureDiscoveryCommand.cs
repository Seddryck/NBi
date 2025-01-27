using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure;

public abstract class StructureDiscoveryCommand : IStructureDiscoveryCommand
{
    protected readonly IDbCommand command;
    protected readonly IEnumerable<IPostCommandFilter> postFilters;
    protected readonly CommandDescription description;

    public virtual CommandDescription Description
    {
        get { return description; }
    }

    protected internal StructureDiscoveryCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
    {
        this.command = command;
        this.postFilters = postFilters;
        this.description = description;
    }

    public abstract IEnumerable<string> Execute();

}
