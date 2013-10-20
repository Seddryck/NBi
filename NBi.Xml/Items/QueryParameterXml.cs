using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class QueryParameterXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("sql-type")]
        public string SqlType { get; set; }

        [XmlText]
        public string StringValue { get; set; }

        public T GetValue<T>()
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFrom(StringValue);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
