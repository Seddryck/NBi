using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.ResultSet;

public class EmptyResultSet
{
    public string ColumnCount { get; set; }

    public List<ColumnDefinitionLight> Columns { get; set; } = new List<ColumnDefinitionLight>();

    public bool ColumnsSpecified
    {
        get => (Columns?.Count ?? 0) != 0;
        set { return; }
    }
}
