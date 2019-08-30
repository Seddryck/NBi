using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.GenbiL;
using NBi.IO.Genbi.Dto;

namespace NBi.UI.Genbi.Stateful
{
    class TestSuiteState
    {
        public BindingList<string> Variables { get; private set; }
        public BindingList<string> ConnectionStringNames { get; private set; }
        public DataTable TestCases { get; private set; }
        public string Template { get; private set; }
        public LargeBindingList<Test> Tests { get; private set; }
        public BindingList<Setting> Settings { get; private set; }
        public IDictionary<string, object> GlobalVariables { get; private set; }


        public TestSuiteState()
        {
            Variables = new BindingList<string>();
            ConnectionStringNames = new BindingList<string>();
            TestCases = new DataTable();
            Tests = new LargeBindingList<Test>();
            Settings = new BindingList<Setting>
            {
                new Setting() { Name = "Default - System-under-test" },
                new Setting() { Name = "Default - Assert" }
            };
            GlobalVariables = new GenerationState().Consumables;
        }
    }
}
