using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Action.Variable;

namespace NBi.GenbiL
{
    public class GenerationState
    {
        public TestCaseCollectionManager TestCaseCollection { get; private set; }
        public ICollection<string> Templates { get; private set; }
        public SettingsManager Settings { get; private set; }
        public TestListManager List { get; private set; }
        public TestSuiteManager Suite { get; private set; }
        public IDictionary<string, object> Variables { get; private set; }

        public GenerationState()
        {
            TestCaseCollection = new TestCaseCollectionManager();
            Templates = new List<string>();
            Settings = new SettingsManager();
            List = new TestListManager();
            Suite = new TestSuiteManager();
            Variables = new Dictionary<string, object>();
            (new AutoVariableAction(true)).Execute(this);
        }
    }
}
