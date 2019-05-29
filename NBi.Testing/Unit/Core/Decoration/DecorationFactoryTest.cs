using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NBi.Xml.Decoration.Command;
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

namespace NBi.Testing.Unit.Core.Decoration.DataEngineering
{
    [TestFixture]
    public class DecorationFactoryTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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

        private IDecorationCommandArgs GetCommandArgsMock(Type type)
        {
            switch (type)
            {
                case Type x when x == typeof(IBatchRunCommandArgs): return Mock.Of<IBatchRunCommandArgs>(m => m.ConnectionString == ConnectionStringReader.GetSqlClient());
                case Type x when x == typeof(ILoadCommandArgs): return Mock.Of<ILoadCommandArgs>(m => m.ConnectionString == ConnectionStringReader.GetSqlClient());
                case Type x when x == typeof(IResetCommandArgs): return Mock.Of<IResetCommandArgs>(m => m.ConnectionString == ConnectionStringReader.GetSqlClient());
                case Type x when x == typeof(IEtlRunCommandArgs): return Mock.Of<IEtlRunCommandArgs>();
                case Type x when x == typeof(IConnectionWaitCommandArgs): return Mock.Of<IConnectionWaitCommandArgs>();
                case Type x when x == typeof(IDeleteCommandArgs): return Mock.Of<IDeleteCommandArgs>();
                case Type x when x == typeof(IDeletePatternCommandArgs): return Mock.Of<IDeletePatternCommandArgs>();
                case Type x when x == typeof(IDeleteExtensionCommandArgs): return Mock.Of<IDeleteExtensionCommandArgs>();
                case Type x when x == typeof(ICopyCommandArgs): return Mock.Of<ICopyCommandArgs>();
                case Type x when x == typeof(ICopyPatternCommandArgs): return Mock.Of<ICopyPatternCommandArgs>();
                case Type x when x == typeof(ICopyExtensionCommandArgs): return Mock.Of<ICopyExtensionCommandArgs>();
                case Type x when x == typeof(IKillCommandArgs): return Mock.Of<IKillCommandArgs>();
                case Type x when x == typeof(IRunCommandArgs): return Mock.Of<IRunCommandArgs>();
                case Type x when x == typeof(IStartCommandArgs): return Mock.Of<IStartCommandArgs>();
                case Type x when x == typeof(IStopCommandArgs): return Mock.Of<IStopCommandArgs>();
                case Type x when x == typeof(IWaitCommandArgs): return Mock.Of<IWaitCommandArgs>();
                case Type x when x == typeof(IParallelCommandArgs): return Mock.Of<IParallelCommandArgs>();
                case Type x when x == typeof(ISequentialCommandArgs): return Mock.Of<ISequentialCommandArgs>();
                default: throw new ArgumentOutOfRangeException();
            }
        }

        [Test]
        [TestCase(typeof(IBatchRunCommandArgs), typeof(BatchRunCommand))]
        [TestCase(typeof(ILoadCommandArgs), typeof(BulkLoadCommand))]
        [TestCase(typeof(IResetCommandArgs), typeof(TruncateCommand))]
        [TestCase(typeof(IEtlRunCommandArgs), typeof(EtlRunCommand))]
        [TestCase(typeof(IConnectionWaitCommandArgs), typeof(ConnectionWaitCommand))]
        [TestCase(typeof(IDeleteCommandArgs), typeof(DeleteCommand))]
        [TestCase(typeof(IDeletePatternCommandArgs), typeof(DeletePatternCommand))]
        [TestCase(typeof(IDeleteExtensionCommandArgs), typeof(DeleteExtensionCommand))]
        [TestCase(typeof(ICopyCommandArgs), typeof(CopyCommand))]
        [TestCase(typeof(ICopyPatternCommandArgs), typeof(CopyPatternCommand))]
        [TestCase(typeof(ICopyExtensionCommandArgs), typeof(CopyExtensionCommand))]
        [TestCase(typeof(IKillCommandArgs), typeof(KillCommand))]
        [TestCase(typeof(IRunCommandArgs), typeof(RunCommand))]
        [TestCase(typeof(IStartCommandArgs), typeof(StartCommand))]
        [TestCase(typeof(IStopCommandArgs), typeof(StopCommand))]
        [TestCase(typeof(IWaitCommandArgs), typeof(WaitCommand))]
        [TestCase(typeof(IParallelCommandArgs), typeof(ParallelCommand))]
        [TestCase(typeof(ISequentialCommandArgs), typeof(SequentialCommand))]
        public void Get_IDecorationCommandArgs_CorrectCommand(Type argsType, Type commandType)
        {
            var args = GetCommandArgsMock(argsType);

            var factory = new DecorationFactory();
            var command = factory.Instantiate(args);

            Assert.That(command, Is.TypeOf(commandType));
        }

        private IDecorationConditionArgs GetConditionArgsMock(Type type)
        {
            switch (type)
            {
                case Type x when x == typeof(IRunningConditionArgs): return Mock.Of<IRunningConditionArgs>();
                default: throw new ArgumentOutOfRangeException();
            }
        }

        [Test]
        [TestCase(typeof(IRunningConditionArgs), typeof(RunningCondition))]
        public void Get_IDecorationConditionArgs_CorrectCondition(Type argsType, Type conditionType)
        {
            var args = GetConditionArgsMock(argsType);

            var factory = new DecorationFactory();
            var command = factory.Instantiate(args);

            Assert.That(command, Is.TypeOf(conditionType));
        }

    }
}
