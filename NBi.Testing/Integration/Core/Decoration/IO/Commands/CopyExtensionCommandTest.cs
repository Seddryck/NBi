using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core;
using NBi.Core.Decoration.IO;
using Moq;
using NBi.Core.Decoration.IO.Commands;
using NBi.Core.Scalar.Resolver;

namespace NBi.Testing.Integration.Core.Decoration.IO.Commands
{
    public class CopyExtensionCommandTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }
        private string CopyDirectoryName { get => $@"Temp\{GetType().Name}-copy\"; }

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
            Directory.CreateDirectory(DirectoryName);

            if (Directory.Exists(CopyDirectoryName))
                Directory.Delete(CopyDirectoryName, true);
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);

            if (Directory.Exists(CopyDirectoryName))
                Directory.Delete(CopyDirectoryName, true);
        }

        [Test]
        [TestCase(".txt", 4)]
        [TestCase(".csv", 1)]
        [TestCase(".txt;.csv", 5)]
        public void Execute_SomeFiles_CorrectCount(string ext, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv", "foo-0.cmd" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var copyExtensionArgs = Mock.Of<IoCopyExtensionCommandArgs>
            (
                c => c.Extension == new LiteralScalarResolver<string>(ext)
                && c.SourcePath == new LiteralScalarResolver<string>(DirectoryName)
                && c.DestinationPath == new LiteralScalarResolver<string>(CopyDirectoryName)
            );

            var command = new CopyExtensionCommand(copyExtensionArgs);
            command.Execute();

            var dir = new DirectoryInfo(CopyDirectoryName);
            Assert.That(dir.GetFiles().Count(), Is.EqualTo(count));
        }
    }
}
