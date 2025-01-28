using Moq;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Transformation;

[TestFixture]
public class TransformationProviderTest
{
    [Test]
    public void Transform_SimpleTranformation_CorrectHandlingOfColumnNames()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load("aaaa;10");
        resultSet.GetColumn(0)?.Rename("MyCol0");
        resultSet.GetColumn(1)?.Rename("MyCol1");

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.CSharp
                && t.OriginalType == ColumnType.Text
                && t.Code == "value.Substring(0,1)"
            );

        var provider = new TransformationProvider(new ServiceLocator(), Context.None);
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet.GetColumn(0)?.Name, Is.EqualTo("MyCol0"));
        Assert.That(resultSet.GetColumn(1)?.Name, Is.EqualTo("MyCol1"));
        Assert.That(resultSet.ColumnCount, Is.EqualTo(2));
    }

    [Test]
    public void Transform_SimpleTranformation_Correct()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load("aaaa;10");

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.CSharp
                && t.OriginalType == ColumnType.Text
                && t.Code == "value.Substring(0,1)"
            );

        var provider = new TransformationProvider(new ServiceLocator(), Context.None);
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet[0][0], Is.EqualTo("a"));
    }

    [Test]
    public void Transform_NativeTranformationTrim_Correct()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load(" aaaa  ;10");

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.Native
                && t.OriginalType == ColumnType.Text
                && t.Code == "text-to-trim"
            );

        var provider = new TransformationProvider(new ServiceLocator(), Context.None);
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet[0][0], Is.EqualTo("aaaa"));
    }

    [Test]
    public void Transform_NativeTranformationFirstCharWithContext_Correct()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load(new[] { ["123456789", 6], new object[] { "abcdefgh", 2 } });

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.Native
                && t.OriginalType == ColumnType.Text
                && t.Code == "text-to-first-chars(#1)"
            );

        var provider = new TransformationProvider(new ServiceLocator(), new Context());
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet[0][0], Is.EqualTo("123456"));
        Assert.That(resultSet[1][0], Is.EqualTo("ab"));
    }

    [Test]
    public void Transform_NativeTranformationBlankToNull_Correct()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load("\t;10");

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.Native
                && t.OriginalType == ColumnType.Text
                && t.Code == "blank-to-null"
            );

        var provider = new TransformationProvider(new ServiceLocator(), Context.None);
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet[0][0], Is.EqualTo("(null)"));
    }

    [Test]
    public void Transform_NativeTranformationUnknown_Exception()
    {
        var resultSet = new DataTableResultSet();
        resultSet.Load("\t;10");

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.Native
                && t.OriginalType == ColumnType.Text
                && t.Code == "unknown"
            );

        var provider = new TransformationProvider(new ServiceLocator(), new Context());

        Assert.Throws<Expressif.NotImplementedFunctionException>(() => provider.Add(new ColumnOrdinalIdentifier(0), transformation));
    }

    [Test]
    public void Transform_TypeSwitch_Correct()
    {
        var resultSet = new DataTableResultSet();
        var obj = new object[] { new DateTime(2016,10,1) };
        resultSet.Load(Enumerable.Repeat(obj,1));

        var transformation = Mock.Of<ITransformationInfo>
            (
                t => t.Language == LanguageType.CSharp
                && t.OriginalType == ColumnType.DateTime
                && t.Code == "value.Month + (value.Year-2000)*12"
            );

        var provider = new TransformationProvider(new ServiceLocator(), Context.None);
        provider.Add(new ColumnOrdinalIdentifier(0), transformation);
        provider.Execute(resultSet);

        Assert.That(resultSet[0][0], Is.EqualTo(202));
    }
}
