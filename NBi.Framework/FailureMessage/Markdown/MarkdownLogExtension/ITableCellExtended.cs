using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension;

interface ITableCellExtended
{ 
    int RequiredWidth { get; }
    string BuildCodeFormattedString(TableCellRenderSpecificationExtended spec);
}
