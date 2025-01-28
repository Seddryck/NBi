using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NBi.Core.ResultSet;
using NBi.Core.Injection;
using NBi.Core.Variable;

namespace NBi.Core.Testing.Transformation;

[TestFixture]
public class TransformerFactoryTest
{
    [Test]
    [TestCase(LanguageType.CSharp, typeof(CSharpTransformer<decimal>))]
    [TestCase(LanguageType.NCalc, typeof(NCalcTransformer<decimal>))]
    [TestCase(LanguageType.Format, typeof(FormatTransformer<decimal>))]
    [TestCase(LanguageType.Native, typeof(NativeTransformer<decimal>))]
    public void Build_Language_Correct(LanguageType language, Type result)
    {
        var info = Mock.Of<ITransformationInfo>
        (
            i => i.Language == language
                && i.OriginalType == ColumnType.Numeric
                && i.Code == "value"   
        );
        var factory = new TransformerFactory(new ServiceLocator(), Context.None);
        var provider = factory.Instantiate(info);

        Assert.That(provider, Is.InstanceOf(result));
    }

    [Test]
    [TestCase(ColumnType.Text, typeof(CSharpTransformer<string>))]
    [TestCase(ColumnType.Numeric, typeof(CSharpTransformer<decimal>))]
    [TestCase(ColumnType.DateTime, typeof(CSharpTransformer<DateTime>))]
    [TestCase(ColumnType.Boolean, typeof(CSharpTransformer<bool>))]
    public void Build_Language_Correct(ColumnType originalType, Type result)
    {
        var info = Mock.Of<ITransformationInfo>
        (
            i => i.Language == LanguageType.CSharp
                && i.OriginalType == originalType
                && i.Code == "value"
        );
        var factory = new TransformerFactory(new ServiceLocator(), Context.None);
        var provider = factory.Instantiate(info);

        Assert.That(provider, Is.InstanceOf(result));
    }

    [Test]
    [TestCase(LanguageType.NCalc, ColumnType.DateTime)]
    [TestCase(LanguageType.NCalc, ColumnType.Boolean)]
    [TestCase(LanguageType.Format, ColumnType.Text)]
    [TestCase(LanguageType.Format, ColumnType.Boolean)]
    public void Build_Language_Correct(LanguageType language, ColumnType columnType)
    {
        var info = Mock.Of<ITransformationInfo>
        (
            i => i.Language == language
                && i.OriginalType == columnType
                && i.Code == "value"
        );
        var factory = new TransformerFactory(new ServiceLocator(), Context.None);
        Assert.Throws<InvalidOperationException>(delegate { factory.Instantiate(info); });
    }

}
