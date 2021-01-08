using NBi.Core;
using NBi.Core.Assemblies.Decoration;
using NBi.Core.Decoration;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Injection;
using NBi.Extensibility.Resolving;
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
        private readonly IDictionary<string, IVariable> variables;

        public ConditionHelper(ServiceLocator serviceLocator, IDictionary<string, IVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.variables = variables;
        }

        public IDecorationConditionArgs Execute(object condition)
        {
            switch (condition)
            {
                case CustomConditionXml custom: return BuildCustomCondition(custom);
                case ServiceRunningConditionXml serviceRunning: return BuildServiceRunning(serviceRunning);
                case FileExistsConditionXml fileExists: return BuildFileExists(fileExists);
                case FolderExistsConditionXml folderExists: return BuildFolderExists(folderExists);
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

        private IDecorationConditionArgs BuildServiceRunning(ServiceRunningConditionXml serviceRunning)
        {
            var scalarHelper = new ScalarHelper(serviceLocator, new Context(variables));
            return new RunningArgs(
                scalarHelper.InstantiateResolver<string>(serviceRunning.ServiceName)
                , scalarHelper.InstantiateResolver<int>(serviceRunning.TimeOut)
            );
        }

        private IDecorationConditionArgs BuildFileExists(FileExistsConditionXml fileExists)
        {
            var scalarHelper = new ScalarHelper(serviceLocator, new Context(variables));
            return new FileExistsConditionArgs(
                serviceLocator.BasePath
                , scalarHelper.InstantiateResolver<string>(fileExists.Path)
                , scalarHelper.InstantiateResolver<string>(fileExists.Name)
                , scalarHelper.InstantiateResolver<bool>(fileExists.NotEmpty)
            );
        }

        private IDecorationConditionArgs BuildFolderExists(FolderExistsConditionXml folderExists)
        {
            var scalarHelper = new ScalarHelper(serviceLocator, new Context(variables));
            return new FolderExistsConditionArgs(
                serviceLocator.BasePath
                , scalarHelper.InstantiateResolver<string>(folderExists.Path)
                , scalarHelper.InstantiateResolver<string>(folderExists.Name)
                , scalarHelper.InstantiateResolver<bool>(folderExists.NotEmpty)
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
