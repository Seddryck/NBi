using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Process;
using System.IO;

namespace NBi.Testing.Integration.Core.Process
{
    public class ProcessRunCommandTest
    {
        #region setup & cleanup

        private const string BATCH_FILE = "MyBatch.cmd";
        private const string INVALID_BATCH_FILE = "MyInvalidBatch.cmd";
        private const string TARGET_FILE = "output_file.txt";
        private string FullPath { get; set; }
        private string FullPathInvalid { get; set; }

        [SetUp]
        public void Setup()
        {
            if (File.Exists(BATCH_FILE))
                File.Delete(BATCH_FILE);

            FullPath = DiskOnFile.CreatePhysicalFile(BATCH_FILE, "NBi.Testing.Integration.Core.Resources." + BATCH_FILE);

            if (File.Exists(INVALID_BATCH_FILE))
                File.Delete(INVALID_BATCH_FILE);

            FullPathInvalid = DiskOnFile.CreatePhysicalFile(INVALID_BATCH_FILE, "NBi.Testing.Integration.Core.Resources." + INVALID_BATCH_FILE);

            if (!File.Exists(FullPath))
                throw new FileNotFoundException("OUps");
            else
                Console.WriteLine("BATCH: " + Path.GetFullPath(BATCH_FILE));

            if (File.Exists(TARGET_FILE))
                File.Delete(TARGET_FILE);
        }

        #endregion

        [Test]
        public void Execute_ExistingBatchWithoutArguments_Executed()
        {
            var processInfo = Mock.Of<IRunCommand>
            (
                c =>c.Argument == string.Empty
                  && c.FullPath == FullPath
                  && c.TimeOut == 1000
            );

            var command = new RunCommand(processInfo);
            command.Execute();

            Assert.That(File.Exists(TARGET_FILE), Is.True);
        }

        [Test]
        public void Execute_InvalidBatchWithoutArguments_Exception()
        {
            var processInfo = Mock.Of<IRunCommand>
            (
                c => c.Argument == string.Empty
                  && c.FullPath == FullPathInvalid
                  && c.TimeOut == 1000
            );

            var command = new RunCommand(processInfo);
            Assert.Throws<InvalidProgramException>(() => command.Execute());
        }

        [Test]
        public void Execute_InvalidBatchWithoutArgumentsNoWait_Success()
        {
            var processInfo = Mock.Of<IRunCommand>
            (
                c => c.Argument == string.Empty
                  && c.FullPath == FullPath
                  && c.TimeOut == 0
            );

            var command = new RunCommand(processInfo);
            command.Execute();
            Assert.Pass();
        }
    }
}
