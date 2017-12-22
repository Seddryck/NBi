using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    public interface ISession
    {
        string ConnectionString { get; }
        object CreateNew();
        Type UnderlyingSessionType { get; }
    }
}
