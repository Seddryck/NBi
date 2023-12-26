﻿using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Transformation.Transformer.Native.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Transformation.Transformer.Native
{
    [TestFixture]
    public class PathTest
    {
        [Test]
        [TestCase(@"C:\", "")]
        [TestCase(@"C:\Dir\", "")]
        [TestCase(@"C:\Dir\Child\", "")]
        [TestCase(@"C:\Dir\ChildFile", "ChildFile")]
        [TestCase(@"C:\Dir\Child\file.txt", "file.txt")]
        [TestCase(@"Dir\file.txt", "file.txt")]
        public void Execute_PathToFilename_Valid(string value, string expected)
        {
            var function = new PathToFilename(string.Empty);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(@"C:\", "")]
        [TestCase(@"C:\Dir\", "")]
        [TestCase(@"C:\Dir\Child\", "")]
        [TestCase(@"C:\Dir\ChildFile", "ChildFile")]
        [TestCase(@"C:\Dir\Child\file.txt", "file")]
        [TestCase(@"Dir\file.txt", "file")]
        public void Execute_PathToFilenameWithoutExtension_Valid(string value, string expected)
        {
            var function = new PathToFilenameWithoutExtension(string.Empty);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(@"C:\", "")]
        [TestCase(@"C:\Dir\", "")]
        [TestCase(@"C:\Dir\Child\", "")]
        [TestCase(@"C:\Dir\ChildFile", "")]
        [TestCase(@"C:\Dir\Child\file.txt", ".txt")]
        [TestCase(@"Dir\file.txt", @".txt")]
        public void Execute_PathToExtension_Valid(string value, string expected)
        {
            var function = new PathToExtension(string.Empty);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(@"C:\", @"C:\")]
        [TestCase(@"C:\Dir\", @"C:\")]
        [TestCase(@"C:\Dir\Child\", @"C:\")]
        [TestCase(@"C:\Dir\ChildFile", @"C:\")]
        [TestCase(@"C:\Dir\Child\file.txt", @"C:\")]
        [TestCase(@"Dir\file.txt", @"")]
        public void Execute_PathToRoot_Valid(string value, string expected)
        {
            var function = new PathToRoot(string.Empty);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(@"C:\", @"C:\")]
        [TestCase(@"C:\Dir\", @"C:\Dir\")]
        [TestCase(@"C:\Dir\Child\", @"C:\Dir\Child\")]
        [TestCase(@"C:\Dir\ChildFile", @"C:\Dir\")]
        [TestCase(@"C:\Dir\Child\file.txt", @"C:\Dir\Child\")]
        [TestCase(@"Dir\file.txt", @"Dir\")]
        public void Execute_PathToDirectory_Valid(string value, string expected)
        {
            var function = new PathToDirectory(string.Empty);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
