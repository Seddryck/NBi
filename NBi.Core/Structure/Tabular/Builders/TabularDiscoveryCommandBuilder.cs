using NBi.Core.Structure.Olap.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Tabular.Builders;

abstract class TabularDiscoveryCommandBuilder : AbstractDiscoveryCommandBuilder
{
    protected TabularDiscoveryCommandBuilder(string captionName, string displayFolderName, string tableName, string visibleName)
        : base(captionName, displayFolderName, tableName, visibleName)
    { }

    protected override string BasicCommandText
    {
        get { return "select {0}, {1} from [$system].dbschema_{2} where 1=1{3}"; }
    }
}
