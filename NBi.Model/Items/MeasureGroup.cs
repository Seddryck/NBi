using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class MeasureGroup : Perspective, IPerspectiveFilter
{
    public string Perspective { get; set; }

    public override string TypeName
    {
        get { return "measure-group"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:perspective", Perspective);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        values.Add(string.Format("Measure group '{0}'", Caption));
        values.Add("Measure groups");
        return values;
    }
}
