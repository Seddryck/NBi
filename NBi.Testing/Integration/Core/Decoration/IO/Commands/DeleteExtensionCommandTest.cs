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
using NBi.Extensibility.Resolving;

namespace NBi.Testing.Integration.Core.Decoration.IO.Commands
{
    public class DeleteExtensionCommandTest
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
        [TestCase(".txt", 2)]
        [TestCase(".csv", 5)]
        [TestCase(".txt;.csv", 1)]
        public void Execute_SomeFiles_CorrectCount(string ext, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv", "foo-0.cmd" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var deleteExtensionArgs = Mock.Of<IDeleteExtensionCommandArgs>
            (
                c => c.Extension == new LiteralScalarResolver<string>(ext)
                && c.Path == new LiteralScalarResolver<string>(DirectoryName)
            );

            var command = new DeleteExtensionCommand(deleteExtensionArgs);
            command.Execute();

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(dir.GetFiles().Count(), Is.EqualTo(count));
        }
    }
}
