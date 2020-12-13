using NBi.Extensibility;
using NBi.Core.ResultSet.Alteration;
using System.Collections.Generic;
using System.Data;

namespace NBi.Core.ResultSet
{
    public delegate IResultSet Load();

    public interface IResultSetService
    {
        IResultSet Execute();

        IReadOnlyList<Alter> Alterations { get; }
        Load Load { get; }
    }
}
