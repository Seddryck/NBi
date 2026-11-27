using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Items;
using System.IO;
using NBi.Model.Settings;

namespace NBi.Model.Decoration.Command;

public class SqlRun : DecorationCommand
{
    public string Name { get; set; }

    public string Path { get; set; }

    public string Version { get; set; }

    public string SpecificConnectionString { get; set; }

    [Obsolete("Replaced by connection-string")]
    public string SpecificConnectionStringOld
    {
        get => SpecificConnectionString;
        set { SpecificConnectionString = value; }
    }

    public string ConnectionString
    {
        get
        {
            if (!string.IsNullOrEmpty(SpecificConnectionString) && SpecificConnectionString.StartsWith("@"))
                return Settings.GetReference(SpecificConnectionString.Remove(0, 1)).ConnectionString.GetValue();
            if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                return SpecificConnectionString;
            if (Settings != null && Settings.GetDefault(SuiteSettings.DefaultScope.Decoration) != null)
                return Settings.GetDefault(SuiteSettings.DefaultScope.Decoration).ConnectionString.GetValue();
            return string.Empty;
        }
    }

    public SqlRun()
    {
        Version = "SqlServer2014";
    }
}
