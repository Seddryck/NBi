using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders
{
    class SchemaDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
    {
        protected override string BasicCommandText
        {
            get { return base.BasicCommandText; }
        }

        public SchemaDiscoveryCommandBuilder()
        {
            CaptionName = "schema";
            TableName = "schemata";
        }

        protected override IEnumerable<ICommandFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
        {
            var filter = filters.SingleOrDefault(f => f.Target == Target.Perspectives);
            if (filter != null)
                yield return new CommandFilter(string.Format("[schema_name]='{0}'"
                                                           , filter.Caption
                                                           ));
        }

    }
}
