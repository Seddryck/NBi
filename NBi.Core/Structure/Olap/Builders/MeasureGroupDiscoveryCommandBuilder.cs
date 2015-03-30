﻿using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.Builders
{
    class MeasureGroupDiscoveryCommandBuilder : MultidimensionalDiscoveryCommandBuilder
    {
        protected override string BasicCommandText
        {
            get { return "select {0}_name, {1} from [$system].mdschema_{2} where left(cube_name,1)<>'$'{3}"; }
        }

        public MeasureGroupDiscoveryCommandBuilder()
        {
            CaptionName = "measuregroup";
            TableName = "measures";
            VisibleName = "measure";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
            yield return new CommandFilter("len(measuregroup_name)>0");
            
            yield return new CommandFilter(string.Format("[cube_name]='{0}'"
                                    , filters.Single(f => f.Target == Target.Perspectives).Caption
                                    ));

            var mgFilter = filters.SingleOrDefault(f => f.Target == Target.MeasureGroups);
            if (mgFilter != null)
                yield return new CommandFilter(string.Format("[measuregroup_name]='{0}'"
                                                , filters.Single(f => f.Target == Target.MeasureGroups).Caption
                                                ));
            
        }

    }
}
