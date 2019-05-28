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
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.IO.Commands;

namespace NBi.Testing.Integration.Core.Decoration.IO.Commands
{
    public class CopyPatternCommandTest
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

        [Test]
        [TestCase("*.*", 5)]
        [TestCase("*.txt", 4)]
        [TestCase("foo-*.txt", 3)]
        [TestCase("foo-?.txt", 2)]
        [TestCase("foo-0.txt", 1)]
        public void GetFiles_Pattern_CorrectCount(string pattern, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var copyPatternArgs = Mock.Of<ICopyPatternCommandArgs>
            (
                c => c.Pattern == new LiteralScalarResolver<string>(pattern)
                && c.SourcePath == new LiteralScalarResolver<string>(DirectoryName)
                && c.DestinationPath == new LiteralScalarResolver<string>(CopyDirectoryName)
            );

            var command = new CopyPatternCommand(copyPatternArgs);
            command.Execute();

            var dir = new DirectoryInfo(CopyDirectoryName);
            Assert.That(dir.GetFiles().Count(), Is.EqualTo(count));
        }
    }
}
