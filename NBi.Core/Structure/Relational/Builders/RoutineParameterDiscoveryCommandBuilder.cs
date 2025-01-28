using NBi.Core.Structure;
using NBi.Core.Structure.Relational.PostFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.Builders;

class RoutineParameterDiscoveryCommandBuilder : RelationalDiscoveryCommandBuilder
{
    protected override string BasicCommandText
        => "select right([{0}_name], len([{0}_name])-1) from INFORMATION_SCHEMA.Routines r inner join INFORMATION_SCHEMA.{1} p on r.[routine_schema]=p.[specific_schema] and r.[routine_name]=p.[specific_name] where 1=1"; 

    public RoutineParameterDiscoveryCommandBuilder()
        : base("parameter", "parameters")
    { }

    protected override IEnumerable<ICommandFilter> BuildCaptionFilters(IEnumerable<CaptionFilter> filters)
    {
        yield return new CommandFilter(string.Format("r.[routine_schema]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Perspectives).Caption
                                                        ));

        yield return new CommandFilter(string.Format("r.[routine_name]='{0}'"
                                                        , filters.Single(f => f.Target == Target.Routines).Caption
                                                        ));

        var filter = filters.SingleOrDefault(f => f.Target == Target.Parameters);
        if (filter != null)
            yield return new CommandFilter(string.Format("p.[parameter_name]='@{0}'"
                                                       , filters.Single(f => f.Target == Target.Parameters).Caption
                                                       ));
    }

    protected override IEnumerable<IFilter> BuildNonCaptionFilters(IEnumerable<IFilter> filters)
    {
        var resultFilter = (IValueFilter?)filters.SingleOrDefault(f => f is IsResultFilter);
        if (resultFilter != null)
            yield return new CommandFilter(string.Format("p.[is_result]='{0}'"
                                                       , resultFilter.Value
                                                       ));

        var parameterDirectionFilter = (IValueFilter?)filters.SingleOrDefault(f => f is ParameterDirectionFilter);
        if (parameterDirectionFilter != null)
            yield return new CommandFilter(string.Format("p.[parameter_mode]='{0}'"
                                                       , parameterDirectionFilter.Value
                                                       ));
    }
}
