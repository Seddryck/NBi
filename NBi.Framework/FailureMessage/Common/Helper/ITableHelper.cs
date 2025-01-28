using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Common.Helper;

public interface ITableHelper<U>
{
    void Render(U writer);
}
