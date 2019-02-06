using NBi.Core;
using NBi.Core.Assemblies;
using NBi.Core.WindowsService;
using NBi.Xml.Decoration.Condition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class ConditionHelper
    {
        public IDecorationConditionMetadata Execute(object condition)
        {
            switch (condition)
            {
                case CustomConditionXml custom: return BuildCustomCondition(custom);
                case ServiceRunningXml serviceRunning: return BuildServiceRunning(serviceRunning);
                default:throw new ArgumentOutOfRangeException();
            }
        }

        private IDecorationConditionMetadata BuildCustomCondition(CustomConditionXml custom)
        {
            var parameters = new Dictionary<string, object>();
            custom.Parameters.ForEach(p => parameters.Add(p.Name, p.StringValue));

            return new CustomConditionMetadata(custom.AssemblyPath, custom.TypeName, parameters);
        }

        private IDecorationConditionMetadata BuildServiceRunning(ServiceRunningXml serviceRunning)
            => new WindowsServiceRunningMetadata(serviceRunning.ServiceName, serviceRunning.TimeOut);

        private class CustomConditionMetadata : ICustomConditionMetadata
        {
            public string AssemblyPath { get; }

            public string TypeName { get; }

            public IReadOnlyDictionary<string, object> Parameters { get; }

            public CustomConditionMetadata(string assemblyPath, string typeName, IDictionary<string, object> parameters)
                => (AssemblyPath, TypeName, Parameters)
                = (assemblyPath, typeName, new ReadOnlyDictionary<string, object>(parameters));
        }
    }
}
