using NBi.Core;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage;

public interface IItemsMessageFormatter
{
    void Build(IEnumerable<string> expectedItems, IEnumerable<string> actualItems, ListComparer.Result result);

    string RenderExpected();
    string RenderActual();
    string RenderAnalysis();
}
