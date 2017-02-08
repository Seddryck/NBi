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
    public class ResultSetComparisonFactoryTest
    {
        [Test]
        public void Instantiate_NonDefaultKeyAndKeyName_Exception()
        {
            var factory = new SettingsResultSetComparisonFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
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

            var factory = new SettingsResultSetComparisonFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
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

            var factory = new SettingsResultSetComparisonFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
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

            var factory = new SettingsResultSetComparisonFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
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
