using System.Collections.Generic;
using System.IO;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;

namespace NBi.Model.Items.ResultSet;

public class ResultSet : BaseItem
{
    public List<Row> _rows { get; set; } = [];

    public IList<IRow> Rows
    {
        get { return _rows.Cast<IRow>().ToList(); }
    }

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

    public string? File { get; set; }

    public ResultSet()
    {
        _rows = [];
    }
}
