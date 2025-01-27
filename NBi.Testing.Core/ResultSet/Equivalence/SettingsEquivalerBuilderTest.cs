using Moq;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Equivalence;

[TestFixture]
public class SettingsComparerBuilderTest
{
    [Test]
    public void Build_NonDefaultKeyAndKeyName_Exception()
    {
        var builder = new SettingsEquivalerBuilder();
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.All, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        builder.Setup(["myKey"], []);
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_NonDefaultKeyAndNamedColumn_Exception()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnNameIdentifier("MyKey");

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.All, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        builder.Setup([columnDef]);
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_TwiceTheSameNamedColumn_Exception()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnNameIdentifier("MyKey");

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.All, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        builder.Setup(Enumerable.Repeat(columnDef, 2).ToList());
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_TwiceTheSameOrdinalColumn_Exception()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnOrdinalIdentifier(1);

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.All, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        builder.Setup(Enumerable.Repeat(columnDef, 2).ToList());
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_IncoherenceDefaultToleranceAndValueType_Exception()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnOrdinalIdentifier(1);

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(ColumnType.Numeric, new DateTimeTolerance(new TimeSpan(1000)));
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }


    [Test]
    public void Build_OverrideUniqueKey_Exception()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnOrdinalIdentifier(0);
        columnDef.Role = ColumnRole.Value;

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(true);
        builder.Setup([columnDef]);
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.First, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    public void Build_OverrideUniqueKeyButCreateNew_NoException()
    {
        var columnDef = Mock.Of<IColumnDefinition>();
        columnDef.Identifier = new ColumnOrdinalIdentifier(0);
        columnDef.Role = ColumnRole.Value;

        var columnDefKey = Mock.Of<IColumnDefinition>();
        columnDefKey.Identifier = new ColumnOrdinalIdentifier(1);
        columnDefKey.Role = ColumnRole.Key;

        var builder = new SettingsEquivalerBuilder();
        builder.Setup(true);
        builder.Setup([columnDef, columnDefKey]);
        builder.Setup(SettingsOrdinalResultSet.KeysChoice.First, SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst);
        Assert.DoesNotThrow(() => builder.Build());
    }
}
