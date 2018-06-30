using NBi.Core.ResultSet.Alteration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Items.Alteration.Mutation
{
    public abstract class AlterationItemXml
    {
        public abstract AlterationType Type { get; }
    }
}
