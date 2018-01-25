using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Caster
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
            var cultures = new List<CultureInfo>();
            cultures.Add(CultureInfo.InvariantCulture);
            cultures.Add(new CultureInfo(culture));

            Cultures = cultures;
        }

        public DateTime Execute(object value)
        {
            if (value is DateTime)
                return (DateTime)value;

            if (value is string)
                return StringParse((string)value);

            return System.Convert.ToDateTime(value, DateTimeFormatInfo.InvariantInfo);
        }

        protected virtual DateTime StringParse(string value)
        {
            bool result = false;
            DateTime dateTime;
            result = ValidDateTime(value, Cultures, out dateTime);
            if (!result)
                throw new ArgumentException(string.Format("'{0}' is not recognized as a valid date", value), "value");

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
                                       DateTimeStyles.AllowWhiteSpaces,
                                       out dateTime);
               if (result)
                   return true;
            }

            return false;
        }

        public bool IsValid(object value)
        {
            if (value is DateTime)
                return true;
            
            if (value is string)
            {
                var temp = DateTime.MinValue;
                return ValidDateTime((string)value, Cultures, out temp);
            }

            try
            {
                System.Convert.ToDateTime(value, DateTimeFormatInfo.InvariantInfo);
                return true;
            }
            catch (Exception)
            { return false; }
        }
    }
}
