#region Using directives
using System.Collections.Generic;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.Xml
{
    [TestFixture]
    public class ContainsConstraintXmlTest
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
        public void Initialize_ConstraintWithQuery_QueryExecutedAndItemsLoaded()
        {
            //Buiding object used during test
            var ctr = new ContainsXml();
            ctr.Items = new List<string>() { "alpha", "beta" };
            ctr.Query = new QueryXml()
            {
                ConnectionString = ConnectionStringReader.GetSqlClient(),
                InlineQuery = "SELECT 'gamma' UNION ALL SELECT 'delta'"
            };
            
            //Method under test
            ctr.Initialize();

            //Test conclusion            
            Assert.That(ctr.Items.Count, Is.EqualTo(4));
            Assert.That(ctr.Items, Has.Member("alpha"));
            Assert.That(ctr.Items, Has.Member("beta"));
            Assert.That(ctr.Items, Has.Member("gamma"));
            Assert.That(ctr.Items, Has.Member("delta"));

        }
    }
}
