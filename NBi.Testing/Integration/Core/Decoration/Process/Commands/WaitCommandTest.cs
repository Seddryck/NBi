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
using System.Diagnostics;

namespace NBi.Testing.Integration.Core.Decoration.Process.Commands
{
    public class WaitCommandTest
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
        public void Execute_WaitOneSecond_DelayOfOnesecond()
        {
            var waitArgs = Mock.Of<IWaitCommandArgs>
            (
                c => c.MilliSeconds == new LiteralScalarResolver<int>("1000")
            );
            var command = new WaitCommand(waitArgs);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            command.Execute();
            stopWatch.Stop();
            Console.WriteLine($"Visible wait equal to { stopWatch.ElapsedMilliseconds} ms");
            Assert.That(stopWatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(1000));
        }
    }
}
