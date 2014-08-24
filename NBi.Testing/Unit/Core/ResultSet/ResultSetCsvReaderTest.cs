#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;

using NUnit.Framework;

#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class ResultSetCsvReaderTest
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
        public void Parse_ValidRaw_SplitCorrectly()
        {
            //Buiding object used during test
            var reader = new ResultSetCsvReader();
            string csv = "\"A\";\"B\";\"10\";\r\n\"C\";\"D\";\"11\";\r\n";

            //Call the method to test
            var rs = reader.Parse(csv);

            //Assertion
            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));

            Assert.That(rs.Rows[0].ItemArray[0], Is.EqualTo((object)"A"));
            Assert.That(rs.Rows[0].ItemArray[1], Is.EqualTo((object)"B"));
            Assert.That(rs.Rows[0].ItemArray[2], Is.EqualTo((object)"10"));

            Assert.That(rs.Rows[1].ItemArray[0], Is.EqualTo((object)"C"));
            Assert.That(rs.Rows[1].ItemArray[1], Is.EqualTo((object)"D"));
            Assert.That(rs.Rows[1].ItemArray[2], Is.EqualTo((object)"11"));

        }

    }
}
