using NBi.Extensibility.Resolving;
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

            var output = ConvertValue(args.Object);

            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Literal evaluated to: {output}");
            return output;
        }

        private T ConvertValue(object value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter.CanConvertFrom(args.Object.GetType()))
                try
                { return (T)converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, value); }
                catch (Exception)
                { throw new ArgumentException($"Cannot convert the value '{value}' to a '{typeof(T).Name}'"); }
            else
                try
                { return (T)Convert.ChangeType(args.Object, typeof(T)); }
                catch (Exception)
                { throw new ArgumentException($"Cannot convert the value '{value}' to a '{typeof(T).Name}'"); }
        }

        object IResolver.Execute() => Execute();
    }
}
