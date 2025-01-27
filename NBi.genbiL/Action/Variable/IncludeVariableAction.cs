using NBi.Core.Variable;
using NBi.GenbiL.Stateful;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.GenbiL.Action.Variable;

public class IncludeVariableAction : Serializer, IVariableAction
{
    public string Filename { get; set; }

    public IncludeVariableAction(string filename)
    {
        Filename = filename;
    }

    public void Execute(GenerationState state)
    {
        var variables = ReadXml(Filename);
        foreach (var variable in variables)
            state.Variables.Add(variable);
    }

    protected virtual IEnumerable<GlobalVariableXml> ReadXml(string filename)
    {
        using var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
        return ReadXml(stream);
    }

    protected internal IEnumerable<GlobalVariableXml> ReadXml(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var str = reader.ReadToEnd();
        var standalone = XmlDeserializeFromString<GlobalVariablesStandaloneXml>(str);
        var globalVariables = new List<GlobalVariableXml>();
        globalVariables = standalone.Variables;
        return globalVariables;
    }

    public string Display => $"Include variables from '{Filename}'";
}
