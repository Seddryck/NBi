using Moq;
using NBi.Core.Calculation.Predicate.DateTime;
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
    public class UpdateDateTimeFilterTest
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
        [TestCase(1, false)]
        [TestCase(-1, true)]
        public void Execute_MoreThan_Correct(int shift, bool result)
        {
            File.AppendAllText(Path.Combine(DirectoryName, "foo.txt"), ".");
            var fileInfo = new FileInfo(Path.Combine(DirectoryName, "foo.txt"));
            var filter = new UpdateDateTimeFilter(new DateTimeMoreThan(false, DateTime.Now.AddDays(shift)), false);

            Assert.That(filter.Execute(fileInfo), Is.EqualTo(result));
        }

        [Test]
        [TestCase(-1, false)]
        [TestCase(1, true)]
        public void Execute_LessThan_Correct(int shift, bool result)
        {
            File.AppendAllText(Path.Combine(DirectoryName, "foo.txt"), ".");
            var fileInfo = new FileInfo(Path.Combine(DirectoryName, "foo.txt"));
            var filter = new UpdateDateTimeFilter(new DateTimeLessThan(false, DateTime.Now.AddDays(shift)), false);

            Assert.That(filter.Execute(fileInfo), Is.EqualTo(result));
        }
    }
}
