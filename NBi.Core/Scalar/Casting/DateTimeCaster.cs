using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    class DateTimeCaster : ICaster<DateTime>
    {
        protected IEnumerable<CultureInfo> Cultures { get; private set; }

        public DateTimeCaster()
            : this("fr-fr")
        {
        }

        protected DateTimeCaster(string culture)
        {
            var cultures = new List<CultureInfo>
            {
                CultureInfo.InvariantCulture,
                new CultureInfo(culture)
            };

            Cultures = cultures;
        }

        public DateTime Execute(object? value)
        {
            if (value is DateTime dt)
                return dt;

            if (value == DBNull.Value || value is null || (value is string && value as string == "(null)"))
                throw new NBiException($"Can't cast the value '(null)' to a dateTime.");

            if (value is string str)
                return StringParse(str);

            return Convert.ToDateTime(value, DateTimeFormatInfo.InvariantInfo);
        }

        object ICaster.Execute(object? value) => Execute(value);

        protected virtual DateTime StringParse(string value)
        {
            var result = ValidDateTime(value, Cultures, out DateTime dateTime);
            if (!result)
                throw new NBiException($"Can't cast the value '{value}' to a valid dateTime.");

            return dateTime;
        }

        /// <summary>
        // Try to convert a string into a DateTime according to the cultures.
        /// </summary>
        /// <param name="value">The original string to convert to DateTime</param>
        /// <param name="culture">The cultures to try if the parsing is not successfull. Each culture is applied after each other, when one is successful the function return the value.</param>
        /// <param name="dateTime">out param with the value converted to dateTime. DateTime.MinValue if the convertion is not possible.</param>
        /// <returns></returns>
        protected virtual bool ValidDateTime(string value, IEnumerable<CultureInfo> cultures, out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;

            foreach (var culture in cultures)
            {
                var result = DateTime.TryParse(value,
                                        culture.DateTimeFormat,
                                        DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                        out dateTime);
                if (result)
                    return true;
            }

            return false;
        }

        public bool IsValid(object? value)
        {
            if (value is DateTime)
                return true;
            
            if (value is string str)
                return ValidDateTime(str, Cultures, out var _);

            try
            {
                Convert.ToDateTime(value, DateTimeFormatInfo.InvariantInfo);
                return true;
            }
            catch (Exception)
            { return false; }
        }

        public bool IsStrictlyValid(object? value) => IsValid(value);
    }
}
