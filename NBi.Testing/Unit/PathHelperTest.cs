using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit
{
    public class PathHelperTest
    {
        [Test]
        [TestCase(@"C:\Temp\", "foo.txt")]
        [TestCase(@"C:\Temp", "foo.txt")]
        public void Combine_RootedPathFileName_Valid(string path, string filename)
        {
            var helper = new PathHelper();
            var fullPath = helper.Combine(@"C:\Windows\", path, filename);
            Assert.That(fullPath, Is.EqualTo(@"C:\Temp\foo.txt"));
        }

        [Test]
        [TestCase(@"Bar", "foo.txt")]
        [TestCase(@"Bar\", "foo.txt")]
        public void Combine_NotRootedPathFileName_Valid(string path, string filename)
        {
            var helper = new PathHelper();
            var fullPath = helper.Combine(@"C:\Temp", path, filename);
            Assert.That(fullPath, Is.EqualTo(@"C:\Temp\Bar\foo.txt"));
        }
    }
}
