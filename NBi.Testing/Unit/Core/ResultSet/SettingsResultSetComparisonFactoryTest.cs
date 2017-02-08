using Moq;
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
    public class SettingsResultSetComparisonFactoryTest
    {
        [Test]
        public void Instantiate_NonDefaultKeyAndKeyName_Exception()
        {
            var builder = new ResultSetComparisonBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsResultSetComparisonByIndex.KeysChoice.All
                , "MyKey"
                , SettingsResultSetComparisonByIndex.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , null
                ));
        }

        [Test]
        public void Instantiate_NonDefaultKeyAndNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new ResultSetComparisonBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsResultSetComparisonByIndex.KeysChoice.All
                , string.Empty
                , SettingsResultSetComparisonByIndex.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 1).ToList()
                ));
        }

        [Test]
        public void Instantiate_TwiceTheSameNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new ResultSetComparisonBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast
                , string.Empty
                , SettingsResultSetComparisonByIndex.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 2).ToList()
                ));
        }

        [Test]
        public void Instantiate_TwiceTheSameIndexedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Index = 1;

            var builder = new ResultSetComparisonBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast
                , string.Empty
                , SettingsResultSetComparisonByIndex.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 2).ToList()
                ));
        }
    }
}
