using NBi.GenbiL.Stateful;
using NBi.Xml.SerializationOption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.GenbiL.Action;

public abstract class Serializer
{
    protected internal T XmlDeserializeFromString<T>(string objectData)
        => (T)XmlDeserializeFromString(objectData, typeof(T));

    protected internal static string XmlSerializeFrom<T>(T objectData)
        => SerializeFrom(objectData!, typeof(T));

    protected virtual object XmlDeserializeFromString(string objectData, Type type)
    {
        var overrides = new ReadOnlyAttributes();
        overrides.Build();

        var serializer = new XmlSerializer(type, overrides);

        using TextReader reader = new StringReader(objectData);
        return serializer.Deserialize(reader) ?? throw new NullReferenceException();
    }

    protected static string SerializeFrom(object objectData, Type type)
    {
        var serializer = new XmlSerializer(type);
        var result = string.Empty;
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, objectData);
            result = writer.ToString();
        }
        return result;
    }
}
