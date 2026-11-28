using NBi.Core.Query.Client;

namespace NBi.Core.PowerBiDesktop;

public class PowerBiDesktopConnectionStringBuilder
{
    private string? connectionString;

    public void Build(string mainWindowTitle)
        => connectionString = BuildLocalConnectionString(mainWindowTitle);

    public string GetConnectionString()
        => connectionString ?? throw new InvalidOperationException();

    internal static string ToConnectionUrl(string name) => $"pbix:///{Uri.EscapeDataString(name)}";

    protected virtual string BuildLocalConnectionString(string name)
    {
        try
        {
            return DubUrlClientFactory.CreateUrl(ToConnectionUrl(name)).Parse();
        }
        catch (DubUrl.Rewriting.InvalidConnectionUrlException ex)
        {
            throw new ConnectionException(ex, $"PBIX = {name}");
        }
    }
}
