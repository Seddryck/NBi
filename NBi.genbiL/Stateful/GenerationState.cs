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
        public TestCaseCollectionManager TestCaseCollection { get; private set; }
        public ICollection<string> Templates { get; private set; }
        public SettingsXml Settings { get; private set; }
        public TestListManager List { get; private set; }
        public TestSuiteManager Suite { get; private set; }
        public IDictionary<string, object> Consumables { get; private set; }
        public IDictionary<string, GlobalVariableXml> Variables { get; private set; }

        public GenerationState()
        {
            TestCaseCollection = new TestCaseCollectionManager();
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
