using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Condition;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Decoration;

[TestFixture]
public class ConditionXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_Check()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition, Is.TypeOf<ConditionXml>());
    }


    [Test]
    public void Deserialize_SampleFile_SetupCountCommands()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_SampleFile_RunningService()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates[0], Is.TypeOf<ServiceRunningConditionXml>());
        var check = (ServiceRunningConditionXml)ts.Tests[testNr].Condition.Predicates[0];
        Assert.That(check.TimeOut, Is.EqualTo("5000")); //Default value
        Assert.That(check.ServiceName, Is.EqualTo("MyService")); 

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates[1], Is.TypeOf<ServiceRunningConditionXml>());
        var check2 = (ServiceRunningConditionXml)ts.Tests[testNr].Condition.Predicates[1];
        Assert.That(check2.TimeOut, Is.EqualTo("1000")); //Value Specified
        Assert.That(check2.ServiceName, Is.EqualTo("MyService2")); 
    }

    [Test]
    public void Deserialize_SampleFile_Custom()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates[0], Is.TypeOf<CustomConditionXml>());
        var condition = (CustomConditionXml)ts.Tests[testNr].Condition.Predicates[0];
        Assert.That(condition.AssemblyPath, Is.EqualTo("myAssembly.dll"));
        Assert.That(condition.TypeName, Is.EqualTo("myType"));
        Assert.That(condition.Parameters, Has.Count.EqualTo(2));
        Assert.That(condition.Parameters[0].Name, Is.EqualTo("firstParam"));
        Assert.That(condition.Parameters[0].StringValue, Is.EqualTo("2012-10-10"));
        Assert.That(condition.Parameters[1].Name, Is.EqualTo("secondParam"));
        Assert.That(condition.Parameters[1].StringValue, Is.EqualTo("102"));
    }

    [Test]
    public void Serialize_Custom_Correct()
    {
        var root = new ConditionXml()
        {
            Predicates =
            [
                new CustomConditionXml()
                {
                    AssemblyPath = "myAssembly.dll",
                    TypeName = "myType",
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<custom "));
        Assert.That(xml, Does.Contain("assembly-path=\"myAssembly.dll\""));
        Assert.That(xml, Does.Contain("type=\"myType\""));
        Assert.That(xml, Does.Not.Contain("<parameter"));
    }

    [Test]
    public void Serialize_CustomWithParameters_Correct()
    {
        var root = new ConditionXml()
        {
            Predicates =
            [
                new CustomConditionXml()
                {
                    AssemblyPath = "myAssembly.dll",
                    TypeName = "myType",
                    Parameters =
                    [
                        new CustomConditionParameterXml() { Name="firstParam", StringValue="myValue" }
                    ]
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<parameter name=\"firstParam\">myValue</parameter>"));
    }

    [Test]
    public void Deserialize_SampleFile_FolderExists()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates[0], Is.TypeOf<FolderExistsConditionXml>());
        var condition = (FolderExistsConditionXml)ts.Tests[testNr].Condition.Predicates[0];
        Assert.That(condition.Path, Is.EqualTo(@"..\"));
        Assert.That(condition.Name, Is.EqualTo("MyFolder"));
        Assert.That(condition.NotEmpty, Is.False);
    }

    [Test]
    public void Serialize_FolderExists_Correct()
    {
        var root = new ConditionXml()
        {
            Predicates =
            [
                new FolderExistsConditionXml()
                {
                    Path = ".",
                    Name = "myFolderName",
                    NotEmpty = false
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<folder-exists "));
        Assert.That(xml, Does.Contain("path=\".\""));
        Assert.That(xml, Does.Contain("name=\"myFolderName\""));
        Assert.That(xml, Does.Not.Contain("not-empty"));
    }

    [Test]
    public void Deserialize_SampleFile_FileExists()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Condition.Predicates[0], Is.TypeOf<FileExistsConditionXml>());
        var condition = (FileExistsConditionXml)ts.Tests[testNr].Condition.Predicates[0];
        Assert.That(condition.Path, Is.EqualTo(@"..\"));
        Assert.That(condition.Name, Is.EqualTo("MyFile.txt"));
        Assert.That(condition.NotEmpty, Is.True);
    }

    [Test]
    public void Serialize_FileExists_Correct()
    {
        var root = new ConditionXml()
        {
            Predicates =
            [
                new FileExistsConditionXml()
                {
                    Path = "Folder\\",
                    Name = "myFileName.txt",
                    NotEmpty = true
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<file-exists "));
        Assert.That(xml, Does.Contain("path=\"Folder\\\""));
        Assert.That(xml, Does.Contain("name=\"myFileName.txt\""));
        Assert.That(xml, Does.Contain("not-empty=\"true\""));
    }
}
