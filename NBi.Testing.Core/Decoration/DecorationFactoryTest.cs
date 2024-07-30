using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.DataEngineering.Commands;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.DataEngineering.Commands.SqlServer;
using NBi.Core;
using NBi.Core.Decoration;
using NBi.Core.Decoration.IO;
using NBi.Core.Decoration.IO.Commands;
using NBi.Core.Decoration.Process;
using NBi.Core.Decoration.Process.Commands;
using NBi.Core.Decoration.Grouping.Commands;
using NBi.Core.Decoration.Grouping;
using NBi.Core.Decoration.Process.Conditions;
using NBi.Core.Assemblies;
using NBi.Core.Assemblies.Decoration;
using System.Reflection;
using NBi.Extensibility.Decoration;
using NBi.Extensibility.Decoration.DataEngineering;
using NBi.Core.Decoration.IO.Conditions;
using System.Collections.Generic;
using NBi.Testing;
using NBi.Extensibility.Resolving;
using System.Collections.ObjectModel;

namespace NBi.Core.Testing.Decoration.DataEngineering
{
    [TestFixture]
    public class DecorationFactoryTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        private IDecorationCommandArgs GetCommandArgsFake(Type type)
        {
            return type switch
            {
                Type x when x == typeof(SqlBatchRunCommandArgs) => new SqlBatchRunCommandArgs(Guid.NewGuid(), ConnectionStringReader.GetSqlClient(), new LiteralScalarResolver<string>("name"), new LiteralScalarResolver<string>("path"), "basePath", new LiteralScalarResolver<string>("version")),
                Type x when x == typeof(TableLoadCommandArgs) => new TableLoadCommandArgs(Guid.NewGuid(), ConnectionStringReader.GetSqlClient(), new LiteralScalarResolver<string>("tablename"), new LiteralScalarResolver<string>("pathname")),
                Type x when x == typeof(TableTruncateCommandArgs) => new TableTruncateCommandArgs(Guid.NewGuid(), ConnectionStringReader.GetSqlClient(), new LiteralScalarResolver<string>("tablename")),
                //case Type x when x == typeof(EtlRunCommandArgs): return new EtlRunCommandArgs(Guid.NewGuid(), );
                Type x when x == typeof(ConnectionWaitCommandArgs) => new ConnectionWaitCommandArgs(Guid.NewGuid(), ConnectionStringReader.GetSqlClient(), new LiteralScalarResolver<int>(100)),
                Type x when x == typeof(IoDeleteCommandArgs) => new IoDeleteCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("name"), "basePath", new LiteralScalarResolver<string>("path")),
                Type x when x == typeof(IoDeletePatternCommandArgs) => new IoDeletePatternCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("pattern"), "basePath", new LiteralScalarResolver<string>("path")),
                Type x when x == typeof(IoDeleteExtensionCommandArgs) => new IoDeleteExtensionCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("extension"), "basePath", new LiteralScalarResolver<string>("path")),
                Type x when x == typeof(IoCopyCommandArgs) => new IoCopyCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("sourceName"), new LiteralScalarResolver<string>("sourcePath"), new LiteralScalarResolver<string>("destinationName"), new LiteralScalarResolver<string>("destinationPath"), "basePath"),
                Type x when x == typeof(IoCopyPatternCommandArgs) => new IoCopyPatternCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("source"), new LiteralScalarResolver<string>("destinatin"), new LiteralScalarResolver<string>("pattern"), "basePath"),
                Type x when x == typeof(IoCopyExtensionCommandArgs) => new IoCopyExtensionCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("source"), new LiteralScalarResolver<string>("destinatin"), new LiteralScalarResolver<string>("extension"), "basePath"),
                Type x when x == typeof(ProcessKillCommandArgs) => new ProcessKillCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("name")),
                Type x when x == typeof(ProcessRunCommandArgs) => new ProcessRunCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("name"), new LiteralScalarResolver<string>("path"), "basePath", new LiteralScalarResolver<string>("name"), new LiteralScalarResolver<int>(100)),
                Type x when x == typeof(ServiceStartCommandArgs) => new ServiceStartCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("name"), new LiteralScalarResolver<int>(100)),
                Type x when x == typeof(ServiceStopCommandArgs) => new ServiceStopCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<string>("name"), new LiteralScalarResolver<int>(100)),
                Type x when x == typeof(WaitCommandArgs) => new WaitCommandArgs(Guid.NewGuid(), new LiteralScalarResolver<int>(100)),
                Type x when x == typeof(GroupParallelCommandArgs) => new GroupParallelCommandArgs(Guid.NewGuid(), true, []),
                Type x when x == typeof(GroupSequentialCommandArgs) => new GroupSequentialCommandArgs(Guid.NewGuid(), true, []),
                Type x when x == typeof(CustomCommandArgs) => new CustomCommandArgs
                                                        (
                                                            Guid.NewGuid(),
                                                            new LiteralScalarResolver<string>($@"{FileOnDisk.GetDirectoryPath()}\NBi.Core.Testing.dll"),
                                                            new LiteralScalarResolver<string>("NBi.Core.Testing.Resources.CustomCommand"),
                                                            new ReadOnlyDictionary<string, IScalarResolver>(new Dictionary<string, IScalarResolver>())
                                                        ),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        [Test]
        [TestCase(typeof(SqlBatchRunCommandArgs), typeof(BatchRunCommand))]
        [TestCase(typeof(TableLoadCommandArgs), typeof(BulkLoadCommand))]
        [TestCase(typeof(TableTruncateCommandArgs), typeof(TruncateCommand))]
        //[TestCase(typeof(EtlRunCommandArgs), typeof(EtlRunCommand))]
        [TestCase(typeof(ConnectionWaitCommandArgs), typeof(ConnectionWaitCommand))]
        [TestCase(typeof(IoDeleteCommandArgs), typeof(DeleteCommand))]
        [TestCase(typeof(IoDeletePatternCommandArgs), typeof(DeletePatternCommand))]
        [TestCase(typeof(IoDeleteExtensionCommandArgs), typeof(DeleteExtensionCommand))]
        [TestCase(typeof(IoCopyCommandArgs), typeof(CopyCommand))]
        [TestCase(typeof(IoCopyPatternCommandArgs), typeof(CopyPatternCommand))]
        [TestCase(typeof(IoCopyExtensionCommandArgs), typeof(CopyExtensionCommand))]
        [TestCase(typeof(ProcessKillCommandArgs), typeof(KillCommand))]
        [TestCase(typeof(ProcessRunCommandArgs), typeof(RunCommand))]
        [TestCase(typeof(ServiceStartCommandArgs), typeof(StartCommand))]
        [TestCase(typeof(ServiceStopCommandArgs), typeof(StopCommand))]
        [TestCase(typeof(WaitCommandArgs), typeof(WaitCommand))]
        [TestCase(typeof(GroupParallelCommandArgs), typeof(ParallelCommand))]
        [TestCase(typeof(GroupSequentialCommandArgs), typeof(SequentialCommand))]
        [TestCase(typeof(CustomCommandArgs), typeof(CustomCommand))]
        public void Get_IDecorationCommandArgs_CorrectCommand(Type argsType, Type commandType)
        {
            var args = GetCommandArgsFake(argsType);

            var factory = new DecorationFactory();
            var command = factory.Instantiate(args);

            Assert.That(command, Is.TypeOf(commandType));
        }

        private IDecorationConditionArgs GetConditionArgsMock(Type type)
        {
            return type switch
            {
                Type x when x == typeof(IRunningConditionArgs) => Mock.Of<IRunningConditionArgs>(),
                Type x when x == typeof(FolderExistsConditionArgs) => new FolderExistsConditionArgs(string.Empty, new LiteralScalarResolver<string>(""), new LiteralScalarResolver<string>(""), new LiteralScalarResolver<bool>(true)),
                Type x when x == typeof(FileExistsConditionArgs) => new FileExistsConditionArgs(string.Empty, new LiteralScalarResolver<string>(""), new LiteralScalarResolver<string>(""), new LiteralScalarResolver<bool>(true)),
                Type x when x == typeof(ICustomConditionArgs) => Mock.Of<ICustomConditionArgs>
                                        (
                                            y => y.AssemblyPath == new LiteralScalarResolver<string>($@"{FileOnDisk.GetDirectoryPath()}\NBi.Core.Testing.dll")
                                            && y.TypeName == new LiteralScalarResolver<string>("NBi.Core.Testing.Resources.CustomConditionTrue")
                                        ),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        [Test]
        [TestCase(typeof(IRunningConditionArgs), typeof(RunningCondition))]
        [TestCase(typeof(FolderExistsConditionArgs), typeof(FolderExistsCondition))]
        [TestCase(typeof(FileExistsConditionArgs), typeof(FileExistsCondition))]
        [TestCase(typeof(ICustomConditionArgs), typeof(CustomCondition))]
        public void Get_IDecorationConditionArgs_CorrectCondition(Type argsType, Type conditionType)
        {
            var args = GetConditionArgsMock(argsType);

            var factory = new DecorationFactory();
            var command = factory.Instantiate(args);

            Assert.That(command, Is.TypeOf(conditionType));
        }

    }
}
