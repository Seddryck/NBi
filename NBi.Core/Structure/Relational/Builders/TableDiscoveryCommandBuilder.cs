﻿using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders
{
    class TableDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
    {
        public TableDiscoveryCommandBuilder()
        {
            CaptionName = "table";
            TableName = "tables";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
            yield return new CommandFilter(string.Format("[table_schema]='{0}'"
                                                            , filters.Single(f => f.Target == Target.Schemas).Caption
                                                            ));

            var filter = filters.SingleOrDefault(f => f.Target == Target.Tables);
            if (filter != null)
                yield return new CommandFilter(string.Format("[table_name]='{0}'"
                                                           , filter.Caption
                                                           ));

        }

    }
}
