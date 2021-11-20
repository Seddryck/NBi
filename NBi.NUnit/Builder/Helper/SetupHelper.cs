using NBi.Core.Assemblies.Decoration;
using NBi.Core.Decoration;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility.Decoration;
using NBi.Extensibility.Decoration.DataEngineering;
using NBi.Extensibility.Resolving;
using NBi.Xml.Decoration.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class SetupHelper
    {
        private readonly ServiceLocator serviceLocator;
        private readonly IDictionary<string, IVariable> variables;

        public SetupHelper(ServiceLocator serviceLocator, IDictionary<string, IVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.variables = variables;
        }

        public IEnumerable<IDecorationCommandArgs> Execute(IEnumerable<DecorationCommandXml> xmlCommands)
        {
            foreach (var xmlCommand in xmlCommands)
                yield return Execute(xmlCommand);
        }

        protected virtual IDecorationCommandArgs Execute(DecorationCommandXml xmlCommand)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));

            switch (xmlCommand)
            {
                case SqlRunXml sqlRun:
                    return new SqlBatchRunCommandArgs(
                        sqlRun.Guid,
                        sqlRun.ConnectionString,
                        helper.InstantiateResolver<string>(sqlRun.Name),
                        helper.InstantiateResolver<string>(sqlRun.Path),
                        sqlRun.Settings?.BasePath,
                        helper.InstantiateResolver<string>(sqlRun.Version)
                    );
                case EtlRunXml etlRun:
                    //return new EtlRunCommandArgs(
                    //    etlRun.Guid,

                    //);
                    return null;
                case ConnectionWaitXml connectionWait:
                    return new ConnectionWaitCommandArgs(
                        connectionWait.Guid,
                        connectionWait.ConnectionString,
                        helper.InstantiateResolver<int>(connectionWait.TimeOut)
                    );
                case TableLoadXml load:
                    return new TableLoadCommandArgs(
                        load.Guid,
                        load.ConnectionString,
                        helper.InstantiateResolver<string>(load.TableName),
                        helper.InstantiateResolver<string>(load.FileName)
                    );
                case TableResetXml reset:
                    return new TableTruncateCommandArgs(
                        reset.Guid,
                        reset.ConnectionString,
                        helper.InstantiateResolver<string>(reset.TableName)
                    );
                case FileDeleteXml ioDelete:
                    return new IoDeleteCommandArgs(
                        ioDelete.Guid,
                        helper.InstantiateResolver<string>(ioDelete.FileName),
                        ioDelete.Settings?.BasePath,
                        helper.InstantiateResolver<string>(ioDelete.Path)
                    );
                case FileDeletePatternXml ioDeletePattern:
                    return new IoDeletePatternCommandArgs(
                        ioDeletePattern.Guid,
                        helper.InstantiateResolver<string>(ioDeletePattern.Pattern),
                        ioDeletePattern.Settings?.BasePath,
                        helper.InstantiateResolver<string>(ioDeletePattern.Path)
                    );
                case FileDeleteExtensionXml ioDeleteExtension:
                    return new IoDeleteExtensionCommandArgs(
                        ioDeleteExtension.Guid,
                        helper.InstantiateResolver<string>(ioDeleteExtension.Extension),
                        ioDeleteExtension.Settings?.BasePath,
                        helper.InstantiateResolver<string>(ioDeleteExtension.Path)
                    );
                case FileCopyXml ioCopy:
                    return new IoCopyCommandArgs(
                        ioCopy.Guid,
                        helper.InstantiateResolver<string>(ioCopy.FileName),
                        helper.InstantiateResolver<string>(ioCopy.SourcePath),
                        helper.InstantiateResolver<string>(ioCopy.FileName),
                        helper.InstantiateResolver<string>(ioCopy.DestinationPath),
                        ioCopy.Settings?.BasePath
                    );
                case FileCopyPatternXml ioCopyPattern:
                    return new IoCopyPatternCommandArgs(
                        ioCopyPattern.Guid,
                        helper.InstantiateResolver<string>(ioCopyPattern.SourcePath),
                        helper.InstantiateResolver<string>(ioCopyPattern.DestinationPath),
                        helper.InstantiateResolver<string>(ioCopyPattern.Pattern),
                        ioCopyPattern.Settings?.BasePath
                    );
                case FileCopyExtensionXml ioCopyExtension:
                    return new IoCopyExtensionCommandArgs(
                        ioCopyExtension.Guid,
                        helper.InstantiateResolver<string>(ioCopyExtension.SourcePath),
                        helper.InstantiateResolver<string>(ioCopyExtension.DestinationPath),
                        helper.InstantiateResolver<string>(ioCopyExtension.Extension),
                        ioCopyExtension.Settings?.BasePath
                    );
                case ExeKillXml processKill:
                    return new ProcessKillCommandArgs(
                        processKill.Guid,
                        helper.InstantiateResolver<string>(processKill.ProcessName)
                    );
                case ExeRunXml processRun:
                    return new ProcessRunCommandArgs(
                        processRun.Guid,
                        helper.InstantiateResolver<string>(processRun.Name),
                        helper.InstantiateResolver<string>(processRun.Path),
                        processRun.Settings?.BasePath,
                        helper.InstantiateResolver<string>(processRun.Argument),
                        helper.InstantiateResolver<int>(processRun.TimeOut)
                    );
                case ServiceStartXml serviceStart:
                    return new ServiceStartCommandArgs(
                        serviceStart.Guid,
                        helper.InstantiateResolver<string>(serviceStart.ServiceName),
                        helper.InstantiateResolver<int>(serviceStart.TimeOut)
                    );
                case ServiceStopXml serviceStop:
                    return new ServiceStopCommandArgs(
                        serviceStop.Guid,
                        helper.InstantiateResolver<string>(serviceStop.ServiceName),
                        helper.InstantiateResolver<int>(serviceStop.TimeOut)
                    );
                case WaitXml wait:
                    return new WaitCommandArgs(
                        wait.Guid,
                        helper.InstantiateResolver<int>(wait.MilliSeconds)
                    );
                case CustomCommandXml custom:
                    return new CustomCommandArgs(
                        custom.Guid,
                        helper.InstantiateResolver<string>(custom.AssemblyPath),
                        helper.InstantiateResolver<string>(custom.TypeName),
                        custom.Parameters.ToDictionary(x => x.Name, y => (IScalarResolver)helper.InstantiateResolver<string>(y.StringValue))
                    );
                case CommandGroupXml group: 
                    switch(group.Parallel)
                    {
                        case true:
                            return new GroupParallelCommandArgs(
                                group.Guid,
                                group.RunOnce,
                                Execute(group.Commands).ToList()
                            );
                        default:
                            return new GroupSequentialCommandArgs(
                                group.Guid,
                                group.RunOnce,
                                Execute(group.Commands).ToList()
                            );
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }

    }
}
