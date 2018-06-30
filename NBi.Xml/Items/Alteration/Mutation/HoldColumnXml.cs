using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet.Alteration;

namespace NBi.Xml.Items.Alteration.Mutation
{
    public class HoldColumnXml : FilteringColumnXml
    {
        public override AlterationType Type => AlterationType.HoldColumns;
    }
}
