using Microsoft.Data.Sqlite;
using NBi.Core.DataType;
using NBi.Core.Structure;
using NUnit.Framework;
using ColumnFilter = NBi.Core.DataType.CaptionFilter;
using DataTypeTarget = NBi.Core.DataType.Target;
using StructureFilter = NBi.Core.Structure.CaptionFilter;
using StructureTarget = NBi.Core.Structure.Target;

namespace NBi.Core.Testing.Query.Client;

public class DubUrlSchemaDiscoveryTest
{
    [Test]
    public void Discover_SqliteUrl_ProvidesTablesColumnsAndTypes()
    {
        var path = Path.GetTempFileName();
        try
        {
            using (var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = path, Pooling = false }.ToString()))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "create table sales (amount decimal(10, 2) not null);";
                command.ExecuteNonQuery();
            }

            var url = "sqlite:////" + new Uri(path).AbsolutePath.TrimStart('/') + "?Pooling=False";
            var structure = new StructureDiscoveryFactoryProvider().Instantiate(url);
            var tables = structure.Instantiate(StructureTarget.Tables, TargetType.Object,
                [new StructureFilter(StructureTarget.Perspectives, "main")]);
            var columns = structure.Instantiate(StructureTarget.Columns, TargetType.Object,
                [new StructureFilter(StructureTarget.Perspectives, "main"), new StructureFilter(StructureTarget.Tables, "sales")]);

            var types = new DataTypeDiscoveryFactoryProvider().Instantiate(url);
            var amount = types.Instantiate(DataTypeTarget.Columns,
                [new ColumnFilter(DataTypeTarget.Perspectives, "main"), new ColumnFilter(DataTypeTarget.Tables, "sales"),
                 new ColumnFilter(DataTypeTarget.Columns, "amount")]).Execute();

            Assert.Multiple(() =>
            {
                Assert.That(tables.Execute(), Is.EqualTo(new[] { "sales" }));
                Assert.That(columns.Execute(), Is.EqualTo(new[] { "amount" }));
                Assert.That(amount, Is.TypeOf<NumericInfo>());
                Assert.That(((NumericInfo)amount!).Precision, Is.EqualTo(10));
                Assert.That(((NumericInfo)amount).Scale, Is.EqualTo(2));
                Assert.That(amount.Nullable, Is.False);
            });
        }
        finally
        {
            File.Delete(path);
        }
    }
}
