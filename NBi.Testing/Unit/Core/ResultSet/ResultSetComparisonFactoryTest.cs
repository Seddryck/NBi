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
            var factory = new ResultSetComparisonSettingsFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
                true
                , ResultSetComparisonByIndexSettings.KeysChoice.All
                , "MyKey"
                , ResultSetComparisonByIndexSettings.ValuesChoice.AllExpectFirst
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

            var factory = new ResultSetComparisonSettingsFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
                true
                , ResultSetComparisonByIndexSettings.KeysChoice.All
                , string.Empty
                , ResultSetComparisonByIndexSettings.ValuesChoice.AllExpectFirst
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

            var factory = new ResultSetComparisonSettingsFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
                true
                , ResultSetComparisonByIndexSettings.KeysChoice.AllExpectLast
                , string.Empty
                , ResultSetComparisonByIndexSettings.ValuesChoice.AllExpectFirst
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

            var factory = new ResultSetComparisonSettingsFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Build(
                true
                , ResultSetComparisonByIndexSettings.KeysChoice.AllExpectLast
                , string.Empty
                , ResultSetComparisonByIndexSettings.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 2).ToList()
                ));
        }
    }
}
