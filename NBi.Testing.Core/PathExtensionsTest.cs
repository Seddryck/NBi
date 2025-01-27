using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing;

public class PathExtensionsTest
{
    [Test]
    [TestCase(@"C:\Temp\", "foo.txt")]
    [TestCase(@"C:\Temp", "foo.txt")]
    public void Combine_RootedPathFileName_Valid(string path, string filename)
    {
        var fullPath = PathExtensions.CombineOrRoot(@"C:\Windows\", path, filename);
        Assert.That(fullPath, Is.EqualTo(@"C:\Temp\foo.txt"));
    }

    [Test]
    [TestCase(@"Bar", "foo.txt")]
    [TestCase(@"Bar\", "foo.txt")]
    public void Combine_NotRootedPathFileName_Valid(string path, string filename)
    {
        var fullPath = PathExtensions.CombineOrRoot(@"C:\Temp", path, filename);
        Assert.That(fullPath, Is.EqualTo(@"C:\Temp\Bar\foo.txt"));
    }
}
