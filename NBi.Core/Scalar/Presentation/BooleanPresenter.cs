using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation;

public class BooleanPresenter : BasePresenter
{
    protected override string PresentNotNull(object value)
    {
        return value switch
        {
            bool x => PresentBoolean(x),
            string x => PresentString(x),
            decimal x => PresentBooleanObject(x),
            short x => PresentBooleanObject(x),
            int x => PresentBooleanObject(x),
            double x => PresentBooleanObject(x),
            float x => PresentBooleanObject(x),
            _ => PresentString(value.ToString() ?? string.Empty),
        };
    }

    protected string PresentBoolean(bool value) => value ? "True" : "False";

    protected string PresentBooleanObject(object value) => PresentBoolean(Convert.ToBoolean(value));

    protected string PresentString(string value)
    {
        var workingValue = value.ToLower().Trim();
        if (new[] { "true", "yes", "1" }.Contains(workingValue))
            return PresentBoolean(true);
        else if (new[] { "false", "no", "0" }.Contains(workingValue))
            return PresentBoolean(false);
        else
            return value;
    }
}
