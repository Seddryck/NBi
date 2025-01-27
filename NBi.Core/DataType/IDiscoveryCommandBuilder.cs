using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

public interface IDiscoveryCommandBuilder
{
    void Build(IEnumerable<CaptionFilter> filters);
    string GetCommandText();
}
