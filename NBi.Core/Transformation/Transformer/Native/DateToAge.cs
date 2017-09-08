using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class DateToAge : INativeTransformation
    {
        public object Evaluate(object value)
        {
            if (value == null)
                return 0;
            else if (value is DateTime)
                return EvaluateDateTime((DateTime)value);
            else
                throw new NotImplementedException();
        }

        private object EvaluateDateTime(DateTime value)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - value.Year;
            // Go back to the year the person was born in case of a leap year
            return value.AddYears(age) > today ? age-- : age;
        }
        
    }
}
