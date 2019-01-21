using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public abstract class BasePresenter : IPresenter
    {
        public string Execute(object value)
        {
            if (value == null || value is DBNull)
                return PresentNull(value);
            else if (value is string && (string)value=="(null)")
                return PresentNull(value);
            else
                return PresentNotNull(value);
        }

        protected virtual string PresentNull(object value) => "(null)";

        protected virtual string PresentNotNull(object value) => value.ToString();
    }
}
