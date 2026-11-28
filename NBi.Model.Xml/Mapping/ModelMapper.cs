using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Xml;
using NBi.Model.Settings;
using NBi.Model.Xml.Settings;

namespace NBi.Model.Xml.Mapping;

/// <summary>Maps the serialized contracts to the independent suite model.</summary>
internal static class ModelMapper
{
    private const string ContractNamespace = "NBi.Model.Xml";
    private const string ModelNamespace = "NBi.Model";

    private static readonly Assembly ContractAssembly = typeof(TestSuiteXml).Assembly;
    private static readonly Assembly ModelAssembly = typeof(Model.TestSuite).Assembly;

    private static readonly IReadOnlyDictionary<string, string> ModelNames = new Dictionary<string, string>
    {
        ["AlterationXml"] = "AlterationDefinition",
        ["AssemblyXml"] = "AssemblyQuery",
        ["ConditionXml"] = "TestCondition",
        ["CustomXml"] = "CustomVariable",
        ["DecorationXml"] = "DecorationDefinition",
        ["EnvironmentXml"] = "EnvironmentVariable",
        ["FileXml"] = "FileSource",
        ["PatternXml"] = "PatternDefinition",
        ["RangeXml"] = "ValueRange",
        ["RankingXml"] = "RankingDefinition",
        ["SequenceXml"] = "SequenceDefinition",
        ["SettingsXml"] = "SuiteSettings",
        ["TestStandaloneXml"] = "StandaloneTest",
        ["TestXml"] = "TestDefinition",
        ["TypeXml"] = "TypeDefinition",
    };

    private static readonly IReadOnlyDictionary<Type, Type> ContractsToModels = ContractAssembly.GetTypes()
        .Where(type => type.FullName?.StartsWith(ContractNamespace + ".", StringComparison.Ordinal) == true)
        .Select(type => (Contract: type, Model: ModelAssembly.GetType(ModelName(type))))
        .Where(pair => pair.Model != null)
        .ToDictionary(pair => pair.Contract, pair => pair.Model!);

    private static readonly IReadOnlyDictionary<Type, Type> ModelsToContracts =
        ContractsToModels.ToDictionary(pair => pair.Value, pair => pair.Key);

    private static readonly ConcurrentDictionary<(Type Source, Type Target), (FieldInfo Source, FieldInfo Target)[]> Fields = new();

    public static Type ContractType(Type type)
    {
        if (ModelsToContracts.TryGetValue(type, out var contract))
            return contract;
        if (type.Assembly == ModelAssembly)
            throw new ArgumentException($"No XML contract is registered for {type}.", nameof(type));
        return type;
    }

    public static object ToContract(object model, Type modelType)
        => Convert(model, ContractType(modelType), ModelsToContracts, new(ReferenceEqualityComparer.Instance))!;

    public static object ToModel(object contract, Type modelType)
        => Convert(contract, modelType, ContractsToModels, new(ReferenceEqualityComparer.Instance))!;

    private static string ModelName(Type contract)
    {
        var name = contract.FullName![ContractNamespace.Length..];
        // Preserve namespace and declaring-type separators while translating CLR type names.
        var prefixLength = name.LastIndexOf('.') + 1;
        var typeNames = name[prefixLength..].Split('+').Select(part =>
            ModelNames.TryGetValue(part, out var mapped) ? mapped
                : part.EndsWith("Xml", StringComparison.Ordinal) ? part[..^3] : part);
        return ModelNamespace + name[..prefixLength] + string.Join("+", typeNames);
    }

