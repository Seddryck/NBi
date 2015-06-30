using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders
{
    class RoutineDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
    {
        protected virtual string BasicCommandText
        {
            get { return "select [{0}_name] from INFORMATION_SCHEMA.{1} where 1=1"; }
        }

        protected virtual string ProcedureType { get; set; }
        

        public RoutineDiscoveryCommandBuilder()
        {
            CaptionName = "routine";
            TableName = "routines";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
           
            yield return new CommandFilter(string.Format("[routine_schema]='{0}'"
                                                            , filters.Single(f => f.Target == Target.Routines).Caption
                                                            ));

            var filter = filters.SingleOrDefault(f => f.Target == Target.Routines);
            if (filter != null)
                yield return new CommandFilter(string.Format("[routine_name]='{0}'"
                                                           , filters.Single(f => f.Target == Target.Routines).Caption
                                                           ));
        }

    }
}
