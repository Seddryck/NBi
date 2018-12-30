using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Service.Dto;
using NBi.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace NBi.Service
{
    public class TestListManager
    {
        private IList<TestXml> lastGeneration;

        internal IList<TestXml> Tests { get; set; }

        public TestListManager()
        {
            Tests = new List<TestXml>();
            lastGeneration = new List<TestXml>();
        }


        public void Build(string template, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            var generator = new StringTemplateEngine(template, variables);
            var cases = GetCases(dataTable, useGrouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            lastGeneration = generator.Build(cases, globalVariables).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            Tests = Tests.Concat(lastGeneration).ToList();
        }

        public void Build(IEnumerable<string> templates, string[] variables, DataTable dataTable, bool useGrouping, IDictionary<string, object> globalVariables)
        {
            if (templates.Count() == 0)
                throw new ArgumentException("No template was specified. You must at least define a template before generating a test suite.");

            if (templates.Count() == 1)
                Build(templates.ElementAt(0), variables, dataTable, useGrouping, globalVariables);
            else
            {
                
                var cases = GetCases(dataTable, useGrouping);
                foreach (var indiv in cases)
                {
                    foreach (var template in templates)
                    {
                        var generator = new StringTemplateEngine(template, variables);
                        generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
                        lastGeneration = generator.Build(new List<List<List<object>>>() { indiv }, globalVariables).ToList();
                        generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
                        Tests = Tests.Concat(lastGeneration).ToList();
                    }
                }
            }
        }

        public  void OnTestGenerated(object sender, ProgressEventArgs e)
        {
            InvokeProgress(e);
        }

        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            Progressed?.Invoke(this, e);
        }

        public bool CanUndo
        {
            get { return lastGeneration.Count != 0; }
        }

        public void Undo()
        {
            foreach (var test in lastGeneration)
            {
                Tests.Remove(test);
            }
            lastGeneration.Clear();
        }

        public IList<Test> GetTests()
        {
            var value = new List<Test>();
            foreach (var test in Tests)
            {
                var t = new Test
                {
                    Content = test.Content,
                    Title = test.Name,
                    Reference = test
                };
                value.Add(t);
            }
            return value;
        }


        protected List<List<List<object>>> GetCases(DataTable dataTable, bool useGrouping)
        {
            if (dataTable.Rows.Count==0)
                return new List<List<List<object>>>();

            int groupedColumn = dataTable.Rows[0].ItemArray.Length - 1;

            var variableTests = new List<List<List<object>>>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var isIdentical = (i != 0) && useGrouping;
                var grouping = dataTable.Rows[i].ItemArray.ToList();
                grouping.RemoveAt(groupedColumn);
                var k = 0;
                while (k < grouping.Count && isIdentical)
                {
                    if (grouping[k].ToString() != variableTests[variableTests.Count - 1][k][0].ToString())
                        isIdentical = false;
                    k++;
                }


                if (!isIdentical)
                {
                    variableTests.Add(new List<List<object>>());
                    for (int j = 0; j < dataTable.Rows[i].ItemArray.Length; j++)
                    {
                        variableTests[variableTests.Count - 1].Add(new List<object>());
                        if (dataTable.Rows[i].ItemArray[j] is IEnumerable<string>)
                        {
                            foreach (var item in (IEnumerable<string>)dataTable.Rows[i].ItemArray[j])
                                variableTests[variableTests.Count - 1][j].Add(item);
                        }
                        else
                            variableTests[variableTests.Count - 1][j].Add(dataTable.Rows[i].ItemArray[j].ToString());
                    }
                }
                else
                    variableTests[variableTests.Count - 1][groupedColumn].Add(dataTable.Rows[i].ItemArray[groupedColumn].ToString());
            }

            return variableTests;
        }

        public void Clear()
        {
            Tests.Clear();
            lastGeneration.Clear();
        }

        public void RemoveAt(int index)
        {
            Tests.RemoveAt(index);
        }

        public void Remove(Test test)
        {
            Tests.Remove(test.Reference);
        }

        public void SetTests(IEnumerable<Test> tests)
        {
            Tests.Clear();
            foreach (var testDto in tests)
                Tests.Add(testDto.Reference);
        }
        
        public void AddCategory(Test test, string categoryName)
        {
            var categories = test.Reference.Categories;

            if (!categories.Contains(categoryName))
            {
                categories.Add(categoryName);
                //test.Reference.Content = StringTemplateEngine.XmlSerializeFrom<TestStandaloneXml>((TestStandaloneXml)test.Reference);
            }

        }

        public IEnumerable<char> GetCategoryForbiddenChars()
        {
            return new List<char>()
            {
                '+', '-'
            };
        }

        public IEnumerable<string> GetExistingCategories()
        {
            var categories = new List<string>();
            foreach (var test in Tests)
            {
                foreach (var category in test.Categories)
                {
                    if (!categories.Contains(category))
                        categories.Add(category);
                }
            }
            return categories;
        }

        public void AddRange(string Filename)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                AddRange(stream);
            }
        }

        protected internal void AddRange(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();

                TestSuiteXml testSuite = null;
                testSuite = XmlDeserializeFromString<TestSuiteXml>(str);

                foreach (var test in testSuite.GetAllTests())
                {
                    Tests.Add(test);
                }
            }
        }

        public void Include(string Filename)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                Include(stream);
            }
        }

        protected internal void Include(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();

                TestStandaloneXml test = null;
                test = XmlDeserializeFromString<TestStandaloneXml>(str);

                test.Content = XmlSerializeFrom<TestStandaloneXml>(test);
                Tests.Add(test);
            }            
        }


        protected internal T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        protected internal static string XmlSerializeFrom<T>(T objectData)
        {
            return SerializeFrom(objectData, typeof(T));
        }

        protected object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        protected static string SerializeFrom(object objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                // Use the Serialize method to store the object's state.
                try
                {
                    serializer.Serialize(writer, objectData);
                }
                catch (Exception e)
                {

                    throw e;
                }

                result = writer.ToString();
            }
            return result;
        }
    }
}
