using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet
{
    [TestFixture]
    class JsonResultSetTest
    {

        [Test]
        public void Build_OneElementOneattribute_ColumnCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }]");

            Assert.That(result.ColumnCount, Is.EqualTo(1));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
        }

        [Test]
        public void Build_OneElementOneattribute_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }]");

            Assert.That(result.Rows.Count(), Is.EqualTo(1));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(1));
            Assert.That(result[0][0], Is.EqualTo("John"));
        }

        [Test]
        public void Build_TwoElementsOneattribute_ColumnCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }, { \"name\":\"Paul\" }]");

            Assert.That(result.ColumnCount, Is.EqualTo(1));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
        }

        [Test]
        public void Build_TwoElementsOneattribute_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }, { \"name\":\"Paul\" }]");

            Assert.That(result.Rows.Count(), Is.EqualTo(2));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(1));
            Assert.That(result[0][0], Is.EqualTo("John"));
            Assert.That(result[1][0], Is.EqualTo("Paul"));
        }


        [Test]
        public void Build_OneElementTwoattributes_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31 }]");

            Assert.That(result.ColumnCount, Is.EqualTo(2));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("age"));
        }

        [Test]
        public void Build_OneElementTwoattributes_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31 }]");

            Assert.That(result.Rows.Count(), Is.EqualTo(1));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(2));
            Assert.That(result[0][0], Is.EqualTo("John"));
            Assert.That(result[0][1], Is.EqualTo(31));
        }


        [Test]
        public void Build_OneElementWithArray_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"name\": \"Mike\", \"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}] }]");

            Assert.That(result.ColumnCount, Is.EqualTo(4));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("hometown"));
            Assert.That(result.GetColumn(2)?.Name, Is.EqualTo("children.name"));
            Assert.That(result.GetColumn(3)?.Name, Is.EqualTo("children.age"));
        }

        [Test]
        public void Build_OneElementWithArrayAndPostElement_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"name\": \"Mike\", \"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}], \"job\": \"lawyer\" }]");

            Assert.That(result.ColumnCount, Is.EqualTo(5));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("hometown"));
            Assert.That(result.GetColumn(2)?.Name, Is.EqualTo("children.name"));
            Assert.That(result.GetColumn(3)?.Name, Is.EqualTo("children.age"));
            Assert.That(result.GetColumn(4)?.Name, Is.EqualTo("job"));
        }

        [Test]
        public void Build_OneElementWithArrayAndPostElementAndMissingAttribute_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}], \"job\": \"lawyer\" }]");

            Assert.That(result.ColumnCount, Is.EqualTo(5));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("hometown"));
            Assert.That(result.GetColumn(2)?.Name, Is.EqualTo("children.age").Or.EqualTo("children.name"));
            Assert.That(result.GetColumn(3)?.Name, Is.EqualTo("children.name").Or.EqualTo("children.age"));
            Assert.That(result.GetColumn(4)?.Name, Is.EqualTo("job"));
        }

        [Test]
        public void Build_OneElementWithArrayAndPostElementAndMissingAttributeInNextElements_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build(
                "[" + 
                    "{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":" + 
                        "[{\"age\": \"6\"}, {\"age\": \"6\"}]" + 
                    ", \"job\": \"lawyer\" }" +
                    ", { \"name\":\"Andrew\", \"hometown\":\"Chicago\", \"children\":" +
                        "[{\"age\": \"4\"}, {\"name\": \"Helen\", \"age\": \"2\"}]" +
                    ", \"job\": \"Engineer\" }" +
                "]"
                );

            Assert.That(result.ColumnCount, Is.EqualTo(5));
            Assert.That(result.GetColumn(0)?.Name, Is.EqualTo("name"));
            Assert.That(result.GetColumn(1)?.Name, Is.EqualTo("hometown"));
            Assert.That(result.GetColumn(2)?.Name, Is.EqualTo("children.age").Or.EqualTo("children.name"));
            Assert.That(result.GetColumn(3)?.Name, Is.EqualTo("children.name").Or.EqualTo("children.age"));
            Assert.That(result.GetColumn(4)?.Name, Is.EqualTo("job"));
        }

        [Test]
        public void Build_OneElementWithArray_RowsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31, \"children\":[{\"name\": \"Mike\"}, {\"name\": \"Helen\"}] }]");

            Assert.That(result.Rows.Count(), Is.EqualTo(2));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(3));
            Assert.That(result[0][0], Is.EqualTo("John"));
            Assert.That(result[0][1], Is.EqualTo(31));
            Assert.That(result[0][2], Is.EqualTo("Mike"));
            Assert.That(result[1][0], Is.EqualTo("John"));
            Assert.That(result[1][1], Is.EqualTo(31));
            Assert.That(result[1][2], Is.EqualTo("Helen"));

        }

        [Test]
        public void Build_OneElementWithArrayMissingAttributes_RowsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31, \"children\":[{\"name\": \"Mike\"}, {\"name\": \"Helen\", \"age\":5}] }]");

            Assert.That(result.Rows.Count(), Is.EqualTo(2));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(4));
            Assert.That(result[0][0], Is.EqualTo("John"));
            Assert.That(result[0][1], Is.EqualTo(31));
            Assert.That(result[0][2], Is.EqualTo("Mike"));
            Assert.That(result[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[1][0], Is.EqualTo("John"));
            Assert.That(result[1][1], Is.EqualTo(31));
            Assert.That(result[1][2], Is.EqualTo("Helen"));
            Assert.That(result[1][3], Is.EqualTo(5));

        }

        [Test]
        public void Build_OneElementWithArrayEmptyArray_RowsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build(
                "[" +
                    "{ \"name\":\"John\", \"hometown\": \"New York\", \"children\":" +
                        "[{\"age\": 6}, {\"name\": \"Helen\", \"age\": 5}]" +
                    ", \"job\": \"lawyer\" }" +
                    ", { \"name\":\"Andrew\", \"hometown\": \"Chicago\" " +
                    ", \"job\": \"engineer\" }" +
                "]"
                );

            Assert.That(result.Rows.Count(), Is.EqualTo(3));
            Assert.That(result[0].ItemArray.Length, Is.EqualTo(5));
            Assert.That(result[0][0], Is.EqualTo("John"));
            Assert.That(result[0][1], Is.EqualTo("New York"));
            Assert.That(result[0][2], Is.EqualTo(6));
            Assert.That(result[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[0][4], Is.EqualTo("lawyer"));
            Assert.That(result[1][0], Is.EqualTo("John"));
            Assert.That(result[1][1], Is.EqualTo("New York"));
            Assert.That(result[1][2], Is.EqualTo(5));
            Assert.That(result[1][3], Is.EqualTo("Helen"));
            Assert.That(result[1][4], Is.EqualTo("lawyer"));
            Assert.That(result[2][0], Is.EqualTo("Andrew"));
            Assert.That(result[2][1], Is.EqualTo("Chicago"));
            Assert.That(result[2][2], Is.EqualTo(DBNull.Value));
            Assert.That(result[2][3], Is.EqualTo(DBNull.Value));
            Assert.That(result[2][4], Is.EqualTo("engineer"));
        }

        [Test]
        public void Build_EmptyDocument_EmptyResultSet()
        {
            var builder = new JsonResultSet();
            var json = 
                "[" +
                    "{ \"name\":\"John\", \"hometown\": \"New York\", \"children\":" +
                        "[{\"age\": 6}, {\"name\": \"Helen\", \"age\": 5}]" +
                    ", \"job\": \"lawyer\" " +
                    ", \"dishes\": [{ \"name\":\"pasta\"}, {\"name\": \"chicken\"}] " +
                    "}" +
                "]";

            Assert.Throws<InvalidOperationException>(() => builder.Build(json));
        }
    }
}
