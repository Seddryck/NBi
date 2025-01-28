using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "<Pending>")]
public class ResultSetUnavailableException : Exception
{
    public ResultSetUnavailableException(Exception innerException)
        : base("Result-set is not available", innerException) { }
}
