using NBi.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Xml.Testing;

public abstract class BaseXmlTest
{
    protected TestSuiteXml DeserializeSample()
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.{GetType().Name}Suite.xml")
                                       ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
            manager.Read(reader);
        manager.ApplyDefaultSettings();
        return manager.TestSuite!;
    }
}
