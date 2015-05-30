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
            get { return base.BasicCommandText + " and [schema_owner]='dbo'"; }
        }

        public SchemaDiscoveryCommandBuilder()
        {
            CaptionName = "schema";
            TableName = "schemata";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
            var filter = filters.SingleOrDefault(f => f.Target == Target.Schemas);
            if (filter != null)
                yield return new CommandFilter(string.Format("[schema_name]='{0}'"
                                                           , filter.Caption
                                                           ));
        }

    }
}
