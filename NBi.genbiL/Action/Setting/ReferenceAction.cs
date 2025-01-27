using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting;

public class ReferenceAction : ISettingAction
{
    public string Name { get; set; }
    public string Variable { get; set; }
    public string Value { get; set; }

    public ReferenceAction(string name, string variable, string value)
    {
        Name = name;
        Variable= variable;
        Value = value;
    }

    public void Execute(GenerationState state)
    {
        if (Variable.ToLower() != "ConnectionString".ToLower())
            throw new ArgumentException("Currently you must define the variable as ConnectionString. Other options are not supported!");
        
        var newReference = state.Settings.References.SingleOrDefault(d => d.Name == Name);
        if (newReference == null)
        {
            newReference = new ReferenceXml() { Name = Name };
            state.Settings.References.Add(newReference);
        }
        newReference.ConnectionString = new ConnectionStringXml() { Inline = Value };
    }

    public string Display => $"Create reference named 'Name' with value for {Variable} and defining it to '{Value}'";
}
