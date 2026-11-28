using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Levels : Dimensions, IHierarchyFilter, IDimensionFilter
{
    public string Dimension { get; set; }

    public string Hierarchy { get; set; }

    protected virtual string ParentPath { get { return string.Format("[{0}].[{1}]", Dimension, Hierarchy); } }

    protected override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

    public override string TypeName
    {
        get { return "levels"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:dimension", Dimension);
        dico.Add("sut:hierarchy", Hierarchy);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Dimension))
            values.Add(string.Format("Dimension '{0}'", Dimension));
        if (!string.IsNullOrEmpty(Hierarchy))
            values.Add(string.Format("Hierarchy '{0}'", Hierarchy));
        values.Add("Levels");
        return values;
    }
}
