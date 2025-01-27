using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public interface IQueryResolver
{
    IQuery Execute();
}
