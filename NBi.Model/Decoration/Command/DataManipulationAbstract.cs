using System;
using System.Linq;
using NBi.Model.Settings;

namespace NBi.Model.Decoration.Command;

public class DataManipulationAbstract : DecorationCommand
{
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
}
