using System.Collections.Generic;
using NBi.NUnit;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class TestXmlTest
    {
        protected string ConnectionString
        {
            get
            {
                //If available use the user file
                if (System.IO.File.Exists("ConnectionString.user.config"))
                {
                    return System.IO.File.ReadAllText("ConnectionString.user.config");
                }
                else if (System.IO.File.Exists("ConnectionString.config"))
                {
                    return System.IO.File.ReadAllText("ConnectionString.config");
                }

                return null;
            }
        }

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
        
        [Test]
        public void Test_Play_Success()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() { new SyntacticallyCorrectXml() },
                TestCases = new List<TestCaseXml>() 
                { new TestCaseXml() 
                    {
                        InlineQuery = "SELECT * FROM Product;",  
                        ConnectionString =  ConnectionString
                    } 
                }
            };

            t.Play();

            Assert.Pass();
        }
    }
}