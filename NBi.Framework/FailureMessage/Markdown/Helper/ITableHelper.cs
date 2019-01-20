using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper
{
    public interface ITableHelper
    {
        MarkdownContainer Render();
    }
}