    private static object? Convert(object? source, Type declaredTarget, IReadOnlyDictionary<Type, Type> types,
        Dictionary<object, object> visited)
    {
        if (source == null)
            return null;
        if (visited.TryGetValue(source, out var existing))
            return existing;

        if (ReferenceEquals(source, InstanceSettlingXml.Unique))
            return Model.InstanceSettling.Unique;
        if (ReferenceEquals(source, Model.InstanceSettling.Unique))
            return InstanceSettlingXml.Unique;
        if (ReferenceEquals(source, Items.Calculation.Grouping.GroupByXml.None))
            return Model.Items.Calculation.Grouping.GroupBy.None;
        if (ReferenceEquals(source, Model.Items.Calculation.Grouping.GroupBy.None))
            return Items.Calculation.Grouping.GroupByXml.None;

        var sourceType = source.GetType();

        if (source is CsvProfileXml xmlProfile)
        {
            return visited[source] = new CsvProfile(xmlProfile.FieldSeparator, xmlProfile.TextQualifier, xmlProfile.RecordSeparator)
            {
                FirstRowHeader = xmlProfile.FirstRowHeader,
                MissingCell = xmlProfile.MissingCell,
                EmptyCell = xmlProfile.EmptyCell,
            };
        }

        if (source is CsvProfile profile)
        {
            return visited[source] = new CsvProfileXml(profile.FieldSeparator, profile.TextQualifier, profile.RecordSeparator)
            {
                FirstRowHeader = profile.FirstRowHeader,
                MissingCell = profile.MissingCell,
                EmptyCell = profile.EmptyCell,
            };
        }

        if (source is XmlElement element && declaredTarget == typeof(string))
            return element.OuterXml;

        if (source is string draft && declaredTarget == typeof(XmlElement))
        {
            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(draft);
            return document.DocumentElement!;
        }

        if (source is Array array)
        {
            var itemType = declaredTarget.GetElementType() ?? array.GetType().GetElementType()!;
            var targetArray = Array.CreateInstance(itemType, array.Length);
            visited.Add(source, targetArray);
            for (var index = 0; index < array.Length; index++)
                targetArray.SetValue(Convert(array.GetValue(index), itemType, types, visited), index);
            return targetArray;
        }

        if (source is IList list && sourceType.IsGenericType)
        {
            var itemType = declaredTarget.IsGenericType
                ? declaredTarget.GetGenericArguments()[0]
                : sourceType.GetGenericArguments()[0];
            var targetList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
            visited.Add(source, targetList);
            foreach (var value in list)
                targetList.Add(Convert(value, itemType, types, visited));
            return targetList;
        }

        // Historical spellings deserialize to empty subclasses of their current wire contract.
        if (source is Constraints.EqualToOldXml or Constraints.EquivalentToOldXml
            or Constraints.FasterThanOldXml or Constraints.SyntacticallyCorrectOldXml
            or Constraints.SubsetOfOldXml or Systems.ResultSetSystemOldXml
            or Constraints.Comparer.WithinListXml)
            sourceType = sourceType.BaseType!;

        if (sourceType.IsEnum && declaredTarget.IsEnum && sourceType != declaredTarget)
            return Enum.ToObject(declaredTarget, (Enum)source);

        if (!types.TryGetValue(sourceType, out var targetType))
        {
            if (declaredTarget.IsInstanceOfType(source))
                return source;

            throw new InvalidOperationException($"No model mapping from {sourceType} to {declaredTarget}.");
        }

        if (sourceType.IsEnum)
            return Enum.Parse(targetType, source.ToString()!);

        var target = Activator.CreateInstance(targetType)
            ?? throw new InvalidOperationException($"Cannot create {targetType}.");
        visited.Add(source, target);

        // Contract setters can translate legacy aliases and mutate several members at once.
        // Copy their resulting state once, preserving shared references and the applied defaults.
        foreach (var (sourceField, targetField) in Fields.GetOrAdd((sourceType, targetType), pair => GetFields(pair, types)))
            targetField.SetValue(target, Convert(sourceField.GetValue(source), targetField.FieldType, types, visited));

        return target;
    }

    private static (FieldInfo Source, FieldInfo Target)[] GetFields((Type Source, Type Target) pair,
        IReadOnlyDictionary<Type, Type> types)
    {
        const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var fields = new List<(FieldInfo Source, FieldInfo Target)>();
        for (var layer = pair.Source; layer != null && types.TryGetValue(layer, out var targetLayer); layer = layer.BaseType)
        {
            var sourceFields = layer.GetFields(flags);
            var targetFields = targetLayer.GetFields(flags).ToDictionary(field => field.Name);
            foreach (var field in sourceFields)
            {
                if (!targetFields.Remove(field.Name, out var target))
                    throw new InvalidOperationException($"No model mapping for {layer}.{field.Name}.");
                fields.Add((field, target));
            }

            if (targetFields.Count != 0)
                throw new InvalidOperationException($"Unmapped fields on {targetLayer}: {string.Join(", ", targetFields.Keys)}.");
        }
        return fields.ToArray();
    }
}
