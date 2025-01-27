using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

public abstract class DataTypeDiscoveryCommand : IDataTypeDiscoveryCommand
{
    protected readonly IDbCommand command;
    protected readonly CommandDescription description;

    public virtual CommandDescription Description
    {
        get { return description; }
    }

    protected internal DataTypeDiscoveryCommand(IDbCommand command, CommandDescription description)
    {
        this.command = command;
        this.description = description;
    }

    public abstract DataTypeInfo? Execute();

}
