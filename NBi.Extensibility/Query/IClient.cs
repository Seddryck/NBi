using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Query;

public interface IClient
{
    string ConnectionString { get; }
    object CreateNew();
    Type UnderlyingSessionType { get; }
}
