using NBi.Core.Query.Client;

namespace NBi.Core.DataType.Relational;

internal sealed class DubUrlDataTypeDiscoveryFactory(DubUrlSchemaDiscovery discovery) : IDataTypeDiscoveryFactory
{
    public IDataTypeDiscoveryCommand Instantiate(Target target, IEnumerable<CaptionFilter> filters)
    {
        if (target != Target.Columns)
            throw new NotSupportedException($"DubUrl metadata does not support the NBi data-type request for {target}.");

        return new MetadataCommand(discovery, new CommandDescription(target, filters.ToArray()));
    }

    private sealed class MetadataCommand(DubUrlSchemaDiscovery discovery, CommandDescription description)
        : DataTypeDiscoveryCommand(description)
    {
        public override DataTypeInfo? Execute()
        {
            string Caption(Target target) => Description.Filters.Single(filter => filter.Target == target).Caption;

            var column = discovery.ReadObjects(true, Caption(Target.Perspectives), Caption(Target.Tables))
                .SelectMany(item => item.Columns)
                .SingleOrDefault(item => item.Name == Caption(Target.Columns));
            if (column is null)
                return null;

            var nativeType = column.NativeType?.Split('(', 2)[0].Trim();
            var type = string.IsNullOrEmpty(nativeType) ? column.Type?.ToString() : nativeType;
            if (type is null || column.Nullable is null)
                throw new NotSupportedException($"DubUrl cannot describe the type of column '{column.Name}'.");

            return new DataTypeInfoFactory().Instantiate(new RelationalRow
            {
                DataType = type,
                IsNullable = column.Nullable.Value ? "YES" : "NO",
                CharacterMaximumLength = column.Length ?? 0,
                NumericPrecision = column.Precision ?? 0,
                NumericScale = column.Scale ?? 0,
            });
        }
    }
}
