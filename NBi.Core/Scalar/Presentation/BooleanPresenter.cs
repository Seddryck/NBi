using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public class BooleanPresenter : BasePresenter
    {
        protected override string PresentNotNull(object value)
        {
            switch (value)
            {
                case bool x : return PresentBoolean(x);
                case string x : return PresentString(x);
                case decimal x : return PresentBooleanObject(x);
                case short x : return PresentBooleanObject(x);
                case int x : return PresentBooleanObject(x);
                case double x : return PresentBooleanObject(x);
                case float x : return PresentBooleanObject(x);
                default:
                    return PresentString(value.ToString() ?? string.Empty);
            }
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
}
