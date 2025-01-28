using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.SerializationOption;

public class CData : IXmlSerializable
{
    private string value;

    /// <summary>
    /// Allow direct assignment from string:
    /// CData cdata = "abc";
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator CData(string value)
    {
        return new CData(value);
    }

    /// <summary>
    /// Allow direct assigment to string
    /// string str = cdata;
    /// </summary>
    /// <param name="cdata"></param>
    /// <returns></returns>
    public static implicit operator string(CData cdata)
    {
        return cdata.value;
    }

    public CData() : this(string.Empty)
    {
    }

    public CData(string value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value;
    }

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
        value = reader.ReadElementString();
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        writer.WriteCData(value);
    }
}
