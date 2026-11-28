using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action.Consumable;
using NBi.GenbiL.Stateful.Tree;
using NBi.Model;
using NBi.Model.Settings;
using NBi.Model.Variables;

namespace NBi.GenbiL.Stateful;

public class GenerationState
{
    public CaseCollection CaseCollection { get; }
    public ICollection<string> Templates { get; }
    public SuiteSettings Settings { get; }
    public RootNode Suite { get; }
    public IDictionary<string, object> Consumables { get; }
    public IList<GlobalVariable> Variables { get; }

    public GenerationState()
    {
        CaseCollection = new CaseCollection();
        Templates = new List<string>();
        Suite = new RootNode();
        Settings = new SuiteSettings();
        Variables = new List<GlobalVariable>();
        Consumables = new Dictionary<string, object>();
        (new AutoConsumableAction(true)).Execute(this);
    }
}
