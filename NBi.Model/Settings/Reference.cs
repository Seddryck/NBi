using NBi.Model.Items.Format;
using NBi.Model.Items;
using System;

namespace NBi.Model.Settings;

public class Reference
{
    public string Name { get; set; }

    public ConnectionString ConnectionString { get; set; }

    [Obsolete("Replaced by connection-string")]
    public ConnectionString ConnectionStringOld
    {
        get => ConnectionString;
        set { ConnectionString = value; }
    }

    public bool ConnectionStringSpecified
    {
        get { return !string.IsNullOrEmpty(ConnectionString.Inline) || ConnectionString.Environment != null; }
        set { return; }
    }

    public string Regex { get; set; }

    public NumericFormat NumericFormat { get; set; }

    public CurrencyFormat CurrencyFormat { get; set; }

    public ReportBase Report { get; set; }

    public EtlBase Etl { get; set; }

    public Reference()
    {
        ConnectionString = new ConnectionString();
    }
}
