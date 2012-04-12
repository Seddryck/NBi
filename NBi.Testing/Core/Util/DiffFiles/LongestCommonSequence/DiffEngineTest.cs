using System;
using System.Collections.Generic;
using NBi.Core.Util.DiffFiles.LongestCommonSequence;
using NUnitFmk = NUnit.Framework;

#region Using directives

using NUnit.Framework;

#endregion

namespace NBi.Testing.Core.Util.DiffFiles.LongestCommonSequence
{
    [TestFixture]
    public class DiffEngineTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void ProcessDiff_SameStrings_NoChange()
        {
            //Buiding object used during test
            var a = "a,b,c,d,e,f,g,h";
            var b = "a,b,c,d,e,f,g,h";

            var orig = new List<TextLine>(); new List<string>(a.Split(new char[] { ',' })).ForEach(line => orig.Add(new TextLine(line)));
            var comp = new List<TextLine>(); new List<string>(b.Split(new char[] { ',' })).ForEach(line => comp.Add(new TextLine(line)));

            var engine = new DiffEngine();

            //Call the method to test
            var res = engine.ProcessDiff(orig, comp);

            //Assertion
            Assert.That(engine.DiffReport().Count, Is.EqualTo(1));
            Assert.That(engine.DiffReport()[0], Is.EqualTo(DiffResultSpan.CreateNoChange(0,0,8)));
        }

        [Test]
        public void ProcessDiff_OneDiff_ThreeSpanReplace()
        {
            //Buiding object used during test
            var a = "a,b,c,d,e,f,g,h";
            var b = "a,b,z,d,e,f,g,h";

            var orig = new List<TextLine>(); new List<string>(a.Split(new char[] { ',' })).ForEach(line => orig.Add(new TextLine(line)));
            var comp = new List<TextLine>(); new List<string>(b.Split(new char[] { ',' })).ForEach(line => comp.Add(new TextLine(line)));

            var engine = new DiffEngine();

            //Call the method to test
            var res = engine.ProcessDiff(orig, comp);

            //Assertion
            Assert.That(engine.DiffReport().Count, Is.EqualTo(3));
            Assert.That(engine.DiffReport()[0], Is.EqualTo(DiffResultSpan.CreateNoChange(0, 0, 2)));
            Assert.That(engine.DiffReport()[1], Is.EqualTo(DiffResultSpan.CreateReplace(2, 2, 1)));
            Assert.That(engine.DiffReport()[2], Is.EqualTo(DiffResultSpan.CreateNoChange(3, 3, 5)));
        }

        [Test]
        public void ProcessDiff_OneNew_ThreeSpanAddDestination()
        {
            //Buiding object used during test
            var a = "a,b,c,d,e,f,g,h";
            var b = "a,b,c,z,d,e,f,g,h";

            var orig = new List<TextLine>(); new List<string>(a.Split(new char[] { ',' })).ForEach(line => orig.Add(new TextLine(line)));
            var comp = new List<TextLine>(); new List<string>(b.Split(new char[] { ',' })).ForEach(line => comp.Add(new TextLine(line)));

            var engine = new DiffEngine();

            //Call the method to test
            var res = engine.ProcessDiff(orig, comp);

            //Assertion
            Assert.That(engine.DiffReport().Count, Is.EqualTo(3));
            Assert.That(engine.DiffReport()[0], Is.EqualTo(DiffResultSpan.CreateNoChange(0, 0, 3)));
            Assert.That(engine.DiffReport()[1], Is.EqualTo(DiffResultSpan.CreateAddDestination(3, 1)));
            Assert.That(engine.DiffReport()[2], Is.EqualTo(DiffResultSpan.CreateNoChange(4, 3, 5)));
        }

        [Test]
        public void ProcessDiff_OneNew_ThreeSpanDeleted()
        {
            //Buiding object used during test
            var a = "a,b,c,d,e,f,g,h";
            var b = "a,b,d,e,f,g,h";

            var orig = new List<TextLine>(); new List<string>(a.Split(new char[] { ',' })).ForEach(line => orig.Add(new TextLine(line)));
            var comp = new List<TextLine>(); new List<string>(b.Split(new char[] { ',' })).ForEach(line => comp.Add(new TextLine(line)));

            var engine = new DiffEngine();

            //Call the method to test
            var res = engine.ProcessDiff(orig, comp);
            //Assertion
            Assert.That(engine.DiffReport().Count, Is.EqualTo(3));
            Assert.That(engine.DiffReport()[0], Is.EqualTo(DiffResultSpan.CreateNoChange(0, 0, 2)));
            Assert.That(engine.DiffReport()[1], Is.EqualTo(DiffResultSpan.CreateDeleteSource(2, 1)));
            Assert.That(engine.DiffReport()[2], Is.EqualTo(DiffResultSpan.CreateNoChange(2, 3, 5)));
        }

    }
}
