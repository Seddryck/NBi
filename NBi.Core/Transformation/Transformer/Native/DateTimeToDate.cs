using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class DateTimeToDate : INativeTransformation
    {

        public object Evaluate(object value)
        {
            if (value == null)
                return null;
            else if (value is DateTime)
                return EvaluateDateTime((DateTime)value);
            else
                throw new NotImplementedException();
        }

        protected virtual object EvaluateDateTime(DateTime value) => value.Date;
    }
}
