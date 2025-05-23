﻿using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class TableXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_TableAndPerspectiveLoaded()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<TableXml>());

        var item = (TableXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Caption, Is.EqualTo("table"));
        Assert.That(item.Perspective, Is.EqualTo("perspective"));
        Assert.That(item.ConnectionString, Is.EqualTo("connectionString"));
    }

           
}
