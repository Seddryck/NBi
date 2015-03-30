﻿using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Tabular.Builders
{
    class SchemaDiscoveryCommandBuilder : TabularDiscoveryCommandBuilder
    {
        public SchemaDiscoveryCommandBuilder()
        {
            CaptionName = "table_schema";
            TableName = "tables";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
            yield return new CommandFilter("left(table_schema,1)<>'$'");

            
            var filter = filters.SingleOrDefault(f => f.Target == Target.Schemas);
            if (filter != null)
                yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                           , filter.Caption
                                                           ));

        }

    }
}
