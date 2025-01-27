using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension;

internal class TableColumnExtended
{
    private ITableCellExtended _headerCell;
    private ITableCellExtended _subHeaderCell;

    public TableColumnExtended()
    {
        _headerCell = new EmptyTableCellExtended();
        _subHeaderCell = new EmptyTableCellExtended();
    }

    public ITableCellExtended HeaderCell
    {
        get { return _headerCell; }
        set { _headerCell = value ?? new EmptyTableCellExtended(); }
    }

    public ITableCellExtended SubHeaderCell
    {
        get { return _subHeaderCell; }
        set { _subHeaderCell = value ?? new EmptyTableCellExtended(); }
    }

    public TableColumnAlignment Alignment { get; set; }
}
