#region Using directives

using NUnit.Framework;
using NBi.NUnit.Runtime;

#endregion

namespace NBi.Testing.Unit.NUnit.Runtime
{
    [TestFixture]
    public class TestSuiteTest
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
        public void GetOwnFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            //Buiding object used during test
            var testSuite = new TestSuite();
            
            //Call the method to test
            var filename = testSuite.GetOwnFilename();

            //Assertion
            Assert.That(filename, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

        [Test]
        public void GetSelfFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            //Buiding object used during test
            var testSuite = new TestSuite();

            //Call the method to test
            var name = testSuite.GetManifestName();

            //Assertion
            Assert.That(name, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

    }
}
