using System.Collections.Generic;
using System.Linq;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Constraints;
using NBi.Core.ResultSet;
using NBi.Model.Items.ResultSet;
using System.IO;
using NBi.Model.Items.Alteration;
using NBi.Model.Items.ResultSet.Combination;
using System;
using NBi.Model.Items.Alteration.Renaming;
using NBi.Model.Items.Alteration.Extension;
using NBi.Model.Items.Calculation;
using NBi.Model.Items.Alteration.Conversion;
using NBi.Model.Items.Alteration.Transform;
using NBi.Model.Items.Alteration.Summarization;
using NBi.Model.Items.Alteration.Reshaping;
using NBi.Model.Items.Alteration.Projection;
using NBi.Model.Items.Alteration.Lookup;
using NBi.Model.Variables.Sequence;
using NBi.Model.Items.Hierarchical.Xml;
using NBi.Model.Items.Hierarchical.Json;
using NBi.Model.Items.Alteration.Merging;
using NBi.Model.Items.Alteration.Duplication;

namespace NBi.Model.Systems;

public class ResultSetSystem : AbstractSystemUnderTest
{
    public List<Row> _rows { get; set; } = [];

    public IList<IRow> Rows
        => _rows.Cast<IRow>().ToList();

    public IList<string> Columns
    {
        get
        {
            if (_rows.Count == 0)
                return new List<string>();

            var names = new List<string>();
            var row = _rows[0];

            foreach (var cell in row.Cells)
            {
                if (!string.IsNullOrEmpty(cell.ColumnName))
                    names.Add(cell.ColumnName);
                else
                    names.Add(string.Empty);
            }
            return names;
        }
    }

    public IContent Content
        => new Content(Rows, Columns);

    [Obsolete("Use File in place of FileAttribute")]
    public virtual string FilePath
    {
        get => File.Path + (File.IsBasic() ? string.Empty : $"!{File.Parser!.Name}");
        set
        {
            var tokens = value.Split(new[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            File.Path = tokens[0];
            if (tokens.Length > 1)
                File.Parser = new Parser() { Name = tokens[1] };
            else
                File.Parser = null;
        }
    }

    public virtual Iteration? Iteration { get; set; } = null;

    public virtual ResultSetSystem? NestedResultSet { get; set; } = null;

    public virtual FileSource File { get; set; } = new FileSource();

    public override BaseItem? BaseItem
        => Query;

    public virtual Query? Query { get; set; }

    public virtual SequenceCombination? SequenceCombination { get; set; }

    public virtual SequenceDefinition? Sequence { get; set; }

    public virtual XmlSource? XmlSource { get; set; }

    public virtual JsonSource? JsonSource { get; set; }

    public virtual EmptyResultSet? Empty { get; set; }

    public bool SequenceCombinationSpecified { get => SequenceCombination != null; set { } }

    public virtual List<AlterationDefinition> Alterations { get; set; } = [];

    public bool AlterationsSpecified
    {
        get => Alterations.Count > 0;
        set {}
    }

    public virtual IfUnavailable? IfUnavailable { get; set; }

    public override ICollection<string> GetAutoCategories()
    {
        return new List<string>() { "Result-set" };
    }

    public ResultSetSystem()
    {
        _rows = new List<Row>();
    }
}
