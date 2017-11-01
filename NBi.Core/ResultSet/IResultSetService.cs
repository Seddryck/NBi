using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Loading;
using System;
using System.Collections.Generic;
using System.Data;

namespace NBi.Core.ResultSet
{
    public interface IResultSetService
    {
        ResultSet Execute();

        IReadOnlyList<Alter> Alterations { get; }
        Load Load { get; }
    }
}
