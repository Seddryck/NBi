using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items;

public abstract class ParameterXml
{
    [DefaultValue(false)]
    [XmlAttribute("remove")]
    public bool IsRemoved { get; set; } = false;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlText]
    public string StringValue { get; set; } = string.Empty;

    public virtual T GetValue<T>()
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null)
        {
            var stringWithoutSpecialChars = StringValue.Replace("\n", "").Replace("\t", "").Replace("\n", "").Trim();
            return (T)(converter.ConvertFrom(stringWithoutSpecialChars) ?? throw new NullReferenceException());
        }
        else
            throw new InvalidOperationException();
    }
}
