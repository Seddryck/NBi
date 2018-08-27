using Moq;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer.Native;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Transformation
{
    [TestFixture]
    public class TransformationProviderTest
    {
        [Test]
        public void Transform_SimpleTranformation_Correct()
        {
            var resultSet = new NBi.Core.ResultSet.ResultSet();
            resultSet.Load("aaaa;10");

            var transformation = Mock.Of<ITransformationInfo>
                (
                    t => t.Language == LanguageType.CSharp
                    && t.OriginalType == NBi.Core.ResultSet.ColumnType.Text
                    && t.Code == "value.Substring(0,1)"
                );

            var provider = new TransformationProvider();
            provider.Add(new ColumnOrdinalIdentifier(0), transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo("a"));
        }

        [Test]
        public void Transform_NativeTranformationTrim_Correct()
        {
            var resultSet = new NBi.Core.ResultSet.ResultSet();
            resultSet.Load(" aaaa  ;10");

            var transformation = Mock.Of<ITransformationInfo>
                (
                    t => t.Language == LanguageType.Native
                    && t.OriginalType == NBi.Core.ResultSet.ColumnType.Text
                    && t.Code == "text-to-trim"
                );

            var provider = new TransformationProvider();
            provider.Add(new ColumnOrdinalIdentifier(0), transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo("aaaa"));
        }

        [Test]
        public void Transform_NativeTranformationBlankToNull_Correct()
        {
            var resultSet = new NBi.Core.ResultSet.ResultSet();
            resultSet.Load("\t;10");

            var transformation = Mock.Of<ITransformationInfo>
                (
                    t => t.Language == LanguageType.Native
                    && t.OriginalType == NBi.Core.ResultSet.ColumnType.Text
                    && t.Code == "blank-to-null"
                );

            var provider = new TransformationProvider();
            provider.Add(new ColumnOrdinalIdentifier(0), transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo("(null)"));
        }


        [Test]
        public void Transform_NativeTranformationUnknown_Exception()
        {
            var resultSet = new NBi.Core.ResultSet.ResultSet();
            resultSet.Load("\t;10");

            var transformation = Mock.Of<ITransformationInfo>
                (
                    t => t.Language == LanguageType.Native
                    && t.OriginalType == NBi.Core.ResultSet.ColumnType.Text
                    && t.Code == "unknown"
                );

            var provider = new TransformationProvider();

            Assert.Throws<NotImplementedTransformationException>(() => provider.Add(new ColumnOrdinalIdentifier(0), transformation));
        }

        [Test]
        public void Transform_TypeSwitch_Correct()
        {
            var resultSet = new NBi.Core.ResultSet.ResultSet();
            var obj = new object[] { new DateTime(2016,10,1) };
            resultSet.Load(Enumerable.Repeat(obj,1));

            var transformation = Mock.Of<ITransformationInfo>
                (
                    t => t.Language == LanguageType.CSharp
                    && t.OriginalType == NBi.Core.ResultSet.ColumnType.DateTime
                    && t.Code == "value.Month + (value.Year-2000)*12"
                );

            var provider = new TransformationProvider();
            provider.Add(new ColumnOrdinalIdentifier(0), transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo(202));
        }
    }
}
