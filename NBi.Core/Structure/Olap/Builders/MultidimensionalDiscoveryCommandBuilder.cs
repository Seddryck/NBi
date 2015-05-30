using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders
{
    abstract class MultidimensionalDiscoveryCommandBuilder : AbstractDiscoveryCommandBuilder
    {
        protected override string BasicCommandText 
        {
            get {return "select {0}_caption, {1} from [$system].mdschema_{2} where left(cube_name,1)<>'$'{3}";}
        }

    }
}
