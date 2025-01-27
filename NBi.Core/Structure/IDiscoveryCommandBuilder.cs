using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure;

public interface IDiscoveryCommandBuilder
{
    void Build(IEnumerable<IFilter> filters);
    string GetCommandText();
    IEnumerable<IPostCommandFilter> GetPostFilters();
}
