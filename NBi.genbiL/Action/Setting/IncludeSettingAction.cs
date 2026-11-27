using NBi.GenbiL.Stateful;
using NBi.Model;
using NBi.Model.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Setting;

public class IncludeSettingAction : Serializer, ISettingAction
{
    public string Filename { get; set; }

    public IncludeSettingAction(string filename) => Filename = filename;

    public void Execute(GenerationState state)
    {
        using var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
        var settings = Include(stream);

        state.Settings.Defaults.Clear();
        foreach (var defaultSetting in settings.Defaults)
            state.Settings.Defaults.Add(defaultSetting);

        state.Settings.References.Clear();
        foreach (var refSetting in settings.References)
            state.Settings.References.Add(refSetting);

        state.Settings.ParallelizeQueries = settings.ParallelizeQueries;
        state.Settings.CsvProfile = settings.CsvProfile;
    }

    protected internal SuiteSettings Include(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var str = reader.ReadToEnd();
        var standalone = XmlDeserializeFromString<SettingsStandalone>(str);
        var settings = new SuiteSettings()
        {
            Defaults = standalone.Defaults,
            References = standalone.References,
            ParallelizeQueries = standalone.ParallelizeQueries,
            CsvProfile = standalone.CsvProfile,
        };
        return settings;
    }

    public string Display => $"Include settings from '{Filename}'";
}
