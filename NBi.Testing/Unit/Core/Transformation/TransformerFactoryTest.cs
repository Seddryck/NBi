using NBi.Core.Transformation;
using NBi.Core.Transformation.CSharp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NBi.Core.ResultSet;

namespace NBi.Testing.Unit.Core.Transformation.CSharp
{
    [TestFixture]
    public class TransformerFactoryTest
    {
        [Test]
        [TestCase(LanguageType.CSharp, typeof(CSharpTransformer<string>))]
        public void Build_Language_Correct(LanguageType language, Type result)
        {
            var info = Mock.Of<ITransformationInfo>
            (
                i => i.Language == language
                    && i.OriginalType == ColumnType.Text
                    && i.Code == "value"   
            );
            var factory = new TransformerFactory();
            var provider = factory.Build(info);

            Assert.IsInstanceOf(result, provider);
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
            var factory = new TransformerFactory();
            var provider = factory.Build(info);

            Assert.IsInstanceOf(result, provider);
        }

    }
}
