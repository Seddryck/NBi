using System;
using System.IO;

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
                    ctr.QueryFile = Path.Combine(Expect.Directory, Path.GetFileName(query));
                    ctr.ConnectionString = Expect.ConnectionString;

                    var tc = new QueryXml();
                    test.TestCases.Add(tc);
                    tc.Filename = query;
                    tc.ConnectionString = Actual.ConnectionString;
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
                    ctr.ResultSetFile = Path.Combine(Expect.Directory, Path.GetFileNameWithoutExtension(query) + ".csv");

                    var tc = new QueryXml();
                    test.TestCases.Add(tc);
                    tc.Filename = query;
                    tc.ConnectionString = Actual.ConnectionString;
                }
            }
            return testSuite;
        }
    }
}
