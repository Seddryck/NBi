using DubUrl.Discovery;
using System.Data.Common;

namespace NBi.Core.Query.Client;

internal sealed class DubUrlSchemaDiscovery(DubUrlClient client, ISchemaMetadataReader reader)
{
    public static bool TryCreate(DubUrlClient client, out DubUrlSchemaDiscovery? discovery)
    {
        discovery = client.Url.TryGetCapability<SchemaDiscoveryCapability>(out var capability)
            ? new(client, capability!.Reader)
            : null;
        return discovery is not null;
    }

    public IEnumerable<(MetadataName Identity, IReadOnlyList<MetadataColumn> Columns)> ReadObjects(
        bool includeColumns, string? schema, string? name)
    {
        using var connection = (DbConnection)client.CreateConnection();
        reader.ConfigureOwnedConnection(connection);
        connection.Open();

        var features = new List<SchemaFeature> { SchemaFeature.Tables };
        if (reader.Capabilities.GetValueOrDefault(SchemaFeature.Views) == MetadataCoverage.Available)
            features.Add(SchemaFeature.Views);
        if (includeColumns)
            features.Add(SchemaFeature.Columns);

        var metadata = reader.ReadAsync(connection, new DiscoveryOptions
        {
            Features = features,
        }).GetAwaiter().GetResult();

        foreach (var feature in features)
        {
            if (metadata.Coverage.TryGetValue(feature, out var coverage)
                && coverage is not (MetadataCoverage.Available or MetadataCoverage.Absent))
                throw new NotSupportedException($"DubUrl cannot supply {feature} metadata for this connection.");
        }

        return metadata.Tables.Select(table => (table.Identity, table.Columns))
            .Concat(metadata.Views.Select(view => (view.Identity, view.Columns)))
            .Where(item => (schema is null || (item.Identity.Schema ?? item.Identity.Database ?? item.Identity.Catalog) == schema)
                && (name is null || item.Identity.Name == name));
    }
}
