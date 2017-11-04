using Moq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Equivalence
{
    [TestFixture]
    public class SettingsComparerBuilderTest
    {
        [Test]
        public void Build_NonDefaultKeyAndKeyName_Exception()
        {
            var builder = new SettingsEquivalerBuilder();
            builder.Setup(SettingsIndexResultSet.KeysChoice.All, SettingsIndexResultSet.ValuesChoice.AllExpectFirst);
            builder.Setup(new[] { "myKey" }, new string[0]);
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Test]
        public void Build_NonDefaultKeyAndNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new SettingsEquivalerBuilder();
            builder.Setup(SettingsIndexResultSet.KeysChoice.All, SettingsIndexResultSet.ValuesChoice.AllExpectFirst);
            builder.Setup(new[] { columnDef });
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Test]
        public void Build_TwiceTheSameNamedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Name = "MyKey";

            var builder = new SettingsEquivalerBuilder();
            builder.Setup(SettingsIndexResultSet.KeysChoice.All, SettingsIndexResultSet.ValuesChoice.AllExpectFirst);
            builder.Setup(Enumerable.Repeat(columnDef, 2).ToList());
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Test]
        public void Build_TwiceTheSameIndexedColumn_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Index = 1;

            var builder = new SettingsEquivalerBuilder();
            builder.Setup(SettingsIndexResultSet.KeysChoice.All, SettingsIndexResultSet.ValuesChoice.AllExpectFirst);
            builder.Setup(Enumerable.Repeat(columnDef, 2).ToList());
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Test]
        public void Build_IncoherenceDefaultToleranceAndValueType_Exception()
        {
            var columnDef = Mock.Of<IColumnDefinition>();
            columnDef.Index = 1;

            var builder = new SettingsEquivalerBuilder();
            builder.Setup(ColumnType.Numeric, new DateTimeTolerance(new TimeSpan(1000)));
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
    }
}
