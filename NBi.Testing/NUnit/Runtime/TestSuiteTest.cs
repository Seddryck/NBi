#region Using directives

using NUnit.Framework;
using NBi.NUnit.Runtime;

#endregion

namespace NBi.Testing.NUnit.Runtime
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
        public void GetTestSuiteConfig_NoExistingConfig_ReturnDefaultValue()
        {
            //Buiding object used during test
            var obj = new TestSuite();
            

            //Call the method to test
            //var cfg = obj.GetTestSuiteConfig();

            //Assertion
            //Assert.That(cfg, Is.EqualTo("TestSuite.xml"));
        }

    }
}
