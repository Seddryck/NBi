using NBi.GenbiL.Stateful;
using NBi.Model;
using NBi.Model.Settings;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    protected virtual IEnumerable<GlobalVariable> ReadXml(string filename)
    {
        using var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
        return ReadXml(stream);
    }

    protected internal IEnumerable<GlobalVariable> ReadXml(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var str = reader.ReadToEnd();
        var standalone = XmlDeserializeFromString<GlobalVariablesStandalone>(str);
        var globalVariables = new List<GlobalVariable>();
        globalVariables = standalone.Variables;
        return globalVariables;
    }

    public string Display => $"Include variables from '{Filename}'";
}
