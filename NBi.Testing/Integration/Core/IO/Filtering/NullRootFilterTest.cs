using NBi.Core.IO.File;
using NBi.Core.IO.Filtering;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.IO.Filtering
{
    public class NullRootFilterTest
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
        public void GetFiles_NullRoot_CorrectCount()
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var fileLister = new FileLister(DirectoryName);
            var filters = new List<IFileFilter>() { new NullRootFilter() };

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(fileLister.Execute(filters).Count(), Is.EqualTo(files.Count()));
        }
    }
}
