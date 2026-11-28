using NBi.Model.Xml;

namespace NBi.GenbiL.Action;

public abstract class Serializer
{
    protected internal T XmlDeserializeFromString<T>(string objectData)
        => (T)XmlDeserializeFromString(objectData, typeof(T));

    protected internal static string XmlSerializeFrom<T>(T objectData)
        => SerializeFrom(objectData!, typeof(T));

    protected virtual object XmlDeserializeFromString(string objectData, Type type)
        => XmlModelSerializer.Deserialize(objectData, type);

    protected static string SerializeFrom(object objectData, Type type)
        => XmlModelSerializer.Serialize(objectData, type, useCData: false);
}
