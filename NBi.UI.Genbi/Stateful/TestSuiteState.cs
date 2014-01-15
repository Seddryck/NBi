using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.Service.Dto;

namespace NBi.UI.Genbi.Stateful
{
    class TestSuiteState
    {
        public BindingList<string> Variables { get; private set; }
        public DataTable TestCases { get; private set; }
        public string Template { get; private set; }
        public LargeBindingList<Test> Tests { get; private set; }
        public BindingList<Setting> Settings { get; private set; }

        public TestSuiteState()
        {
            Variables = new BindingList<string>();
            TestCases = new DataTable();
            Tests = new LargeBindingList<Test>();
            Settings = new BindingList<Setting>();
            Settings.Add(new Setting() { Name = "Default - System-under-test" });
            Settings.Add(new Setting() { Name = "Default - Assert" });
        }
    }
}
