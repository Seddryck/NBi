using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.IO.File;
using NBi.Core.IO.Filtering;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.IO.Filtering
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

            File.SetCreationTime(Path.Combine(DirectoryName, "foo-0.txt"), DateTime.Now.AddDays(-3));
            File.SetLastWriteTime(Path.Combine(DirectoryName, "foo-0.txt"), DateTime.Now.AddDays(-1));
            File.SetCreationTime(Path.Combine(DirectoryName, "foo-01.txt"), DateTime.Now.AddDays(-1));
            File.SetLastWriteTime(Path.Combine(DirectoryName, "foo-01.txt"), DateTime.Now.AddDays(-1));

            var fileLister = new FileLister(DirectoryName);
            var filters = new List<IFileFilter>()
            {
                new PatternRootFilter("foo-*.txt"),
                new CreationDateTimeFilter(new DateTimeMoreThan(false, DateTime.Now.AddDays(-2)), false),
                new UpdateDateTimeFilter(new DateTimeMoreThan(false, DateTime.Now.AddHours(-1)), false),
            };

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(fileLister.Execute(filters).Count(), Is.EqualTo(1));
        }
    }
}
