using System;
using System.IO;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.EqualTo;
using NBi.Xml.Items;

namespace NBi.Xml
{
    public class TestSuiteBuilder
    {
        protected Settings actual;
        protected Settings expect;

        protected delegate TestSuiteXml BuildMethod();
        protected BuildMethod buildMethod;
        
        protected class Settings
        {
            public string Directory { get; set; }
            public string ConnectionString { get; set; }
        }

        public void DefineActual(string directory, string connectionString)
        {
            actual = new Settings();
            actual.Directory = directory;
            actual.ConnectionString = connectionString;
        }

        public void DefineExpect(string directory, string connectionString)
        {
            expect = new Settings();
            expect.Directory = directory;
            expect.ConnectionString = connectionString;
            buildMethod = BuildQueriesBased;
        }

        public void DefineExpect(string directory)
        {
            expect = new Settings();
            expect.Directory = directory;
            buildMethod = BuildResultSetsBased;
        }

        public TestSuiteBuilder()
        {
        }

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
                    sut.Item.File = query;
                    sut.Item.ConnectionString = actual.ConnectionString;
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
                    ctr.ResultSet = new ResultSetXml()
                    {
                        File = Path.Combine(expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")
                    };

                    var sut = new Systems.ExecutionXml();
                    test.Systems.Add(sut);
                    sut.Item.File = query;
                    sut.Item.ConnectionString = actual.ConnectionString;
                }
            }
            return testSuite;
        }
    }
}
