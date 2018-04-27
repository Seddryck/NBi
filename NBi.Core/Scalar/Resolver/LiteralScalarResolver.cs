using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public class LiteralScalarResolver<T> : IScalarResolver<T>
    {
        private readonly LiteralScalarResolverArgs args;

        public LiteralScalarResolver(LiteralScalarResolverArgs args)
        {
            this.args = args;
        }

        internal LiteralScalarResolver(object value)
        {
            this.args = new LiteralScalarResolverArgs(value);
        }

        public T Execute()
        {
            IFormatProvider formatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;
            if (typeof(T) == typeof(DateTime))
                formatProvider = System.Globalization.DateTimeFormatInfo.InvariantInfo;

            var converter = TypeDescriptor.GetConverter(typeof(T));

            var output = converter.CanConvertFrom(args.Object.GetType()) ?
                converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, args.Object)
                : Convert.ChangeType(args.Object, typeof(T));

            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Literal evaluated to: {output}");
            return (T)output;
        }
    }
}
