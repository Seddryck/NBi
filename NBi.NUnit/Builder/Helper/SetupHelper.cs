using ImpromptuInterface;
using NBi.Core;
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
            switch (xmlCommand)
            {
                case SqlRunXml sqlRun: return BuildDataEngineeringBatchRun(sqlRun);
                case EtlRunXml etlRun: return BuildDataEngineeringEtlRun(etlRun);
                case ConnectionWaitXml connectionWait: return BuildDataEngineeringConnectionWait(connectionWait);
                case TableLoadXml load: return BuildDataEngineeringTableLoad(load);
                case TableResetXml reset: return BuildDataEngineeringTableReset(reset);
                case FileDeleteXml fileDelete: return BuildIoDelete(fileDelete);
                case FileDeletePatternXml deletePattern: return BuildIoDeletePattern(deletePattern);
                case FileDeleteExtensionXml deleteExtension: return BuildIoDeleteExtension(deleteExtension);
                case FileCopyXml fileCopy: return BuildIoCopy(fileCopy);
                case FileCopyPatternXml filePattern: return BuildIoCopyPattern(filePattern);
                case FileCopyExtensionXml fileExtension: return BuildIoCopyExtension(fileExtension);
                case ExeKillXml exeKill: return BuildProcessKill(exeKill);
                case ExeRunXml exeRun: return BuildProcessRun(exeRun);
                case ServiceStartXml serviceStart: return BuildProcessStart(serviceStart);
                case ServiceStopXml serviceStop: return BuildProcessStop(serviceStop);
                case WaitXml wait: return BuildProcessWait(wait);
                case CustomCommandXml custom: return BuildProcessCustom(custom);
                case CommandGroupXml group: return BuildGroup(group.Guid, group.Commands, group.Parallel, group.RunOnce);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private IBatchRunCommandArgs BuildDataEngineeringBatchRun(SqlRunXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Name = helper.InstantiateResolver<string>(xml.Name),
                Path = helper.InstantiateResolver<string>(xml.Path),
                xml.Settings?.BasePath,
                Version = helper.InstantiateResolver<string>(xml.Version),
                xml.ConnectionString,
            };
            return args.ActLike<IBatchRunCommandArgs>();
        }

        private IEtlRunCommandArgs BuildDataEngineeringEtlRun(EtlRunXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Name = helper.InstantiateResolver<string>(xml.Name),
                Path = helper.InstantiateResolver<string>(xml.Path),
                Version = helper.InstantiateResolver<string>(xml.Version),
            };
            return args.ActLike<IEtlRunCommandArgs>();
        }

        private IConnectionWaitCommandArgs BuildDataEngineeringConnectionWait(ConnectionWaitXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                xml.ConnectionString,
                TimeOut = helper.InstantiateResolver<int>(xml.TimeOut),
            };
            return args.ActLike<IConnectionWaitCommandArgs>();
        }
        private ILoadCommandArgs BuildDataEngineeringTableLoad(TableLoadXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                TableName = helper.InstantiateResolver<string>(xml.TableName),
                FileName = helper.InstantiateResolver<string>(xml.InternalFileName),
                xml.ConnectionString
            };
            return args.ActLike<ILoadCommandArgs>();
        }

        private IResetCommandArgs BuildDataEngineeringTableReset(TableResetXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                TableName = helper.InstantiateResolver<string>(xml.TableName),
                xml.ConnectionString
            };
            return args.ActLike<IResetCommandArgs>();
        }

        private IDeleteCommandArgs BuildIoDelete(FileDeleteXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Name = helper.InstantiateResolver<string>(xml.FileName),
                Path = helper.InstantiateResolver<string>(xml.Path),
                xml.Settings?.BasePath
            };
            return args.ActLike<IDeleteCommandArgs>();
        }
        private IDeletePatternCommandArgs BuildIoDeletePattern(FileDeletePatternXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Path = helper.InstantiateResolver<string>(xml.Path),
                Pattern = helper.InstantiateResolver<string>(xml.Pattern),
                xml.Settings?.BasePath
            };
            return args.ActLike<IDeletePatternCommandArgs>();
        }

        private IDeleteExtensionCommandArgs BuildIoDeleteExtension(FileDeleteExtensionXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Path = helper.InstantiateResolver<string>(xml.Path),
                Extension = helper.InstantiateResolver<string>(xml.Extension),
                xml.Settings?.BasePath
            };
            return args.ActLike<IDeleteExtensionCommandArgs>();
        }

        private ICopyCommandArgs BuildIoCopy(FileCopyXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                SourceName = helper.InstantiateResolver<string>(xml.FileName),
                SourcePath = helper.InstantiateResolver<string>(xml.SourcePath),
                DestinationName = helper.InstantiateResolver<string>(xml.FileName),
                DestinationPath = helper.InstantiateResolver<string>(xml.DestinationPath),
                xml.Settings?.BasePath
            };
            return args.ActLike<ICopyCommandArgs>();
        }

        private ICopyPatternCommandArgs BuildIoCopyPattern(FileCopyPatternXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                SourcePath = helper.InstantiateResolver<string>(xml.SourcePath),
                DestinationPath = helper.InstantiateResolver<string>(xml.DestinationPath),
                Pattern = helper.InstantiateResolver<string>(xml.Pattern),
                xml.Settings?.BasePath
            };
            return args.ActLike<ICopyPatternCommandArgs>();
        }

        private ICopyExtensionCommandArgs BuildIoCopyExtension(FileCopyExtensionXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                SourcePath = helper.InstantiateResolver<string>(xml.SourcePath),
                DestinationPath = helper.InstantiateResolver<string>(xml.DestinationPath),
                Extension = helper.InstantiateResolver<string>(xml.Extension),
                xml.Settings?.BasePath
            };
            return args.ActLike<ICopyExtensionCommandArgs>();
        }

        private IKillCommandArgs BuildProcessKill(ExeKillXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                ProcessName = helper.InstantiateResolver<string>(xml.ProcessName),
            };
            return args.ActLike<IKillCommandArgs>();
        }

        private IRunCommandArgs BuildProcessRun(ExeRunXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                Name = helper.InstantiateResolver<string>(xml.Name),
                Path = helper.InstantiateResolver<string>(xml.Path),
                xml.Settings?.BasePath,
                Argument = helper.InstantiateResolver<string>(xml.Argument),
                TimeOut = helper.InstantiateResolver<int>(xml.TimeOut),
            };
            return args.ActLike<IRunCommandArgs>();
        }

        private IStartCommandArgs BuildProcessStart(ServiceStartXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                ServiceName = helper.InstantiateResolver<string>(xml.ServiceName),
                TimeOut = helper.InstantiateResolver<int>(xml.TimeOut),
            };
            return args.ActLike<IStartCommandArgs>();
        }

        private IStopCommandArgs BuildProcessStop(ServiceStopXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                ServiceName = helper.InstantiateResolver<string>(xml.ServiceName),
                TimeOut = helper.InstantiateResolver<int>(xml.TimeOut),
            };
            return args.ActLike<IStopCommandArgs>();
        }

        private IWaitCommandArgs BuildProcessWait(WaitXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                MilliSeconds = helper.InstantiateResolver<int>(xml.MilliSeconds),
            };
            return args.ActLike<IWaitCommandArgs>();
        }

        private ICustomCommandArgs BuildProcessCustom(CustomCommandXml xml)
        {
            var helper = new ScalarHelper(serviceLocator, new Context(variables));
            var args = new
            {
                xml.Guid,
                AssemblyPath = helper.InstantiateResolver<string>(xml.AssemblyPath),
                TypeName = helper.InstantiateResolver<string>(xml.TypeName),
                Parameters = xml.Parameters.ToDictionary(x => x.Name, y => helper.InstantiateResolver<object>(y.StringValue)),
            };
            return args.ActLike<ICustomCommandArgs>();
        }

        private IGroupCommandArgs BuildGroup(Guid guid, IEnumerable<DecorationCommandXml> xmlCommands, bool isParallel, bool runOnce)
        {
            var commands = Execute(xmlCommands).ToList();
            var args = new {
                Guid = guid,
                RunOnce = runOnce,
                Commands = commands,
            };

            switch (isParallel)
            {
                case true: return args.ActLike<IParallelCommandArgs>();
                default: return args.ActLike<ISequentialCommandArgs>();
            }
        }
    }
}
