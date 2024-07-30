using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public class DateTimePresenter : BasePresenter
    {
        protected override string PresentNotNull(object value)
        {
            switch (value)
            {
                case DateTime x: return PresentDateTime(x);
                case string x: return PresentString(x);
                default:
                    return PresentString(value.ToString() ?? string.Empty);
            }
        }

        protected string PresentDateTime(DateTime value)
        {
            if (value.TimeOfDay.Ticks == 0)
                return value.ToString("yyyy-MM-dd");
            else if (value.Millisecond == 0)
                return value.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        
        protected string PresentString(string value)
        {
            DateTime valueDateTime = DateTime.MinValue;
            if (DateTime.TryParse(value, out valueDateTime))
                return PresentDateTime(valueDateTime);
            else
                return value;
        }
    }
}
