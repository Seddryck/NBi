using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class RoutineParameter : Routine, IPerspectiveFilter, IRoutineFilter, IResultFilter, IParameterDirectionFilter
{
    public string Routine { get; set; }

    public IsResultOption IsResult { get; set; }

    public ParameterDirectionOption Direction { get; set; }

    protected override string Path { get { return string.Format("[{0}].[{1}]", Routine, Caption); } }

    public override string TypeName
    {
        get { return "routine's parameter"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:routine", Routine);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Routine))
            values.Add(string.Format("Routine '{0}'", Routine));
        values.Add(string.Format("Parameter '{0}'", Caption));
        values.Add("Parameters");
        return values;
    }
}
