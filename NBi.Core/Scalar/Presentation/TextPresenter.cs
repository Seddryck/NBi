using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public class TextPresenter : BasePresenter
    {
        protected override string PresentNotNull(object value)
        {
            return value switch
            {
                string x => PresentString(x),
                _ => PresentString(value.ToString()!),
            };
        }

        protected string PresentString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "(empty)";
            else if (string.IsNullOrEmpty(value.Trim()))
                return $"({value.Length} space{(value.Length > 1 ? "s" : string.Empty)})";
            else
                return value;
        }
    }
}
