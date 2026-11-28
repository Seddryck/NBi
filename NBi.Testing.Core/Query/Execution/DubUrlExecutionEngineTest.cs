using NBi.Core.Query.Execution;
using NUnit.Framework;

namespace NBi.Core.Testing.Query.Execution;

public class DubUrlExecutionEngineTest
{
    [Test]
    public void Instantiate_SqliteUrl_ExecutesParameterizedQueryThroughAllResultPaths()
    {
        var query = new NBi.Core.Query.Query("select @value as answer", "sqlite:///:memory:",
            TimeSpan.FromSeconds(5), [new NBi.Core.Query.QueryParameter("@value", 42)]);
        var engine = new ExecutionEngineFactory().Instantiate(query);

        var dataset = engine.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(dataset.Tables[0].Columns[0].ColumnName, Is.EqualTo("answer"));
            Assert.That(dataset.Tables[0].Rows[0][0], Is.EqualTo(42));
            Assert.That(engine.ExecuteScalar(), Is.EqualTo(42));
            Assert.That(engine.ExecuteList<int>(), Is.EqualTo(new[] { 42 }));
        });
    }
}
