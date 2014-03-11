using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items
{
    public abstract class ParameterXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string StringValue { get; set; }

        public virtual T GetValue<T>()
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
