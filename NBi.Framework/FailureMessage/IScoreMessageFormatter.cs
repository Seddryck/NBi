using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage;

public interface IScoreMessageFormatter
{

    void Initialize(decimal score, decimal threshold, bool result);
    string RenderExpected();
    string RenderActual();
    string RenderMessage();
}
