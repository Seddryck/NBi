#region Using directives
using NBi.Core.Analysis.Metadata;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Unit.Core.Analysis.Metadata
{
    [TestFixture]
    public class CubeMetadataTest
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
        public void FindMeasures_OneLetterExistingInThreeCaptions_ThreeMatches()
        {
            //Buiding object used during test
            var cm = new CubeMetadata();
            cm.Perspectives.Add(new Perspective("p"));
            cm.Perspectives["p"].MeasureGroups.Add(new MeasureGroup("mg"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m1]","m1","df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m2]", "m2", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m3]", "xm3x", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m4]", "4", "df"));

            //Call the method to test
            var res = cm.FindMeasures("m");

            //Assertion
            Assert.That(res.Perspectives.Count, Is.EqualTo(1));
            Assert.That(res.Perspectives["p"].MeasureGroups.Count, Is.EqualTo(1));
            Assert.That(res.Perspectives["p"].MeasureGroups["mg"].Measures.Count, Is.EqualTo(3));
        }

        [Test]
        public void FindMeasures_OneLetterExistingInNoCaptions_ZeroMatches()
        {
            //Buiding object used during test
            var cm = new CubeMetadata();
            cm.Perspectives.Add(new Perspective("p"));
            cm.Perspectives["p"].MeasureGroups.Add(new MeasureGroup("mg"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m1]", "m1", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m2]", "m2", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m3]", "xm3x", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m4]", "4", "df"));

            //Call the method to test
            var res = cm.FindMeasures("z");

            //Assertion
            Assert.That(res.Perspectives.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindMeasures_ComplexRegexMatchingTwoCaptions_TwoMatches()
        {
            //Buiding object used during test
            var cm = new CubeMetadata();
            cm.Perspectives.Add(new Perspective("p"));
            cm.Perspectives["p"].MeasureGroups.Add(new MeasureGroup("mg"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m1]", "m1", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m2]", "m2", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m3]", "xm3x", "df"));
            cm.Perspectives["p"].MeasureGroups["mg"].Measures.Add(new Measure("[m4]", "4", "df"));

            //Call the method to test
            var res = cm.FindMeasures("m[0-9]$");

            //Assertion
            Assert.That(res.Perspectives.Count, Is.EqualTo(1));
            Assert.That(res.Perspectives["p"].MeasureGroups.Count, Is.EqualTo(1));
            Assert.That(res.Perspectives["p"].MeasureGroups["mg"].Measures.Count, Is.EqualTo(2));
        }

    }
}
