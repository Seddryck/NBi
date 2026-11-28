using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Columns : Dimensions, ITableFilter
{
    public string Table { get; set; }

    protected virtual string ParentPath { get { return string.Format("[{0}]", Table); } }
    protected  override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

    public override string TypeName
    {
        get { return "tables"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:table", Table);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Table))
            values.Add(string.Format("Table '{0}'", Table));
        values.Add("Columns");
        return values;
    }
}
