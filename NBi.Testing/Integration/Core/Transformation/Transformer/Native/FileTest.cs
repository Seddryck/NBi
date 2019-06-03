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
        public void Execute_PathToSize_Valid()
        {
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);

            var function = new FileToSize();
            var result = function.Evaluate(existingFile);
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void Execute_PathToCreationDate_Valid()
        {
            var dt = DateTime.Now;
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetCreationTime(existingFile, dt);

            var function = new FileToCreationDateTime();
            var result = function.Evaluate(existingFile);
            Assert.That(result, Is.EqualTo(dt));
        }

        [Test]
        public void Execute_PathToCreationDateUtc_Valid()
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetCreationTime(existingFile, dt);

            var function = new FileToCreationDateTimeUtc();
            var result = function.Evaluate(existingFile);
            Assert.That(result, Is.EqualTo(dt.Add(offset)));
        }

        [Test]
        public void Execute_PathToUpdateDateTime_Valid()
        {
            var dt = DateTime.Now;
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetLastWriteTime(existingFile, dt);

            var function = new FileToUpdateDateTime();
            var result = function.Evaluate(existingFile);
            Assert.That(result, Is.EqualTo(dt));
        }

        [Test]
        public void Execute_PathToUpdateDateTimeUtc_Valid()
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var existingFile = $@"{DirectoryName}Text.txt";
            File.WriteAllText(existingFile, "a small text", Encoding.ASCII);
            File.SetLastWriteTime(existingFile, dt);

            var function = new FileToUpdateDateTimeUtc();
            var result = function.Evaluate(existingFile);
            Assert.That(result, Is.EqualTo(dt.Add(offset)));
        }

        [Test]
        public void Execute_PathToUpdateDateTimeUtcNotExistingFile_ExternalDependencyNotFound()
        {
            var dt = DateTime.Now;
            var offset = DateTime.UtcNow.Subtract(DateTime.Now);
            var notExistingFile = $@"{DirectoryName}NotExistingText.txt";

            var function = new FileToUpdateDateTimeUtc();
            Assert.Throws<ExternalDependencyNotFoundException>(() => function.Evaluate(notExistingFile));
        }
    }
}
