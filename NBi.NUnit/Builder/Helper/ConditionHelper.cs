using NBi.Core;
using NBi.Core.Assemblies;
using NBi.Core.Decoration;
using NBi.Core.Decoration.Process;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
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
        private readonly ServiceLocator serviceLocator;
        private readonly IDictionary<string, ITestVariable> variables;

        public ConditionHelper(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.variables = variables;
        }

        public IDecorationConditionArgs Execute(object condition)
        {
            switch (condition)
            {
                case CustomConditionXml custom: return BuildCustomCondition(custom);
                case ServiceRunningXml serviceRunning: return BuildServiceRunning(serviceRunning);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private IDecorationConditionArgs BuildCustomCondition(CustomConditionXml custom)
        {
            var parameters = new Dictionary<string, object>();
            custom.Parameters.ForEach(p => parameters.Add(p.Name, p.StringValue));

            return new CustomConditionArgs(custom.AssemblyPath, custom.TypeName, parameters);
        }

        private IDecorationConditionArgs BuildServiceRunning(ServiceRunningXml serviceRunning)
        {
            var scalarHelper = new ScalarHelper(serviceLocator, variables);
            return new RunningArgs(
                scalarHelper.InstantiateResolver<string>(serviceRunning.ServiceName)
                , scalarHelper.InstantiateResolver<int>(serviceRunning.TimeOut)
            );
        }

        private class RunningArgs : IRunningConditionArgs
        {
            public RunningArgs(IScalarResolver<string> serviceName, IScalarResolver<int> timeOut)
            {
                ServiceName = serviceName;
                TimeOut = timeOut;
            }

            public IScalarResolver<string> ServiceName { get; }

            public IScalarResolver<int> TimeOut { get; }
        }

        private class CustomConditionArgs : ICustomConditionArgs
        {
            public string AssemblyPath { get; }

            public string TypeName { get; }

            public IReadOnlyDictionary<string, object> Parameters { get; }

            public CustomConditionArgs(string assemblyPath, string typeName, IDictionary<string, object> parameters)
                => (AssemblyPath, TypeName, Parameters)
                = (assemblyPath, typeName, new ReadOnlyDictionary<string, object>(parameters));
        }
    }
}
