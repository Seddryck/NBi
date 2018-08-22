using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NullToDate : INativeTransformation
    {

        public DateTime DefaultDate { get; }

        public NullToDate(string date)
        {
            var caster = new DateTimeCaster();
            DefaultDate = caster.Execute(date);
        }

        public object Evaluate(object value)
        {
            if (value == null || DBNull.Value.Equals(value) || value as string == "(empty)" || value as string == "(null)" || value is string && string.IsNullOrEmpty(value as string))
                return DefaultDate;
            else
                return value;
        }
    }
}
