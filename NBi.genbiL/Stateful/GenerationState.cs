using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action.Consumable;
using NBi.GenbiL.Stateful.Tree;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;

namespace NBi.GenbiL.Stateful;

public class GenerationState
{
    public CaseCollection CaseCollection { get; }
    public ICollection<string> Templates { get; }
    public SettingsXml Settings { get; }
    public RootNode Suite { get; }
    public IDictionary<string, object> Consumables { get; }
    public IList<GlobalVariableXml> Variables { get; }

    public GenerationState()
    {
        CaseCollection = new CaseCollection();
        Templates = new List<string>();
        Suite = new RootNode();
        Settings = new SettingsXml();
        Variables = new List<GlobalVariableXml>();
        Consumables = new Dictionary<string, object>();
        (new AutoConsumableAction(true)).Execute(this);
        
    }
}
