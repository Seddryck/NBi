using DubUrl;
using DubUrl.BulkCopy;
using DubUrl.Mapping;
using DubUrl.Provider.Sqlite.Mapping;
using DubUrl.Querying.Parametrizing;
using Microsoft.Data.Sqlite;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.DataEngineering.Commands;
using NBi.Core.Query.Client;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System.Data;

namespace NBi.Core.Testing.Decoration.DataEngineering;

public class DubUrlBulkLoadTest
{
    [Test]
    public void Execute_UrlWithBulkCapability_ForwardsCsvToProvider()
    {
        var bulkCopy = new RecordingBulkCopy();
        var catalog = new ProviderCatalogBuilder();
        var dialect = DubUrlClientFactory.CreateUrl("sqlite:///:memory:").Dialect;
        catalog.AddProvider(new ProviderRegistration("SQLite test", ["bulktest"], "Microsoft.Data.Sqlite",
            () => SqliteFactory.Instance, dialect, () => new NamedParametrizer(),
            context => new SqliteMapper(context.ConnectionStringBuilder, context.Dialect, context.Parametrizer, context.RootPath))
            { BulkCopyProvider = bulkCopy });
        const string connectionString = "bulktest:///:memory:";
        var client = new DubUrlClient(new ConnectionUrl(connectionString, catalog.Build()), connectionString);
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "first;second\r\n");
            var args = new TableLoadCommandArgs(Guid.NewGuid(), connectionString,
                new LiteralScalarResolver<string>("sales"), new LiteralScalarResolver<string>(path));

            var command = new DataEngineeringFactory().Instantiate(args, client);
            Assert.That(command, Is.TypeOf<DubUrlBulkLoadCommand>());
            command.Execute();

            Assert.That(bulkCopy.Table, Is.EqualTo("sales"));
            Assert.That(bulkCopy.Values, Is.EqualTo(new[] { "first", "second" }));
        }
        finally
        {
            File.Delete(path);
        }
    }

    private sealed class RecordingBulkCopy : IBulkCopyProvider, IBulkCopyEngine
    {
        public string? Table { get; private set; }
        public object[]? Values { get; private set; }

        public IBulkCopyEngine Create(ConnectionUrl url, IBulkCopyDependencyResolver dependencies) => this;

        public void Write(string tableName, IDataReader reader)
        {
            Table = tableName;
            Assert.That(reader.Read(), Is.True);
            Values = new object[reader.FieldCount];
            reader.GetValues(Values);
            Assert.That(reader.Read(), Is.False);
        }

        public void Dispose() { }
    }
}
