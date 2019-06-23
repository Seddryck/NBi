using Moq;
using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.Calculation.Predicate.Numeric;
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
    public class SizeFilterTest
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
        [TestCase(100, false)]
        [TestCase(0, true)]
        public void Execute_MoreThan_Correct(int size, bool result)
        {
            File.AppendAllText(Path.Combine(DirectoryName, "foo.txt"), ".");
            var fileInfo = new FileInfo(Path.Combine(DirectoryName, "foo.txt"));
            var filter = new SizeFilter(new NumericMoreThan(false, new LiteralScalarResolver<decimal>(size)));

            Assert.That(filter.Execute(fileInfo), Is.EqualTo(result));
        }

        [Test]
        [TestCase(100, true)]
        [TestCase(0, false)]
        public void Execute_LessThan_Correct(int size, bool result)
        {
            File.AppendAllText(Path.Combine(DirectoryName, "foo.txt"), ".");
            var fileInfo = new FileInfo(Path.Combine(DirectoryName, "foo.txt"));
            var filter = new SizeFilter(new NumericLessThan(false, new LiteralScalarResolver<decimal>(size)));

            Assert.That(filter.Execute(fileInfo), Is.EqualTo(result));
        }
    }
}
