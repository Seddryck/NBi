using NBi.Model.Items;

namespace NBi.Model.Settings;

public class ConnectionString
{
    public string Inline { get; set; }
    public EnvironmentVariable Environment { get; set; }

    public string GetValue()
        => !string.IsNullOrEmpty(Inline) ? Inline
            : Environment != null ? EnvironmentVariable.GetValue(Environment.Name)!
            : string.Empty;
}