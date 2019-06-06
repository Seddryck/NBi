using NBi.Core;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Transformation.Transformer.Native.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.Transformation.Transformer.Native
{
    [TestFixture]
    public class FileTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void CreateDirectory()
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);
        }

        [TearDown]
        public void DeleteDirectory()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToSize_Valid(string basePath)
        {
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);

            var filename = string.IsNullOrEmpty(basePath) ? existingFile : existingFile.Replace(basePath, string.Empty);
            var function = new FileToSize(basePath);
            var result = function.Evaluate(filename);
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToCreationDate_Valid(string basePath)
        {
            var dt = DateTime.Now;
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetCreationTime(existingFile, dt);

            var filename = string.IsNullOrEmpty(basePath) ? existingFile : existingFile.Replace(basePath, string.Empty);
            var function = new FileToCreationDateTime(basePath);
            var result = function.Evaluate(filename);
            Assert.That(result, Is.EqualTo(dt));
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToCreationDateUtc_Valid(string basePath)
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetCreationTime(existingFile, dt);

            var filename = string.IsNullOrEmpty(basePath) ? existingFile : existingFile.Replace(basePath, string.Empty);
            var function = new FileToCreationDateTimeUtc(basePath);
            var result = function.Evaluate(filename);
            Assert.That(result, Is.EqualTo(dt.Add(offset)));
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToUpdateDateTime_Valid(string basePath)
        {
            var dt = DateTime.Now;
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetLastWriteTime(existingFile, dt);

            var filename = string.IsNullOrEmpty(basePath) ? existingFile : existingFile.Replace(basePath, string.Empty);
            var function = new FileToUpdateDateTime(basePath);
            var result = function.Evaluate(filename);
            Assert.That(result, Is.EqualTo(dt));
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToUpdateDateTimeUtc_Valid(string basePath)
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetLastWriteTime(existingFile, dt);

            var filename = string.IsNullOrEmpty(basePath) ? existingFile : existingFile.Replace(basePath, string.Empty);
            var function = new FileToUpdateDateTimeUtc(basePath);
            var result = function.Evaluate(filename);
            Assert.That(result, Is.EqualTo(dt.Add(offset)));
        }

        [Test]
        [TestCase("")]
        [TestCase(@"Temp\")]
        public void Execute_PathToUpdateDateTimeUtcNotExistingFile_ExternalDependencyNotFound(string basePath)
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var notExistingFile = $@"{DirectoryName}NotExistingText.txt";

            var filename = string.IsNullOrEmpty(basePath) ? notExistingFile : notExistingFile.Replace(basePath, string.Empty);
            var function = new FileToUpdateDateTimeUtc(basePath);
            var ex = Assert.Throws<ExternalDependencyNotFoundException>(() => function.Evaluate(filename));
            Assert.That(ex.Message, Is.Not.StringContaining(@"Temp\Temp"));
            Assert.That(ex.Message, Is.Not.StringContaining(@"\\"));
            Assert.That(ex.Message, Is.StringContaining(@":\"));
            Assert.That(ex.Message, Is.StringEnding(@".txt'."));
        }
    }
}
