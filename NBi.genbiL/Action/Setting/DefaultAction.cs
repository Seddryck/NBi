using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting;

public class DefaultAction : ISettingAction
{
    public DefaultType DefaultType { get; set; }
    public string Variable { get; set; }
    public string Value { get; set; }

    public DefaultAction(DefaultType defaultType, string variable, string value)
    {
        DefaultType = defaultType;
        Variable= variable;
        Value = value;
    }

    public void Execute(GenerationState state)
    {
        if (Variable.ToLower() != "ConnectionString".ToLower())
            throw new ArgumentException("Currently you must define the variable as ConnectionString. Other options are not supported!");

        var defaultScope = MapDefaultScope(DefaultType);
        state.Settings.GetDefault(defaultScope).ConnectionString = new ConnectionStringXml() { Inline = Value };
    }

    private SettingsXml.DefaultScope MapDefaultScope(DefaultType defaultValue)
    {
        switch (defaultValue)
        {
            case DefaultType.Everywhere: return SettingsXml.DefaultScope.Everywhere;
            case DefaultType.SystemUnderTest:return SettingsXml.DefaultScope.SystemUnderTest;
            case DefaultType.Assert:return SettingsXml.DefaultScope.Assert;
            case DefaultType.SetupCleanup: return SettingsXml.DefaultScope.Decoration;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public string Display => $"Create {GetLiteralForDefaulType(DefaultType)} default value for {Variable} and defining it to '{Value}'";

    private string GetLiteralForDefaulType(Action.DefaultType defaultType)
    {
        switch (defaultType)
        {
            case DefaultType.Everywhere: return "Everywhere";
            case DefaultType.SystemUnderTest: return "System-Under-Test";
            case DefaultType.Assert: return "Assert";
            case DefaultType.SetupCleanup: return "Setup-cleanup";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
