using System;
using System.IO;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.EqualTo;
using NBi.Xml.Systems;

namespace NBi.Xml
{
    public class TestSuiteBuilder
    {
        protected Settings Actual;
        protected Settings Expect;

        protected delegate TestSuiteXml BuildMethod();
        protected BuildMethod buildMethod;
        
        protected class Settings
        {
            public string Directory { get; set; }
            public string ConnectionString { get; set; }
        }

        public void DefineActual(string directory, string connectionString)
        {
            Actual = new Settings();
            Actual.Directory = directory;
            Actual.ConnectionString = connectionString;
        }

        public void DefineExpect(string directory, string connectionString)
        {
            Expect = new Settings();
            Expect.Directory = directory;
            Expect.ConnectionString = connectionString;
            buildMethod = BuildQueriesBased;
        }

        public void DefineExpect(string directory)
        {
            Expect = new Settings();
            Expect.Directory = directory;
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

            var queries = Directory.GetFiles(Actual.Directory);
            foreach (var query in queries)
            {
                if (File.Exists(Path.Combine(Expect.Directory, Path.GetFileName(query))))
                {
                    var test = new TestXml();

                    testSuite.Tests.Add(test);
                    test.Name = Path.GetFileNameWithoutExtension(query);
                    test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                    var ctr = new EqualToXml();
                    test.Constraints.Add(ctr);

                    ctr.Query = new QueryXml()
                    {
                        Filename = Path.Combine(Expect.Directory, Path.GetFileName(query)),
                        ConnectionString = Expect.ConnectionString
                    };

                    var sut = new Systems.QueryXml();
                    test.Systems.Add(sut);
                    sut.Filename = query;
                    sut.ConnectionString = Actual.ConnectionString;
                }
            }
            return testSuite;
        }

        protected internal TestSuiteXml BuildResultSetsBased()
        {
            var testSuite = new TestSuiteXml();

            var queries = Directory.GetFiles(Actual.Directory);
            foreach (var query in queries)
            {
                if (File.Exists(Path.Combine(Expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")))
                {
                    var test = new TestXml();

                    testSuite.Tests.Add(test);
                    test.Name = Path.GetFileNameWithoutExtension(query);
                    test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                    var ctr = new EqualToXml();
                    test.Constraints.Add(ctr);
                    ctr.ResultSet = new ResultSetXml()
                    {
                        File = Path.Combine(Expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv")
                    };

                    var sut = new Systems.QueryXml();
                    test.Systems.Add(sut);
                    sut.Filename = query;
                    sut.ConnectionString = Actual.ConnectionString;
                }
            }
            return testSuite;
        }
    }
}
