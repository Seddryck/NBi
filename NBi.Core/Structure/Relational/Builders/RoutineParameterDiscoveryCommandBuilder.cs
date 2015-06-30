using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders
{
    abstract class RoutineParameterDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
    {
        protected virtual string BasicCommandText
        {
            get { return "select [{0}_name], [parameter_name], [parameter_mode], [data_type] from INFORMATION_SCHEMA.{1} left outer join INFORMATION_SCHEMA.Parameters on [routine_schema]=[specific_schema] and [routine_name]=[routine_catalog] where 1=1"; }
        }

        protected virtual string ProcedureType { get; set; }


        public RoutineParameterDiscoveryCommandBuilder()
        {
            CaptionName = "routine";
            TableName = "routines";
        }

        protected override IEnumerable<ICommandFilter> BuildFilters(IEnumerable<CaptionFilter> filters)
        {
            yield return new CommandFilter(string.Format("[routine_type]='{0}'"
                                                            , ProcedureType
                                                            ));
            

            yield return new CommandFilter(string.Format("r.[routine_schema]='{0}'"
                                                            , filters.Single(f => f.Target == Target.Schemas).Caption
                                                            ));

            var additionalFilters = BuildAdditionalFilters(filters);
            foreach (var additionalFilter in additionalFilters)
                yield return additionalFilter;
        }

        protected abstract IEnumerable<ICommandFilter> BuildAdditionalFilters(IEnumerable<CaptionFilter> filters);
    }
}
