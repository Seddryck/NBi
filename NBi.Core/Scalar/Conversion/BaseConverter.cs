using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Conversion
{
    abstract class BaseConverter<T, U> : IConverter
    {
        public object? DefaultValue { get; }

        public Type DestinationType
        {
            get
            {
                var type = typeof(U);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return Nullable.GetUnderlyingType(type)!;
                return type;
            }
        }

        private readonly IPredicate predicate;
        private readonly CultureInfo cultureInfo;

        public BaseConverter(CultureInfo cultureInfo, object? defaultValue)
        {
            var info = GetPredicateArgs(cultureInfo);
            var predicateFactory = new PredicateFactory();
            predicate = predicateFactory.Instantiate(info);

            DefaultValue = defaultValue;
            this.cultureInfo = cultureInfo;
        }

        protected abstract PredicateArgs GetPredicateArgs(CultureInfo cultureInfo);

        public virtual object? Execute(object x)
        {
            if (predicate.Execute(x) && x is T tX)
                return OnExecute(tX, cultureInfo);
            else
                return DefaultValue;
        }

        protected abstract U OnExecute(T x, CultureInfo cultureInfo);
    }
}
