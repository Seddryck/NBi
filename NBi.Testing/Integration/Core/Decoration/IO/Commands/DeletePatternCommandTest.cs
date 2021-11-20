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
    public class DeletePatternCommandTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
            Directory.CreateDirectory(DirectoryName);

        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
        }

        [Test]
        [TestCase("*.*", 0)]
        [TestCase("*.txt", 1)]
        [TestCase("foo-*.txt", 2)]
        [TestCase("foo-?.txt", 3)]
        [TestCase("foo-0.txt", 4)]
        public void GetFiles_Pattern_CorrectCount(string pattern, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var deletePatternArgs = Mock.Of<IoDeletePatternCommandArgs>
            (
                c => c.Pattern == new LiteralScalarResolver<string>(pattern)
                && c.Path == new LiteralScalarResolver<string>(DirectoryName)
            );

            var command = new DeletePatternCommand(deletePatternArgs);
            command.Execute();

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(dir.GetFiles().Count(), Is.EqualTo(count));
        }
    }
}
