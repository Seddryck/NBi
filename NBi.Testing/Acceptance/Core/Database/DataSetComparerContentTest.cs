using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Database
{
    [TestFixture]
    public class DataSetComparerContentTest
    {

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
        public void ValidateStructure_SameQuery_Success()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";

            var ds = new DataSetComparer(ConnectionStringReader.Get(), ConnectionStringReader.Get());
            var res = ds.ValidateContent(sql);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
            
        }

        [Test]
        public void ValidateStructure_DifferentRowsCount_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU, Label FROM Product WHERE ProductID=1;";

            var ds = new DataSetComparer(ConnectionStringReader.Get(), sql, ConnectionStringReader.Get());
            var res = ds.ValidateContent(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Different number of rows, 2 expected and was 1"));
        }

        [Test]
        public void ValidateStructure_DifferentRowsReturnedNumericMismatch_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product WHERE ProductID=2;";
            var sql2 = "SELECT ProductID, ProductSKU, Label FROM Product WHERE ProductID=1;";

            var ds = new DataSetComparer(ConnectionStringReader.Get(), sql, ConnectionStringReader.Get());
            var res = ds.ValidateContent(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("At row 1, numeric value of column \"ProductID\" are different in both datasets, expected was \"2\" and actual was \"1\"."));
        }

        [Test]
        public void ValidateStructure_DifferentRowsReturnedAlphaMismatch_Failed()
        {
            var sql = "SELECT 1, ProductSKU, Label FROM Product WHERE ProductID=2;";
            var sql2 = "SELECT ProductID, ProductSKU, Label FROM Product WHERE ProductID=1;";

            var ds = new DataSetComparer(ConnectionStringReader.Get(), sql, ConnectionStringReader.Get());
            var res = ds.ValidateContent(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("At row 1, value of column \"ProductSKU\" are different in both datasets, expected was \"SED125\" and actual was \"PRD001\"."));
        }
        
       

    }
}
