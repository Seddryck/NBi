using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service;

namespace NBi.GenbiL.Stateful
{
    public class GenerationState
    {
        public TestCaseSetCollectionState TestCaseSetCollection { get; private set; }
        public TemplateState Template { get; private set; }
        public SettingsState Settings { get; private set; }
        public TestListManager List { get; private set; }
        public TestSuiteManager Suite { get; private set; }

        public GenerationState()
        {
            TestCaseSetCollection = new TestCaseSetCollectionState();
            Template = new TemplateState();
            Settings = new SettingsState();
            List = new TestListManager();
            Suite = new TestSuiteManager();
        }
    }
}
