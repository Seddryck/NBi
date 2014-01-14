using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service;

namespace NBi.GenbiL
{
    public class GenerationState
    {
        public TestCasesManager TestCases { get; private set; }
        public TemplateManager Template { get; private set; }
        public SettingsManager Settings { get; private set; }
        public TestListManager List { get; private set; }
        public TestSuiteManager Suite { get; private set; }

        public GenerationState()
        {
            TestCases = new TestCasesManager();
            Template = new TemplateManager();
            Settings = new SettingsManager();
            List = new TestListManager();
            Suite = new TestSuiteManager();
        }
    }
}
