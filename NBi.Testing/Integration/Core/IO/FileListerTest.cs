using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.IO.File;
using NBi.Core.IO.Filtering;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.IO.Filtering
{
    public class FileListerTest
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
        public void Execute_PatternCreationUpdate_CorrectCount()
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var fileLister = new FileLister(DirectoryName);
            var filters = new List<IFileFilter>()
            {
                new PatternRootFilter("foo-*.txt"),
            };

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(fileLister.Execute(filters).Count(), Is.EqualTo(3));
        }
    }
}
