using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Routines : AbstractMembersItem, IPerspectiveFilter
{
    public string Perspective { get; set; }

    protected virtual string Path { get { return string.Format("[{0}]", Caption); } }

    public override string TypeName
    {
        get { return "routines"; }
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
        values.Add("Routines");
        return values;
    }
}
