using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class QueryParameterXml
    {
        private string name;
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                //value = value.Trim();
                
                //if (!value.StartsWith("@"))
                //    value = "@" + value;

                name = value;
            }
        }

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
