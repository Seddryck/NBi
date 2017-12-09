using NBi.Core.ResultSet.Alteration;
using System;
using System.Collections.Generic;
using System.Data;

namespace NBi.Core.ResultSet
{
    public delegate ResultSet Load();

    public interface IResultSetService
    {
        ResultSet Execute();

        IReadOnlyList<Alter> Alterations { get; }
        Load Load { get; }
    }
}
