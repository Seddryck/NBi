using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Core.Database
{
    [TestFixture]
    public class DataSetComparerStructureTest
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.user.config");
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.config");
            }
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void ValidateStructure_SameStructure_Success()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";

            var ds = new DataSetComparer(_connectionString, _connectionString);
            var res = ds.ValidateStructure(sql);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));

        }
        
        [Test]
        public void ValidateStructure_SameStructureAndTableAlias_Success()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU, Label FROM Product Prd;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));

        }

        [Test]
        public void ValidateStructure_FieldRenamed_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU AS Sku, Label FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Object named \"ProductSKU\" was missing or not correctly positionned in actual result, \"Sku\" was found at its place."));

        }

        [Test]
        public void ValidateStructure_TypeChanged_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT CAST(ProductID AS VARCHAR(10)) AS ProductID, ProductSKU, Label FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Object named \"ProductID\" was defined as \"int\" in expected result and \"string\" in actual result."));

        }

        [Test]
        public void ValidateStructure_FieldMissing_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.Not.Empty);

        }

        [Test]
        public void ValidateStructure_FieldTooMuch_Failed()
        {
            var sql = "SELECT ProductID, ProductSKU FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU, Label FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.Not.Empty);

        }

        [Test]
        public void ValidateStructure_QueryDifferentButSameStructure_Success()
        {
            var sql = "SELECT ProductID, ProductSKU AS k, Label FROM Product;";
            var sql2 = "SELECT CAST(RIGHT(ProductSKU,1) AS int) AS ProductID, CAST(ProductSKU AS VARCHAR(10)) AS k, LEFT(Label, 5) AS Label FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
        }

        [Test]
        public void ValidateStructure_QueryWithBracketsForFields_Success()
        {
            var sql = "SELECT ProductID, ProductSKU, Label FROM Product;";
            var sql2 = "SELECT ProductID, ProductSKU, [Label] FROM Product;";

            var ds = new DataSetComparer(_connectionString, sql, _connectionString);
            var res = ds.ValidateStructure(sql2);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
        }

    }
}
