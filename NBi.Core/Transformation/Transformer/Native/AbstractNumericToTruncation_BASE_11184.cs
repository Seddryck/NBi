using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractNumericToTruncation : INativeTransformation
    {
        public AbstractNumericToTruncation()
        { }

        public object Evaluate(object value)
        {
            if (value == null || DBNull.Value.Equals(value) || value as string == "(empty)" || value as string == "(null)" || value is string && string.IsNullOrEmpty(value as string))
                return null;
            else
            {
                var caster = new NumericCaster();
                var numeric = caster.Execute(value);
                return Truncate(numeric);
            }
        }

        protected abstract decimal Truncate(decimal numeric);
    }
}
