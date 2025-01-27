using System;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class XmlManagerTest
{
    [Test]
    public void Load_ValidFile_Success()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuite.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerSample.xml");
        
        var manager = new XmlManager();
        manager.Load(filename);

        Assert.That(manager.TestSuite, Is.Not.Null);
    }

    [Test]
    public void Load_ValidFile_TestContentIsCorrect()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestContentIsCorrect.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerSample.xml");

        var manager = new XmlManager();
        manager.Load(filename);

        Assert.That(manager.TestSuite!.Tests[0].Content, Is.Not.Null);
        Assert.That(manager.TestSuite.Tests[0].Content, Does.EndWith("</test>"));
    }

    [Test]
    public void Load_InvalidFormat_ThrowException()
    {
        var filename = FileOnDisk.CreatePhysicalFile("InvalidFormat.nbits", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerInvalidFormat.xml");

        var manager = new XmlManager();
        var ex = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        Assert.That(ex!.Message, Does.Contain("At line 14"));
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public void Load_InvalidFile_ThrowException()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuiteInvalidSyntax.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerInvalidSyntax.xml");

        var manager = new XmlManager();
        Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
    }

    [Test]
    public void Load_InvalidFile_ExceptionHasCorrectInformation()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuiteInvalidSyntax.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerInvalidSyntax.xml");

        var manager = new XmlManager();
        var exception = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        Assert.That(exception!.Message, Does.Contain("1 error has been found during the validation of the test-suite"));
        Assert.That(exception.Message, Does.Contain("\tAt line 4: The element 'test' in namespace 'http://NBi/TestSuite' has invalid child element 'syntacticallyCorrect' in namespace 'http://NBi/TestSuite'."));
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public void Load_InvalidMultipleFile_ThrowException()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuiteInvalidSyntaxMultiple.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerInvalidSyntaxMultiple.xml");

        var manager = new XmlManager();
        Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
    }

    [Test]
    public void Load_InvalidMultipleFile_ExceptionHasCorrectInformation()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuiteInvalidSyntaxMultiple.xml", $"{GetType().Assembly.GetName().Name}.Resources.XmlManagerInvalidSyntaxMultiple.xml");

        var manager = new XmlManager();
        var exception = Assert.Throws<ArgumentException>(delegate { manager.Load(filename); });
        Assert.That(exception!.Message, Does.Contain("2 errors have been found during the validation of the test-suite"));
        Assert.That(exception.Message, Does.Contain("At line 6: The element 'execution' in namespace 'http://NBi/TestSuite' has invalid child element 'sql' in namespace 'http://NBi/TestSuite'."));
        Assert.That(exception.Message, Does.Contain("At line 11: The 'name' attribute is not declared."));
    }
}
