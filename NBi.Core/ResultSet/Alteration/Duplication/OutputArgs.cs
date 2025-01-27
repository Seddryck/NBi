using NBi.Core.Injection;
using NBi.Core.ResultSet.Alteration.Duplication.OuputStrategies;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet.Alteration.Duplication;

public class OutputArgs
{
    public IColumnIdentifier Identifier { get; set; }
    public IOutputStrategy? Strategy { get; set; }

    public OutputArgs(IColumnIdentifier identifier, OutputClass value)
        => (Identifier, Strategy) = (identifier, Instantiate(value));

    protected virtual IOutputStrategy? Instantiate(OutputClass value)
    {
        return value switch
        {
            OutputClass.Index => new IndexOutputStrategy(),
            OutputClass.Total => new TotalOutputStrategy(),
            OutputClass.IsOriginal => new IsOriginalOutputStrategy(),
            OutputClass.IsDuplicable => new IsDuplicableOutputStrategy(),
            _ => null,
        };
    }
}

public class OutputScriptArgs : OutputArgs
{
    public OutputScriptArgs(ServiceLocator serviceLocator, Context context, IColumnIdentifier identifier, LanguageType language, string script)
        : base(identifier, OutputClass.Script)
        => Strategy = new ScriptOuputStrategy(serviceLocator, context, script, language);
}

public class OutputValueArgs : OutputArgs
{
    public OutputValueArgs(IColumnIdentifier identifier, string value)
        : base(identifier, OutputClass.Static)
        => Strategy = new ValueOutputStrategy(value);
}

public enum OutputClass
{
    [XmlEnum(Name = "static")]
    Static = 0,
    [XmlEnum(Name = "script")]
    Script = 1,
    [XmlEnum(Name = "index")]
    Index = 2,
    [XmlEnum(Name = "total")]
    Total = 3,
    [XmlEnum(Name = "is-original")]
    IsOriginal = 4,
    [XmlEnum(Name = "is-duplicable")]
    IsDuplicable = 5,
}
