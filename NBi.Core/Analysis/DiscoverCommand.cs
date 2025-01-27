using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis;

public class DiscoverCommand
{
    public string ConnectionString { get; protected set; }
    public string? Perspective { get; set; }
    public string? Path { get; set; }
    public string? MeasureGroup { get; set; }
    public DiscoveryTarget Target { get; protected set; }
    public string? Function { get; set; }

    public DiscoverCommand(DiscoveryTarget target, string connectionString)
        => (ConnectionString, Target) = (connectionString, target);
}
