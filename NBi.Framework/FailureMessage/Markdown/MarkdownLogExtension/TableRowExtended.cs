using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension
{
    class TableRowExtended
    {
        private IEnumerable<ITableCellExtended> _cells = Enumerable.Empty<ITableCellExtended>();

        public IEnumerable<ITableCellExtended> Cells
        {
            get { return _cells; }
            set { _cells = value ?? Enumerable.Empty<ITableCellExtended>(); }
        }
    }
}
