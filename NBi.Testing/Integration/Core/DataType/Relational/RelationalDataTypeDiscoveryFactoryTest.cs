using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NBi.Core.DataType;
using NBi.Core.DataType.Relational;
using System.Data.SqlClient;

namespace NBi.Testing.Integration.Core.DataType.Relational
{

    [TestFixture]
    [Category("Sql")]
    public class RelationalDataTypeDataTypeDiscoveryFactoryTest
    {
        [TestCase("BusinessEntityID", "int")]
        [TestCase("FirstName", "nvarchar")]
        [TestCase("rowguid", "uniqueidentifier")]
        [TestCase("ModifiedDate", "datetime")]
        [Test]
        public void Execute_ColumnInt_CorrectDataTypeName(string columnName, string typeName)
        {
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var factory = new RelationalDataTypeDiscoveryFactory(conn);
            var cmd = factory.Instantiate(Target.Columns,
                new CaptionFilter[] {
                    new CaptionFilter(Target.Perspectives,"Person"),
                    new CaptionFilter(Target.Tables,"Person"),
                    new CaptionFilter(Target.Columns,columnName),
                });

            var dataType = cmd.Execute();

            Assert.That(dataType.Name, Is.EqualTo(typeName));
        }


    }


}
