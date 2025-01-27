using System;
using System.IO;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;

namespace NBi.Xml;

public class TestSuiteBuilder
{
    protected Settings? actual;
    protected Settings? expect;

    protected delegate TestSuiteXml BuildMethod();
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

    public TestSuiteXml Build()
    {
        return buildMethod.Invoke();
    }

    protected internal TestSuiteXml BuildQueriesBased()
    {
        var testSuite = new TestSuiteXml();

        var queries = Directory.GetFiles(actual.Directory);
        foreach (var query in queries)
        {
            if (File.Exists(Path.Combine(expect.Directory, Path.GetFileName(query))))
            {
                var test = new TestXml();

                testSuite.Tests.Add(test);
                test.Name = Path.GetFileNameWithoutExtension(query);
                test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                var ctr = new EqualToXml();
                test.Constraints.Add(ctr);

                ctr.Query = new QueryXml()
                {
                    File = Path.Combine(expect.Directory, Path.GetFileName(query)),
                    ConnectionString = expect.ConnectionString
                };

                var sut = new Systems.ExecutionXml();
                test.Systems.Add(sut);
                ((QueryXml)sut.Item).File = query;
                ((QueryXml)sut.Item).ConnectionString = actual.ConnectionString;
            }
        }
        return testSuite;
    }

    protected internal TestSuiteXml BuildResultSetsBased()
    {
        var testSuite = new TestSuiteXml();

        var queries = Directory.GetFiles(actual.Directory);
        foreach (var query in queries)
        {
            if (File.Exists(Path.Combine(expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")))
            {
                var test = new TestXml();

                testSuite.Tests.Add(test);
                test.Name = Path.GetFileNameWithoutExtension(query);
                test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                var ctr = new EqualToXml();
                test.Constraints.Add(ctr);
                ctr.ResultSetOld = new ResultSetXml()
                {
                    File = Path.Combine(expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")
                };

                var sut = new Systems.ExecutionXml();
                test.Systems.Add(sut);
                ((QueryXml)sut.Item).File = query;
                ((QueryXml)sut.Item).ConnectionString = actual.ConnectionString;
            }
        }
        return testSuite;
    }
}
