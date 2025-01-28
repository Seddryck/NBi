using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public interface ISingleCaseAction : ICaseAction
{
    void Execute(CaseSet testCases);
}
