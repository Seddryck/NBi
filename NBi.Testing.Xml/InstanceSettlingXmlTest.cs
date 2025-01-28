using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Xml.Variables;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using NBi.Xml.Items;
using System.Reflection;
using NBi.Xml.Settings;
using NBi.Core.Transformation;
using NBi.Core.ResultSet;

namespace NBi.Xml.Testing.Unit;

public class InstanceSettlingXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFilWithInstanceSettlinge_InstanceDefinitionNotNull()
    {
        var ts = DeserializeSample();
        var test = ts.Tests[0] as TestXml;

        // Check the properties of the object.
        Assert.That(test.InstanceSettling, Is.Not.Null);
        Assert.That(test.InstanceSettling, Is.Not.EqualTo(InstanceSettlingXml.Unique));
    }


    [Test]
    public void Serialize_TestWithInstanceSettling_InstanceSettlingCorrectlySerialized()
    {
        var test = new TestXml()
        {
            InstanceSettling = new InstanceSettlingXml()
            {
                Variable = new InstanceVariableXml() { Name = "firstOfMonth" }
            }
        };

        var serializer = new XmlSerializer(typeof(TestXml));
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, test);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<instance-settling"));
            Assert.That(content, Does.Contain("<local-variable"));
            Assert.That(content, Does.Not.Contain("<category"));
            Assert.That(content, Does.Not.Contain("<trait"));
        }
    }

    [Test]
    public void Deserialize_SampleFileWithoutInstanceSettling_InstanceDefinitionNotNull()
    {
        var ts = DeserializeSample();
        var test = ts.Tests[1] as TestXml;

        // Check the properties of the object.
        Assert.That(test.InstanceSettling, Is.Not.Null);
        Assert.That(test.InstanceSettling, Is.EqualTo(InstanceSettlingXml.Unique));
    }

    [Test]
    public void Deserialize_SampleFile_InstanceDefinitionHasVariable()
    {
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[0].InstanceSettling.Variable, Is.Not.Null);
    }

    [Test]
    public void Serialize_TestWithoutInstanceSettling_InstanceSettlingNotSerialized()
    {
        var test = new TestXml();

        var serializer = new XmlSerializer(typeof(TestXml));
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, test);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Not.Contain("<instance-settling"));
        }
    }

    [Test]
    public void Serialize_WithCategorieAndTrait_CategorieAndTraitNotSerialized()
    {
        var test = new TestXml()
        {
            InstanceSettling = new InstanceSettlingXml()
            {
                Variable = new InstanceVariableXml() { Name = "firstOfMonth" },
                Categories = ["~{@firstOfMonth:MMM}", "~{@firstOfMonth:MM}"],
                Traits = [new TraitXml() { Name = "Year", Value = "~{@firstOfMonth:YYYY}" }]
            }
        };

        var serializer = new XmlSerializer(test.GetType());
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, test);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<instance-settling"));
            Assert.That(content, Does.Contain("<category"));
            Assert.That(content, Does.Contain("<trait"));
        }
    }

    [Test]
    public void Deserialize_SampleFileWithDerivations_DerivationsNotNull()
    {
        var ts = DeserializeSample();
        var test = ts.Tests[2] as TestXml;

        // Check the properties of the object.
        Assert.That(test.InstanceSettling.DerivedVariables, Is.Not.Null);
        Assert.That(test.InstanceSettling.DerivedVariables.Count, Is.EqualTo(2));
        Assert.That(test.InstanceSettling.DerivedVariables[0].Name, Is.EqualTo("date"));
        Assert.That(test.InstanceSettling.DerivedVariables[0].BasedOn, Is.EqualTo("file"));
        Assert.That(test.InstanceSettling.DerivedVariables[0].ColumnType, Is.EqualTo(ColumnType.DateTime));
        Assert.That(test.InstanceSettling.DerivedVariables[0].Script, Is.Not.Null);
        Assert.That(test.InstanceSettling.DerivedVariables[0].Script.Language, Is.EqualTo(LanguageType.Native));
        Assert.That(test.InstanceSettling.DerivedVariables[0].Script.Code, Is.Not.Null.Or.Empty);

        Assert.That(test.InstanceSettling.DerivedVariables[1].Name, Is.EqualTo("age"));
        Assert.That(test.InstanceSettling.DerivedVariables[1].BasedOn, Is.EqualTo("date"));
        Assert.That(test.InstanceSettling.DerivedVariables[1].ColumnType, Is.EqualTo(ColumnType.Text));
        Assert.That(test.InstanceSettling.DerivedVariables[1].Script, Is.Not.Null);
        Assert.That(test.InstanceSettling.DerivedVariables[1].Script.Language, Is.EqualTo(LanguageType.Native));
        Assert.That(test.InstanceSettling.DerivedVariables[1].Script.Code, Is.Not.Null.Or.Empty);
    }


    [Test]
    public void Serialize_SampleFileWithDerivations_DerivationsCorrectlySerialized()
    {
        var test = new TestXml()
        {
            InstanceSettling = new InstanceSettlingXml()
            {
                Variable = new InstanceVariableXml() { Name = "firstOfMonth" },
                DerivedVariables =
                [
                    new DerivedVariableXml()
                    {
                        Name ="secondOfMonth", BasedOn = "firstOfMonth", ColumnType=ColumnType.DateTime,
                        Script = new ScriptXml() { Language=LanguageType.Native, Code="date-to-next-day"}
                    },
                    new DerivedVariableXml()
                    {
                        Name ="age", BasedOn = "secondOfMonth", ColumnType=ColumnType.Numeric,
                        Script = new ScriptXml() { Language=LanguageType.Native, Code="date-to-age"}
                    },
                ]
            }
        };

        var serializer = new XmlSerializer(typeof(TestXml));
        using (var stream = new MemoryStream())
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer.Serialize(writer, test);
                var content = Encoding.UTF8.GetString(stream.ToArray());

                Debug.WriteLine(content);

                Assert.That(content, Does.Contain("<instance-settling"));
                Assert.That(content, Does.Contain("<local-variable"));
                Assert.That(content, Does.Contain("<derived-variable"));
                Assert.That(content, Does.Contain("name=\"secondOfMonth\""));
                Assert.That(content, Does.Contain("based-on=\"firstOfMonth\""));
                Assert.That(content, Does.Contain("<script"));
                Assert.That(content, Does.Contain("date-to-next-day"));
                Assert.That(content, Does.Contain("date-to-age"));
            }
        }
    }
}
