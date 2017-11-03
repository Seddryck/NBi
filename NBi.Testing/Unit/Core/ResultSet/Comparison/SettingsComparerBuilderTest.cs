using Moq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparison;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Comparison
{
    [TestFixture]
    public class SettingsComparerBuilderTest
    {
        [Test]
        public void Instantiate_NonDefaultKeyAndKeyName_Exception()
        {
            var builder = new SettingsComparerBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsIndexResultSet.KeysChoice.All
                , "MyKey"
                , SettingsIndexResultSet.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , null
                , ComparerKind.EqualTo
                ));
        }

        [Test]
        public void Instantiate_NonDefaultKeyAndNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new SettingsComparerBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsIndexResultSet.KeysChoice.All
                , string.Empty
                , SettingsIndexResultSet.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 1).ToList()
                , ComparerKind.EqualTo
                ));
        }

        [Test]
        public void Instantiate_TwiceTheSameNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new SettingsComparerBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsIndexResultSet.KeysChoice.AllExpectLast
                , string.Empty
                , SettingsIndexResultSet.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 2).ToList()
                , ComparerKind.EqualTo
                ));
        }

        [Test]
        public void Instantiate_TwiceTheSameIndexedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Index = 1;

            var builder = new SettingsComparerBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.Setup(
                true
                , SettingsIndexResultSet.KeysChoice.AllExpectLast
                , string.Empty
                , SettingsIndexResultSet.ValuesChoice.AllExpectFirst
                , string.Empty
                , ColumnType.Numeric
                , null
                , Enumerable.Repeat(columnDef, 2).ToList()
                , ComparerKind.EqualTo
                ));
        }
    }
}
