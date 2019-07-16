using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action.Consumable;
using NBi.Xml.Settings;
using NBi.Xml.Variables;

namespace NBi.GenbiL.Stateful
{
    public class GenerationState
    {
        public CaseCollection CaseCollection { get; }
        public ICollection<string> Templates { get; }
        public SettingsXml Settings { get; }
        public TestListManager List { get; }
        public TestSuiteManager Suite { get; }
        public IDictionary<string, object> Consumables { get; }
        public IDictionary<string, GlobalVariableXml> Variables { get; }

        public GenerationState()
        {
            CaseCollection = new CaseCollection();
            Templates = new List<string>();
            Settings = new SettingsXml();
            List = new TestListManager();
            Suite = new TestSuiteManager();
            Consumables = new Dictionary<string, object>();
            (new AutoConsumableAction(true)).Execute(this);
            Variables = new Dictionary<string, GlobalVariableXml>();
        }
    }
}
