using NBi.Core.ResultSet;
using NBi.Model.Items;
using NBi.Model.Variables.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables.Sequence;

public class SequenceDefinition
{
    public string? Name { get; set; }

    public ColumnType Type { get; set; }

    public SentinelLoop? SentinelLoop { get; set; }

    public List<string> Items { get; set; } = new List<string>();

    public Query? Query { get; set; }

    public FileLoop? FileLoop { get; set; }

    public CustomVariable? Custom { get; set; }

    public bool ItemsSpecified { get => Items.Count > 0; set { } }

    public FilterSequence? Filter { get; set; }

    public bool FilterSpecified { get => Filter != null; set { } }
}
