using System.IO;
#region Using directives

using NBi.Xml;
using System.Reflection;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Xml.Settings;
using System;
#endregion

namespace NBi.Xml.Testing.Unit.Settings;

[TestFixture]
public class SettingsXmlTest
{

    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
    public void TearDownMethods()
    {
    }

    //Called before each test
    [SetUp]
    public void SetupTest()
    {
    }

    //Called after each test
    [TearDown]
    public void TearDownTest()
    {
    }
    #endregion

    protected TestSuiteXml DeserializeSample(string filename)
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.{filename}.xml")
                                       ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        manager.ApplyDefaultSettings();
        return manager.TestSuite!;
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultLoaded()
    {           
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithDefault");

        Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(3)); //(One empty and one initialized)
        var sutDefault = ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest);
        Assert.That(sutDefault.ConnectionStringSpecified, Is.True);
        Assert.That(sutDefault.ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(sutDefault.Parameters, Is.Not.Null);

        var varDefault = ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest);
        Assert.That(varDefault.ConnectionStringSpecified, Is.True);
        Assert.That(varDefault.ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(varDefault.Parameters, Is.Not.Null);
    }

    [Test]
    public void Deserialize_SettingsWithoutTagParallelizeQueries_ParallelizeQueriesIsTrue()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithDefault");

        Assert.That(ts.Settings.ParallelizeQueries, Is.True);
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultForwardedToItem()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithDefault");

        var system = (ExecutionXml)ts.Tests[testNr].Systems[0];
        Assert.That(system.Item.ConnectionString, Is.Null.Or.Empty);
        Assert.That(system.Settings, Is.Not.Null);
        Assert.That(system.Settings!.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(system.Item.Settings, Is.Not.Null);
        Assert.That(system.Item.Settings!.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(system.Item.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline, Is.EqualTo(system.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithDefault_RoleIsDeserialized()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithDefault");

        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.Null.Or.Empty);
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.Roles, Is.EqualTo("admin"));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithDefaultForAssert_DefaultReplicatedForTest()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithDefaultAssert");

        var assert = (EqualToXml)ts.Tests[testNr].Constraints[0];
        Assert.That(assert.BaseItem!.ConnectionString, Is.Null.Or.Empty);
        Assert.That(assert.Settings, Is.Not.Null);
        Assert.That(assert.Settings!.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(assert.BaseItem.Settings, Is.Not.Null);
        Assert.That(assert.BaseItem.Settings!.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(assert.BaseItem.Settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.Inline, Is.EqualTo(assert.Settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.Inline));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithoutDefault_NoDefaultLoadedButAreAutomaticallyCreated()
    {
        //int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithoutDefault");

        Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(2));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultReplicatedForTest()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithoutDefault");

        Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).ConnectionString, Is.Null.Or.Empty);
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultEqualToEmptyDefaultAndNotNull()
    {
        //int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithoutDefault");

        Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest), Is.Not.Null);
        Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionStringSpecified, Is.False);
        Assert.That(ts.Settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).Parameters, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceLoaded()
    {
        //int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithReference");

        Assert.That(ts.Settings.References.Count, Is.EqualTo(2));
        Assert.That(ts.Settings.GetReference("first-ref"), Is.Not.Null);
        Assert.That(ts.Settings.GetReference("first-ref").ConnectionString.Inline, Is.EqualTo("My First Connection String"));
        Assert.That(ts.Settings.GetReference("second-ref"), Is.Not.Null);
        Assert.That(ts.Settings.GetReference("second-ref").ConnectionString.Inline, Is.EqualTo("My Second Connection String"));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithReferenceNotExisting_ThrowArgumentException()
    {
        //int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithReference");

        Assert.Throws<System.ArgumentOutOfRangeException>(delegate { ts.Settings.GetReference("not-existing"); });
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceAppliedToTest()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithReference");

        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.Not.Null.And.Not.Empty);
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.EqualTo("@second-ref"));
        var system = (ExecutionXml)ts.Tests[testNr].Systems[0];
        Assert.That(system.Item.ConnectionString, Is.Not.Null.And.Not.Empty);
        Assert.That(system.Item.ConnectionString, Does.StartWith("@"));
        Assert.That(system.Item.ConnectionString, Is.EqualTo("@second-ref"));
        Assert.That(system.Settings, Is.Not.Null);
        Assert.That(system.Settings!.GetReference("second-ref").ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(system.Item.Settings, Is.Not.Null);
        Assert.That(system.Item.Settings!.GetReference("second-ref").ConnectionString.Inline, Is.Not.Null.And.Not.Empty);
        Assert.That(system.Item.Settings.GetReference("second-ref").ConnectionString.Inline, Is.EqualTo(system.Settings.GetReference("second-ref").ConnectionString.Inline));
    }

    [Test]
    public void DeserializeEqualToResultSet_SettingsWithReferenceButMissingconnStrRef_NullAppliedToTest()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithReference");
        Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).ConnectionString, Is.Null.Or.Empty);
        Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.Null.Or.Empty);
    }


    [Test]
    public void DeserializeStructurePerspective_SettingsWithoutParallelizeQueries_False()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithoutParallelQueries");

        Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.True);
    }

    [Test]
    public void DeserializeStructurePerspective_SettingsWithParallelizeQueriesSetFalse_False()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithParallelQueriesSetFalse");

        Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.False);
    }

    [Test]
    public void DeserializeStructurePerspective_SettingsWithParallelizeQueriesSetTrue_True()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithParallelQueriesSetTrue");

        Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ParallelizeQueries, Is.True);
    }

    [Test]
    public void DeserializeStructurePerspective_SettingsWithParameters_True()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithParameters");

        var parameters =((QueryXml)ts.Tests[testNr].Systems[0].BaseItem!).GetParameters();
        Assert.That(parameters.Count, Is.EqualTo(3));
    }

    [Test]
    public void DeserializeStructurePerspective_SettingsWithVariables_True()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("SettingsXmlWithVariables");

        var parameters = ((QueryXml)ts.Tests[testNr].Systems[0].BaseItem!).GetTemplateVariables();
        Assert.That(parameters.Count, Is.EqualTo(3));
    }
}
