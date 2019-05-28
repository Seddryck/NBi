using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core;

namespace NBi.Testing.Integration
{
    public class DirectoryInfoExtensionsTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
            Directory.CreateDirectory(DirectoryName);
        }

        [Test]
        [TestCase(".txt", 4)]
        [TestCase(".csv", 1)]
        [TestCase(".txt;.csv", 5)]
        public void GetFilesByExtensions_SingleExtension_CorrectCount(string ext, int count)
        {
            var extensions = ext.Split(';');
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv", "foo-0.cmd" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(dir.GetFilesByExtensions(extensions).Count(), Is.EqualTo(count));
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

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(dir.GetFiles(pattern, SearchOption.AllDirectories).Count(), Is.EqualTo(count));
        }

        [Test]
        [TestCase("*.*", 6)]
        [TestCase("*.txt", 5)]
        [TestCase("foo-*.txt", 4)]
        [TestCase("foo-?.txt", 3)]
        [TestCase("foo-0.txt", 2)]
        public void GetFiles_PatternWithSubdirectories_CorrectCount(string pattern, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");
            Directory.CreateDirectory($@"{DirectoryName}\Sub");
            File.AppendAllText(Path.Combine($@"{DirectoryName}\Sub", "foo-0.txt"), ".");

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(dir.GetFiles(pattern, SearchOption.AllDirectories).Count(), Is.EqualTo(count));
        }
    }
}
