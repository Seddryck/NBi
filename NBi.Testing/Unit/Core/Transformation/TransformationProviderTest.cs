using Moq;
using NBi.Core.Transformation;
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
            provider.Add(0, transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo("a"));
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
            provider.Add(0, transformation);
            provider.Transform(resultSet);

            Assert.That(resultSet.Rows[0][0], Is.EqualTo(202));
        }
    }
}
