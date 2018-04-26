using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    public interface IClientFactory
    {
        bool CanHandle(string connectionString);
        IClient Instantiate(string connectionString);
    }
}
