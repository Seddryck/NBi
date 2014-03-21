using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items
{
    public class QueryParameterXml : IQueryParameter
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

        private string sqlType;
        [XmlAttribute("sql-type")]
        public string SqlType
        {
            get
            {
                return sqlType;
            }
            set
            {
                sqlType = value;
            }
        }

        [XmlText]
        public string StringValue { get; set; }

        public T GetValue<T>()
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                var stringWithoutSpecialChars = StringValue.Replace("\n", "").Replace("\t", "").Replace("\n", "").Trim();
                return (T)converter.ConvertFrom(stringWithoutSpecialChars);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
