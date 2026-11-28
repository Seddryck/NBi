using System;
using System.IO;
using NBi.Model.Constraints;
using NBi.Model.Items;
using NBi.Model.Items.ResultSet;

namespace NBi.Model;

public class TestSuiteBuilder
{
    protected Settings? actual;
    protected Settings? expect;

    protected delegate TestSuite BuildMethod();
    protected BuildMethod? buildMethod;

    protected class Settings
    {
        public string Directory { get; set; } =string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }

    public void DefineActual(string directory, string connectionString)
    {
        actual = new Settings
        {
            Directory = directory,
            ConnectionString = connectionString
        };
    }

    public void DefineExpect(string directory, string connectionString)
    {
        expect = new Settings
        {
            Directory = directory,
            ConnectionString = connectionString
        };
        buildMethod = BuildQueriesBased;
    }

    public void DefineExpect(string directory)
    {
        expect = new Settings
        {
            Directory = directory
        };
        buildMethod = BuildResultSetsBased;
    }

    public TestSuiteBuilder()
    { }

    public TestSuite Build()
    {
        return buildMethod.Invoke();
    }

    protected internal TestSuite BuildQueriesBased()
    {
        var testSuite = new TestSuite();

        var queries = Directory.GetFiles(actual.Directory);
        foreach (var query in queries)
        {
            if (File.Exists(Path.Combine(expect.Directory, Path.GetFileName(query))))
            {
                var test = new TestDefinition();

                testSuite.Tests.Add(test);
                test.Name = Path.GetFileNameWithoutExtension(query);
                test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                var ctr = new EqualTo();
                test.Constraints.Add(ctr);

                ctr.Query = new Query()
                {
                    File = Path.Combine(expect.Directory, Path.GetFileName(query)),
                    ConnectionString = expect.ConnectionString
                };

                var sut = new Systems.Execution();
                test.Systems.Add(sut);
                ((Query)sut.Item).File = query;
                ((Query)sut.Item).ConnectionString = actual.ConnectionString;
            }
        }
        return testSuite;
    }

    protected internal TestSuite BuildResultSetsBased()
    {
        var testSuite = new TestSuite();

        var queries = Directory.GetFiles(actual.Directory);
        foreach (var query in queries)
        {
            if (File.Exists(Path.Combine(expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")))
            {
                var test = new TestDefinition();

                testSuite.Tests.Add(test);
                test.Name = Path.GetFileNameWithoutExtension(query);
                test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                var ctr = new EqualTo();
                test.Constraints.Add(ctr);
                ctr.ResultSetOld = new ResultSet()
                {
                    File = Path.Combine(expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")
                };

                var sut = new Systems.Execution();
                test.Systems.Add(sut);
                ((Query)sut.Item).File = query;
                ((Query)sut.Item).ConnectionString = actual.ConnectionString;
            }
        }
        return testSuite;
    }
}
