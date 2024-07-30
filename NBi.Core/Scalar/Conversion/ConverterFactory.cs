using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Conversion
{
    public class ConverterFactory
    {
        public IConverter Instantiate(string from, string to, object? defaultValue, string culture)
        {
            var cultureFactory = new CultureFactory();
            var cultureInfo = cultureFactory.Instantiate(culture);

            if (from != "text")
                throw new ArgumentOutOfRangeException();

            return to switch
            {
                "date" => new TextToDateConverter(cultureInfo, CastToDateTime(defaultValue)),
                "dateTime" => new TextToDateTimeConverter(cultureInfo, CastToDateTime(defaultValue)),
                "numeric" => new TextToNumericConverter(cultureInfo, CastToNumeric(defaultValue)),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private DateTime? CastToDateTime(object? obj)
        {
            if (obj == null)
                return null;

            var caster = new DateTimeCaster();
            if (caster.IsValid(obj))
                return caster.Execute(obj);

            throw new ArgumentException();
        }

        private decimal? CastToNumeric(object? obj)
        {
            if (obj == null)
                return null;

            var caster = new NumericCaster();
            if (caster.IsValid(obj))
                return caster.Execute(obj);

            throw new ArgumentException();
        }
    }
}
