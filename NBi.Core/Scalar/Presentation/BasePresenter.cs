using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public abstract class BasePresenter : IPresenter
    {
        public string Execute(object? value)
        {
            if (value == null || value is DBNull)
                return PresentNull();
            else if (value is string && (string)value=="(null)")
                return PresentNull();
            else
                return PresentNotNull(value);
        }

        protected virtual string PresentNull() => "(null)";

        protected virtual string PresentNotNull(object value) => value.ToString() ?? string.Empty;
    }
}
