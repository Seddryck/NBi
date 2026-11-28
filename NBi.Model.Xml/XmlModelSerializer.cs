using System.Collections.Concurrent;
using System.Xml.Serialization;
using NBi.Model.Xml.Mapping;
using NBi.Model.Xml.SerializationOption;

namespace NBi.Model.Xml;

/// <summary>Reads and writes NBi XML using the serialization-independent model.</summary>
public static class XmlModelSerializer
{
    private enum Mode { Read, Write, Plain }

    private static readonly ConcurrentDictionary<(Type Contract, Mode Mode), Lazy<XmlSerializer>> Serializers = new();

    public static T Deserialize<T>(string xml) => (T)Deserialize(xml, typeof(T));

    public static string Serialize<T>(T model) where T : notnull => Serialize(model, typeof(T));

    public static object Deserialize(string xml, Type modelType)
    {
        var contractType = ModelMapper.ContractType(modelType);
        using var reader = new StringReader(xml);
        var contract = GetSerializer(contractType, Mode.Read).Deserialize(reader)
            ?? throw new InvalidOperationException($"No {contractType.Name} could be read.");
        return contractType == modelType ? contract : ModelMapper.ToModel(contract, modelType);
    }

    public static string Serialize(object model, Type modelType, bool useCData = true)
    {
        var contractType = ModelMapper.ContractType(modelType);
        var contract = contractType == modelType ? model : ModelMapper.ToContract(model, modelType);
        using var writer = new StringWriter();
        GetSerializer(contractType, useCData ? Mode.Write : Mode.Plain).Serialize(writer, contract);
        return writer.ToString();
    }

    internal static XmlSerializer GetPlainContractSerializer(Type contract)
        => GetSerializer(contract, Mode.Plain);

    private static XmlSerializer GetSerializer(Type contract, Mode mode)
        => Serializers.GetOrAdd((contract, mode), key => new(() => CreateSerializer(key.Contract, key.Mode))).Value;

    private static XmlSerializer CreateSerializer(Type contract, Mode mode)
    {
        if (mode == Mode.Plain)
            return new XmlSerializer(contract, ModelEnumMappings.Create());

        ReadWriteAttributes attributes = mode == Mode.Read ? new ReadOnlyAttributes() : new WriteOnlyAttributes();
        attributes.Build();
        return new XmlSerializer(contract, attributes);
    }
}
