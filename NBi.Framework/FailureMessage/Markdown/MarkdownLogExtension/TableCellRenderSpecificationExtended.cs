using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension;

class TableCellRenderSpecificationExtended
{
    private readonly TableColumnAlignment _alignment;
    private readonly int _maximumWidth;

    public TableCellRenderSpecificationExtended(TableColumnAlignment alignment, int maximumWidth)
    {
        _alignment = alignment;
        _maximumWidth = maximumWidth;
    }



    public TableColumnAlignment Alignment
    {
        get { return _alignment; }
    }

    public int MaximumWidth
    {
        get { return _maximumWidth; }
    }
}
