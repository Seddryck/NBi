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
using NBi.Core.ResultSet;
using NBi.Xml.Variables.Custom;

namespace NBi.Xml.Testing.Unit.Variables;

public class LocalVariableXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_InstanceDefinitionHasVariable()
    {
        var ts = DeserializeSample();
        var instance = ts.Tests[0].InstanceSettling as InstanceSettlingXml;

        // Check the properties of the object.
        Assert.That(instance.Variable, Is.Not.Null);
    }

    [Test]
    public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
    {
        var ts = DeserializeSample();
        var instance = ts.Tests[0].InstanceSettling as InstanceSettlingXml;

        // Check the properties of the object.
        Assert.That(instance.Variable.Name, Is.EqualTo("firstDayOfMonth"));
        Assert.That(instance.Variable.Type, Is.EqualTo(ColumnType.DateTime));
    }

    [Test]
    public void Serialize_InstanceSetting_LocalVariableCorrectlySerialized()
    {
        var instanceSetting = new InstanceSettlingXml()
        {
            Variable = new InstanceVariableXml()
            {
                Name = "firstOfMonth",
                Type = ColumnType.DateTime,
            }
        };

        var serializer = new XmlSerializer(typeof(InstanceSettlingXml));
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, instanceSetting);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<local-variable"));
            Assert.That(content, Does.Contain("name=\"firstOfMonth\""));
            Assert.That(content, Does.Contain("type=\"dateTime\""));
            Assert.That(content, Does.Not.Contain("<item"));
        }
    }

    [Test]
    public void Deserialize_Items_ListOfItems()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[1].InstanceSettling.Variable as InstanceVariableXml;

        // Check the properties of the object.
        Assert.That(variable.Items, Is.Not.Null);
        Assert.That(variable.Items, Has.Count.EqualTo(4));
        Assert.That(variable.Items, Has.Member("Spring"));
        Assert.That(variable.Items, Has.Member("Summer"));
        Assert.That(variable.Items, Has.Member("Fall"));
        Assert.That(variable.Items, Has.Member("Winter"));
    }

    [Test]
    public void Serialize_Items_ItemCorrectlySerialized()
    {
        var root = new InstanceSettlingXml()
        {
            Variable = new InstanceVariableXml()
            {
                Name = "season",
                Type = ColumnType.Text,
                Items = ["Spring", "Summer", "Fall", "Winter"]
            }
        };

        var serializer = new XmlSerializer(root.GetType());
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, root);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<local-variable"));
            Assert.That(content, Does.Contain("name=\"season\""));
            Assert.That(content, Does.Contain("type=\"text\""));
            Assert.That(content, Does.Not.Contain("loop"));
            Assert.That(content, Does.Contain("<item"));
            Assert.That(content, Does.Contain(">Spring<"));
            Assert.That(content, Does.Contain(">Summer<"));
            Assert.That(content, Does.Contain(">Fall<"));
            Assert.That(content, Does.Contain(">Winter<"));
        }
    }

    [Test]
    public void Serialize_Custom_CustomCorrectlySerialized()
    {
        var root = new InstanceSettlingXml()
        {
            Variable = new InstanceVariableXml()
            {
                Name = "season",
                Type = ColumnType.Text,
                Custom = new CustomXml
                {
                    AssemblyPath = "AssemblyPath\\myAssembly.dll",
                    TypeName = "@VarType",
                    Parameters =
                        [
                            new CustomParameterXml{Name="myParam", StringValue="@VarParam"}
                        ]
                }
            }
        };

        var serializer = new XmlSerializer(root.GetType());
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, root);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<local-variable"));
            Assert.That(content, Does.Contain("name=\"season\""));
            Assert.That(content, Does.Contain("type=\"text\""));
            Assert.That(content, Does.Not.Contain("loop"));
            Assert.That(content, Does.Contain("<custom"));
            Assert.That(content, Does.Contain("assembly-path=\"AssemblyPath\\myAssembly.dll\""));
            Assert.That(content, Does.Contain("type=\"@VarType\""));
            Assert.That(content, Does.Contain("<parameter name=\"myParam\">"));
            Assert.That(content, Does.Contain("@VarParam"));
        }
    }
}
