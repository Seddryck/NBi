using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service;
using NBi.Xml;

namespace NBi.GenbiL.Stateful
{
    public class GenerationState
    {
        public TestCaseSetCollectionState TestCaseSetCollection { get; private set; }
        public TemplateState Template { get; private set; }
        public SettingsState Settings { get; private set; }
        public TestSuiteXml Suite { get; private set; }
        public ConnectionStringManager ConnectionStrings { get; private set; }

        public GenerationState()
        {
            TestCaseSetCollection = new TestCaseSetCollectionState();
            Template = new TemplateState();
            Settings = new SettingsState();
            Suite = new TestSuiteXml();
        }
    }
}
