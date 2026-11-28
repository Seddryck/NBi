using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Column : Table, ITableFilter
{
    public string Table { get; set; }

    public override string TypeName
    {
        get { return "column"; }
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
