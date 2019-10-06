using NBi.Core;
using NBi.Core.Assemblies.Decoration;
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
            var helper = new ScalarHelper(serviceLocator, new Context(variables));

            return new CustomConditionArgs(
                helper.InstantiateResolver<string>(custom.AssemblyPath),
                helper.InstantiateResolver<string>(custom.TypeName),
                custom.Parameters.ToDictionary(x => x.Name, y => helper.InstantiateResolver<object>(y.StringValue) as IScalarResolver)
            );
        }

        private IDecorationConditionArgs BuildServiceRunning(ServiceRunningXml serviceRunning)
        {
            var scalarHelper = new ScalarHelper(serviceLocator, new Context(variables));
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
            public IScalarResolver<string> AssemblyPath { get; }

            public IScalarResolver<string> TypeName { get; }

            public IReadOnlyDictionary<string, IScalarResolver> Parameters { get; }

            public CustomConditionArgs(IScalarResolver<string> assemblyPath, IScalarResolver<string> typeName, IDictionary<string, IScalarResolver> parameters)
                => (AssemblyPath, TypeName, Parameters)
                = (assemblyPath, typeName, new ReadOnlyDictionary<string, IScalarResolver>(parameters));
        }
    }
}
