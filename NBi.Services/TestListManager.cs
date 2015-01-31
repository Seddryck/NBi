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

        private IList<TestXml> tests;
        private IList<TestXml> lastGeneration;

        internal IList<TestXml> Tests 
        { 
            get 
            {
                return tests;
            }
            set 
            {
                tests=value;
            }
        }
        
        
        public TestListManager()
        {
            tests = new List<TestXml>();
            lastGeneration = new List<TestXml>();
        }


        public void Build(string template, string[] variables, DataTable dataTable, bool useGrouping)
        {
            var generator = new StringTemplateEngine(template, variables);
            var cases = GetCases(dataTable, useGrouping);
            generator.Progressed += new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            lastGeneration = generator.Build(cases).ToList();
            generator.Progressed -= new EventHandler<ProgressEventArgs>(this.OnTestGenerated);
            tests = tests.Concat(lastGeneration).ToList();
        }

        public  void OnTestGenerated(object sender, ProgressEventArgs e)
        {
            InvokeProgress(e);
        }

        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            var handler = Progressed;
            if (handler != null)
                handler(this, e);
        }

        public bool CanUndo
        {
            get { return lastGeneration.Count != 0; }
        }

        public void Undo()
        {
            foreach (var test in lastGeneration)
            {
                tests.Remove(test);
            }
            lastGeneration.Clear();
        }

        public IList<Test> GetTests()
        {
            var value = new List<Test>();
            foreach (var test in tests)
            {
                var t = new Test();
                t.Content = test.Content;
                t.Title = test.Name;
                t.Reference = test;
                value.Add(t);
            }
            return value;
        }


        protected List<List<List<object>>> GetCases(DataTable dataTable, bool useGrouping)
        {
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
            tests.Clear();
            lastGeneration.Clear();
        }

        public void RemoveAt(int index)
        {
            tests.RemoveAt(index);
        }

        public void Remove(Test test)
        {
            tests.Remove(test.Reference);
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
                var serializer = new Serializer();
                test.Reference.Content = serializer.XmlSerializeFrom<TestStandaloneXml>((TestStandaloneXml)test.Reference);
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
            foreach (var test in tests)
            {
                foreach (var category in test.Categories)
                {
                    if (!categories.Contains(category))
                        categories.Add(category);
                }
            }
            return categories;
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
                tests.Add(test);
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
