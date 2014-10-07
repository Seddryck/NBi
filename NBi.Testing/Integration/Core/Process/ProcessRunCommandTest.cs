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
    class ProcessRunCommandTest
    {
        [Test]
        public void Execute_ExistingBatchWithoutArguments_Executed()
        {
            var batch = @"MyBatch.cmd";
            if (File.Exists(batch))
                File.Delete(batch);

            var target = "output_file.txt";
            if (File.Exists(target))
                File.Delete(target);

            DiskOnFile.CreatePhysicalFile(batch, "NBi.Testing.Integration.Core.Resources." + batch);

            var processInfo = Mock.Of<IRunCommand>
            (
                c =>c.Argument == string.Empty
                  && c.FullPath == batch
                  && c.TimeOut == 1000
            );

            var command = new RunCommand(processInfo);
            command.Execute();

            Assert.That(File.Exists(target), Is.True);
        }

        [Test]
        public void Execute_InvalidBatchWithoutArguments_Exception()
        {
            var batch = @"MyInvalidBatch.cmd";
            if (File.Exists(batch))
                File.Delete(batch);

            DiskOnFile.CreatePhysicalFile(batch, "NBi.Testing.Integration.Core.Resources." + batch);

            var processInfo = Mock.Of<IRunCommand>
            (
                c => c.Argument == string.Empty
                  && c.FullPath == batch
                  && c.TimeOut == 1000
            );

            var command = new RunCommand(processInfo);
            Assert.Throws<InvalidProgramException>(() => command.Execute());
        }

        [Test]
        public void Execute_InvalidBatchWithoutArgumentsNoWait_Success()
        {
            var batch = @"MyInvalidBatch.cmd";
            if (File.Exists(batch))
                File.Delete(batch);

            DiskOnFile.CreatePhysicalFile(batch, "NBi.Testing.Integration.Core.Resources." + batch);

            var processInfo = Mock.Of<IRunCommand>
            (
                c => c.Argument == string.Empty
                  && c.FullPath == batch
                  && c.TimeOut == 0
            );

            var command = new RunCommand(processInfo);
            command.Execute();
            Assert.Pass();
        }
    }
}
