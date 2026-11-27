using NBi.Core.Query.Client;

namespace NBi.Core.Structure.Relational;

internal sealed class DubUrlStructureDiscoveryFactory(DubUrlSchemaDiscovery discovery) : IStructureDiscoveryFactory
{
    public StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<IFilter> filters)
    {
        var captions = filters.ToArray();
        if (target is not (Target.Tables or Target.Columns) || captions.Any(filter => filter is not CaptionFilter))
            throw new NotSupportedException($"DubUrl metadata does not support the NBi structure request for {target}.");

        return new MetadataCommand(discovery, new CommandDescription(target, captions));
    }

    private sealed class MetadataCommand(DubUrlSchemaDiscovery discovery, CommandDescription description)
        : StructureDiscoveryCommand(description)
    {
        public override IEnumerable<string> Execute()
        {
            var captions = Description.Filters.Cast<CaptionFilter>();
            string? Caption(Target target) => captions.SingleOrDefault(filter => filter.Target == target)?.Caption;

            var columns = Description.Target == Target.Columns;
            var objects = discovery.ReadObjects(columns, Caption(Target.Perspectives), Caption(Target.Tables));
            if (!columns)
                return objects.Select(item => item.Identity.Name).ToArray();

            var name = Caption(Target.Columns);
            return objects.SelectMany(item => item.Columns)
                .Where(column => name is null || column.Name == name)
                .Select(column => column.Name)
                .ToArray();
        }
    }
}
