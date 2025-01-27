using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Extensibility.Query;

public interface IClientFactory
{
    bool CanHandle(string connectionString);
    IClient Instantiate(string connectionString);
}
