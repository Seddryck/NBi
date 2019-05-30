using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.Core.Decoration.Process;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.Process.Commands;

namespace NBi.Testing.Integration.Core.Decoration.Process.Commands
{
    public class RunCommandTest
    {
        #region setup & cleanup

        private const string BATCH_FILE = "MyBatch.cmd";
        private const string INVALID_BATCH_FILE = "MyInvalidBatch.cmd";
        private const string TARGET_FILE = "output_file.txt";
        private string Path { get; set; }

        [SetUp]
        public void Setup()
        {
            if (File.Exists(BATCH_FILE))
                File.Delete(BATCH_FILE);

            Path = System.IO.Path.GetDirectoryName(DiskOnFile.CreatePhysicalFile(BATCH_FILE, "NBi.Testing.Integration.Core.Resources." + BATCH_FILE));

            if (File.Exists(INVALID_BATCH_FILE))
                File.Delete(INVALID_BATCH_FILE);

            DiskOnFile.CreatePhysicalFile(INVALID_BATCH_FILE, "NBi.Testing.Integration.Core.Resources." + INVALID_BATCH_FILE);

            if (File.Exists(TARGET_FILE))
                File.Delete(TARGET_FILE);
        }

        #endregion

        [Test]
        public void Execute_ExistingBatchWithoutArguments_Executed()
        {
            var runArgs = Mock.Of<IRunCommandArgs>
            (
                c => c.Argument == new LiteralScalarResolver<string>(string.Empty)
                  && c.Path == new LiteralScalarResolver<string>(Path)
                  && c.Name == new LiteralScalarResolver<string>(BATCH_FILE)
                  && c.TimeOut == new LiteralScalarResolver<int>(1000)
            );

            var command = new RunCommand(runArgs);
            command.Execute();

            Assert.That(File.Exists(TARGET_FILE), Is.True);
        }

        [Test]
        public void Execute_InvalidBatchWithoutArguments_Exception()
        {
            var runArgs = Mock.Of<IRunCommandArgs>
            (
                c => c.Argument == new LiteralScalarResolver<string>(string.Empty)
                  && c.Path == new LiteralScalarResolver<string>(Path)
                  && c.Name == new LiteralScalarResolver<string>(INVALID_BATCH_FILE)
                  && c.TimeOut == new LiteralScalarResolver<int>(1000)
            );

            var command = new RunCommand(runArgs);
            Assert.Throws<InvalidProgramException>(() => command.Execute());
        }

        [Test]
        public void Execute_InvalidBatchWithoutArgumentsNoWait_Success()
        {
            var runArgs = Mock.Of<IRunCommandArgs>
            (
                c => c.Argument == new LiteralScalarResolver<string>(string.Empty)
                  && c.Path == new LiteralScalarResolver<string>(Path)
                  && c.Name == new LiteralScalarResolver<string>(BATCH_FILE)
                  && c.TimeOut == new LiteralScalarResolver<int>(0)
            );

            var command = new RunCommand(runArgs);
            command.Execute();
            Assert.Pass();
        }
    }
}
