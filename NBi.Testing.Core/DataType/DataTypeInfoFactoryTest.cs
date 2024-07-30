using NBi.Core.DataType;
using NBi.Core.Structure;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure.Relational;
using NBi.Core.Structure.Tabular;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBi.Core.Testing.DataType
{
    public class DataTypeInfoFactoryTest
    {
        [Test]
        [TestCase("varchar")]
        [TestCase("int")]
        [TestCase("tinyint")]
        [TestCase("char")]
        [TestCase("smalldatetime")]
        [TestCase("float")]
        [TestCase("decimal")]
        public void Instantiate_SqlType_CorrectName(string value)
        {
            var factory = new DataTypeInfoFactory();
            var obj = factory.Instantiate(value);
            Assert.That(obj.Name, Is.EqualTo(value));
        }

        [Test]
        [TestCase("varchar", typeof(TextInfo))]
        [TestCase("varchar(10)", typeof(TextInfo))]
        [TestCase("int", typeof(NumericInfo))]
        [TestCase("tinyint", typeof(NumericInfo))]
        [TestCase("char", typeof(TextInfo))]
        [TestCase("smalldatetime", typeof(DateTimeInfo))]
        [TestCase("float", typeof(NumericInfo))]
        [TestCase("decimal", typeof(NumericInfo))]
        [TestCase("decimal(10,2)", typeof(NumericInfo))]
        public void Instantiate_SqlType_CorrectName(string value, Type type)
        {
            var factory = new DataTypeInfoFactory();
            var obj = factory.Instantiate(value);
            Assert.That(obj, Is.TypeOf(type));
        }

        [Test]
        [TestCase("varchar(10)")]
        [TestCase("decimal(10,3)")]
        [TestCase("varchar")]
        [TestCase("int")]
        public void Instantiate_SqlType_CorrectDisplay(string value)
        {
            var factory = new DataTypeInfoFactory();
            var obj = factory.Instantiate(value);
            Assert.That(obj.ToString(), Is.EqualTo(value));
        }

        [Test]
        [TestCase("varchar(10)")]
        [TestCase("char(10)")]
        [TestCase("nchar(10)")]
        [TestCase("nvarchar(10)")]
        public void Instantiate_TextType_CorrectLength(string value)
        {
            var factory = new DataTypeInfoFactory();
            var obj = factory.Instantiate(value);
            Assert.That(obj, Is.AssignableTo<ILength>());
            Assert.That(((ILength)obj).Length, Is.EqualTo(10));
        }

        [Test]
        [TestCase("decimal(10,3)")]
        public void Instantiate_NumericType_CorrectScalePrecision(string value)
        {
            var factory = new DataTypeInfoFactory();
            var obj = factory.Instantiate(value);
            Assert.That(obj, Is.AssignableTo<IPrecision>());
            Assert.That(((IPrecision)obj).Precision, Is.EqualTo(10));
            Assert.That(obj, Is.AssignableTo<IScale>());
            Assert.That(((IScale)obj).Scale, Is.EqualTo(3));
        }

    }
}
