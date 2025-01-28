#region Using directives
using System.IO;
using System.Reflection;
using NBi.Extensibility;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.Transformation;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Alteration;
using System.Collections.Generic;
using System;
#endregion

namespace NBi.Xml.Testing.Unit.Items.ResultSet;

[TestFixture]
public class TransformXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_CSharp_CSharpAndCode()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
        var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;



        Assert.That(transfo!.Language, Is.EqualTo(LanguageType.CSharp));
        Assert.That(transfo.Code.Trim, Is.EqualTo("value.Trim().ToUpper();"));
    }

    [Test]
    public void Deserialize_Native_NativeAndNativeTransfo()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
        var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;

        Assert.That(transfo!.Language, Is.EqualTo(LanguageType.Native));
        Assert.That(transfo.Code, Is.EqualTo("empty-to-null"));
    }

    [Test]
    public void Deserialize_OldValueTransformation_CorrectlyRead()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
        var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;

        Assert.That(transfo!.Language, Is.EqualTo(LanguageType.Native));
        Assert.That(transfo.Code, Is.EqualTo("empty-to-null"));
    }

    [Test]
    public void Serialize_CSharp_CodeTransfo()
    {
        var def = new ColumnDefinitionXml()
        {
            TransformationInner = new LightTransformXml()
            {
                Language = LanguageType.CSharp,
                Code = "value.Trim().ToUpper();"
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

        Assert.That(xml, Does.Contain(">value.Trim().ToUpper();<"));
        Assert.That(xml, Does.Not.Contain("column-index"));
    }

    [Test]
    public void Serialize_Format_CodeTransfo()
    {
        var def = new ColumnDefinitionXml()
        {
            TransformationInner = new LightTransformXml()
            {
                Language = LanguageType.Format,
                Code = "##.000"
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

        Assert.That(xml, Does.Contain("format"));
        Assert.That(xml, Does.Contain(">##.000<"));
        Assert.That(xml, Does.Not.Contain("column-index"));
    }

    [Test]
    public void Serialize_Native_CodeTransfo()
    {
        var def = new ColumnDefinitionXml()
        {
            TransformationInner = new LightTransformXml()
            {
                Language = LanguageType.Native,
                Code = "empty-to-null"
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

        Assert.That(xml, Does.Contain("native"));
        Assert.That(xml, Does.Contain(">empty-to-null<"));
        Assert.That(xml, Does.Not.Contain("column-index"));
    }

    [Test]
    public void Serialize_AlterTransform_OrdinalCorrect()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new TransformXml()
                {
                    Identifier = new ColumnOrdinalIdentifier(2),
                    Language = LanguageType.Native,
                    Code = "empty-to-null"
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<transform "));
        Assert.That(xml, Does.Contain("column=\"#2\""));
    }

    [Test]
    public void Serialize_AlterTransform_IdentifierCorrect()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new TransformXml()
                {
                    Identifier = new ColumnIdentifierFactory().Instantiate("[MyName]"),
                    Language = LanguageType.Native,
                    Code = "empty-to-null"
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<transform "));
        Assert.That(xml, Does.Contain("column=\"[MyName]\""));
    }
}
