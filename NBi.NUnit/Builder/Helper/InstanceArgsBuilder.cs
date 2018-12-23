using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class InstanceArgsBuilder
    {
        private readonly ServiceLocator serviceLocator;
        private readonly IDictionary<string, ITestVariable> globalVariables;

        private bool isSetup = false;
        private object obj = null;
        private SettingsXml settings = SettingsXml.Empty;

        private ResultSetAllRowsBuilder = null

        public InstanceArgsBuilder(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> globalVariables)
        {
            this.serviceLocator = serviceLocator;
            this.globalVariables = globalVariables;
        }

        public void Setup(SettingsXml settings)
        {
            this.settings = settings;
        }

        public void Setup(InstanceDefinitionXml definition)
        {
            this.obj = obj;
        }

        public void Build()
        {
            if ((obj as InstanceDefinitionXml).Variable != null)
            {
                var args = new SingleVariableInstanceArgs(
                    (obj as InstanceDefinitionXml).Variable.Name
                    (obj as InstanceDefinitionXml).Variable.ColumnType

                    , )
                
            }
        }

    }
}
