using NBi.GenbiL.Stateful;
using NBi.Xml;
using NBi.Xml.SerializationOption;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

    protected internal SettingsXml Include(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var str = reader.ReadToEnd();
        var standalone = XmlDeserializeFromString<SettingsStandaloneXml>(str);
        var settings = new SettingsXml()
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
