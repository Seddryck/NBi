using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension;

internal class EmptyTableCellExtended : ITableCellExtended
{
    public int RequiredWidth { get { return 0; } }

    public string BuildCodeFormattedString(TableCellRenderSpecificationExtended spec)
    {
        return "";
    }
}
