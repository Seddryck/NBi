using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    class JsonResultSetTest
    {

        [Test]
        public void Build_OneElementOneattribute_ColumnCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }]");

            Assert.That(result.Columns, Has.Count.EqualTo(1));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
        }

        [Test]
        public void Build_OneElementOneattribute_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }]");

            Assert.That(result.Rows, Has.Count.EqualTo(1));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(1));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
        }

        [Test]
        public void Build_TwoElementsOneattribute_ColumnCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }, { \"name\":\"Paul\" }]");

            Assert.That(result.Columns, Has.Count.EqualTo(1));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
        }

        [Test]
        public void Build_TwoElementsOneattribute_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\" }, { \"name\":\"Paul\" }]");

            Assert.That(result.Rows, Has.Count.EqualTo(2));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(1));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[1].ItemArray[0], Is.EqualTo("Paul"));
        }


        [Test]
        public void Build_OneElementTwoattributes_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31 }]");

            Assert.That(result.Columns, Has.Count.EqualTo(2));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("age"));
        }

        [Test]
        public void Build_OneElementTwoattributes_RowCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31 }]");

            Assert.That(result.Rows, Has.Count.EqualTo(1));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(2));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[0].ItemArray[1], Is.EqualTo(31));
        }


        [Test]
        public void Build_OneElementWithArray_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"name\": \"Mike\", \"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}] }]");

            Assert.That(result.Columns, Has.Count.EqualTo(4));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("hometown"));
            Assert.That(result.Columns[2].ColumnName, Is.EqualTo("children.name"));
            Assert.That(result.Columns[3].ColumnName, Is.EqualTo("children.age"));
        }

        [Test]
        public void Build_OneElementWithArrayAndPostElement_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"name\": \"Mike\", \"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}], \"job\": \"lawyer\" }]");

            Assert.That(result.Columns, Has.Count.EqualTo(5));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("hometown"));
            Assert.That(result.Columns[2].ColumnName, Is.EqualTo("children.name"));
            Assert.That(result.Columns[3].ColumnName, Is.EqualTo("children.age"));
            Assert.That(result.Columns[4].ColumnName, Is.EqualTo("job"));
        }

        [Test]
        public void Build_OneElementWithArrayAndPostElementAndMissingAttribute_ColumnsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"hometown\":\"New York\", \"children\":[{\"age\": \"6\"}, {\"name\": \"Helen\", \"age\": \"6\"}], \"job\": \"lawyer\" }]");

            Assert.That(result.Columns, Has.Count.EqualTo(5));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("hometown"));
            Assert.That(result.Columns[2].ColumnName, Is.EqualTo("children.age").Or.EqualTo("children.name"));
            Assert.That(result.Columns[3].ColumnName, Is.EqualTo("children.name").Or.EqualTo("children.age"));
            Assert.That(result.Columns[4].ColumnName, Is.EqualTo("job"));
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

            Assert.That(result.Columns, Has.Count.EqualTo(5));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("name"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("hometown"));
            Assert.That(result.Columns[2].ColumnName, Is.EqualTo("children.age").Or.EqualTo("children.name"));
            Assert.That(result.Columns[3].ColumnName, Is.EqualTo("children.name").Or.EqualTo("children.age"));
            Assert.That(result.Columns[4].ColumnName, Is.EqualTo("job"));
        }

        [Test]
        public void Build_OneElementWithArray_RowsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31, \"children\":[{\"name\": \"Mike\"}, {\"name\": \"Helen\"}] }]");

            Assert.That(result.Rows, Has.Count.EqualTo(2));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(3));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[0].ItemArray[1], Is.EqualTo(31));
            Assert.That(result.Rows[0].ItemArray[2], Is.EqualTo("Mike"));
            Assert.That(result.Rows[1].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[1].ItemArray[1], Is.EqualTo(31));
            Assert.That(result.Rows[1].ItemArray[2], Is.EqualTo("Helen"));

        }

        [Test]
        public void Build_OneElementWithArrayMissingAttributes_RowsCorrectlyReturned()
        {
            var builder = new JsonResultSet();
            var result = builder.Build("[{ \"name\":\"John\", \"age\":31, \"children\":[{\"name\": \"Mike\"}, {\"name\": \"Helen\", \"age\":5}] }]");

            Assert.That(result.Rows, Has.Count.EqualTo(2));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(4));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[0].ItemArray[1], Is.EqualTo(31));
            Assert.That(result.Rows[0].ItemArray[2], Is.EqualTo("Mike"));
            Assert.That(result.Rows[0].ItemArray[3], Is.EqualTo(DBNull.Value));
            Assert.That(result.Rows[1].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[1].ItemArray[1], Is.EqualTo(31));
            Assert.That(result.Rows[1].ItemArray[2], Is.EqualTo("Helen"));
            Assert.That(result.Rows[1].ItemArray[3], Is.EqualTo(5));

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

            Assert.That(result.Rows, Has.Count.EqualTo(3));
            Assert.That(result.Rows[0].ItemArray.Length, Is.EqualTo(5));
            Assert.That(result.Rows[0].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[0].ItemArray[1], Is.EqualTo("New York"));
            Assert.That(result.Rows[0].ItemArray[2], Is.EqualTo(6));
            Assert.That(result.Rows[0].ItemArray[3], Is.EqualTo(DBNull.Value));
            Assert.That(result.Rows[0].ItemArray[4], Is.EqualTo("lawyer"));
            Assert.That(result.Rows[1].ItemArray[0], Is.EqualTo("John"));
            Assert.That(result.Rows[1].ItemArray[1], Is.EqualTo("New York"));
            Assert.That(result.Rows[1].ItemArray[2], Is.EqualTo(5));
            Assert.That(result.Rows[1].ItemArray[3], Is.EqualTo("Helen"));
            Assert.That(result.Rows[1].ItemArray[4], Is.EqualTo("lawyer"));
            Assert.That(result.Rows[2].ItemArray[0], Is.EqualTo("Andrew"));
            Assert.That(result.Rows[2].ItemArray[1], Is.EqualTo("Chicago"));
            Assert.That(result.Rows[2].ItemArray[2], Is.EqualTo(DBNull.Value));
            Assert.That(result.Rows[2].ItemArray[3], Is.EqualTo(DBNull.Value));
            Assert.That(result.Rows[2].ItemArray[4], Is.EqualTo("engineer"));
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
