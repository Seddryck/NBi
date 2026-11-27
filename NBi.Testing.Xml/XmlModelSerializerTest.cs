using System.Xml.Serialization;
using NBi.Model;
using RowCount = NBi.Model.Constraints.RowCount;
using NBi.Model.Constraints.Comparer;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Systems;
using NUnit.Framework;

namespace NBi.Model.Xml.Testing;

public class XmlModelSerializerTest
{
    [Test]
    public void ModelDoesNotReferenceXmlSerialization()
    {
        var assembly = typeof(TestSuite).Assembly;

        Assert.That(assembly.GetReferencedAssemblies().Select(reference => reference.Name),
            Has.None.Matches<string>(name => name.StartsWith("System.Xml") || name == "NBi.Model.Xml" || name == "NBi.Core" || name == "NBi.Core"));

        var xmlAttributes = assembly.GetTypes().SelectMany(type =>
            type.CustomAttributes.Concat(type.GetMembers().SelectMany(member => member.CustomAttributes)))
            .Where(attribute => attribute.AttributeType.Namespace == typeof(XmlElementAttribute).Namespace);
        Assert.That(xmlAttributes, Is.Empty);
    }

    [Test]
    public void StandaloneTest_RoundTripsPolymorphicItemsAndDrafts()
    {
        var model = new StandaloneTest
        {
            Name = "Report validation",
            Systems = [new Execution { Item = new Query { InlineQuery = "select '<&>'" } }],
            Constraints = [new RowCount { Equal = new Equal { Reference = "3" } }],
            Drafts = ["<draft custom='value'><note>details</note></draft>"],
        };

        var xml = XmlModelSerializer.Serialize(model);
        var restored = XmlModelSerializer.Deserialize<StandaloneTest>(xml);

        Assert.Multiple(() =>
        {
            Assert.That(xml, Does.Contain("<![CDATA[select '<&>']]>"));
            Assert.That(((Query)((Execution)restored.Systems.Single()).Item).InlineQuery, Is.EqualTo("select '<&>'"));
            Assert.That(((RowCount)restored.Constraints.Single()).Equal!.Reference, Is.EqualTo("3"));
            Assert.That(restored.Drafts.Single(), Does.Contain("<note>details</note>"));
        });
    }

    [Test]
    public void ContractTypes_PreserveTheirDefaultModelState()
    {
        var modelTypes = typeof(TestSuite).Assembly.GetExportedTypes().Where(type =>
            type.Namespace?.StartsWith("NBi.Model", StringComparison.Ordinal) == true
            && type.Namespace?.StartsWith("NBi.Model", StringComparison.Ordinal) == true
            && type.IsClass && !type.IsAbstract && type != typeof(NBi.Model.TestSuiteBuilder) && type.GetConstructor(Type.EmptyTypes) != null);

        foreach (var modelType in modelTypes)
        {
            var model = Activator.CreateInstance(modelType)!;
            var contract = Mapping.ModelMapper.ToContract(model, modelType);
            var restored = Mapping.ModelMapper.ToModel(contract, modelType);

            Assert.That(contract.GetType().Assembly, Is.SameAs(typeof(XmlManager).Assembly), modelType.FullName);
            Assert.That(restored, Is.TypeOf(modelType), modelType.FullName);
        }
    }

    [Test]
    public void Comparison_RoundTripsSharedEnumAliases()
    {
        var test = new StandaloneTest
        {
            Constraints = [new Model.Constraints.EqualTo
            {
                KeysDef = Model.Constraints.KeySelection.AllExceptLast,
                ValuesDef = Model.Constraints.ValueSelection.Last,
                ValuesDefaultType = Core.ResultSet.ColumnType.DateTime,
            }],
        };

        var xml = XmlModelSerializer.Serialize(test);
        var restored = (Model.Constraints.EqualTo)XmlModelSerializer.Deserialize<StandaloneTest>(xml).Constraints.Single();

        Assert.Multiple(() =>
        {
            Assert.That(xml, Does.Contain("all-except-last").And.Contain("dateTime"));
            Assert.That(restored.KeysDef, Is.EqualTo(Model.Constraints.KeySelection.AllExceptLast));
            Assert.That(restored.ValuesDef, Is.EqualTo(Model.Constraints.ValueSelection.Last));
            Assert.That(restored.ValuesDefaultType, Is.EqualTo(Core.ResultSet.ColumnType.DateTime));
        });
    }

    [Test]
    public void LegacyConstraintName_MapsToTheCurrentModelType()
    {
        const string xml = """
            <test name="Legacy">
              <assert><subsetOf><item>abc</item></subsetOf></assert>
            </test>
            """;

        var model = XmlModelSerializer.Deserialize<StandaloneTest>(xml);

        Assert.That(model.Constraints.Single(), Is.TypeOf<NBi.Model.Constraints.ContainedIn>());
        Assert.That(XmlModelSerializer.Serialize(model), Does.Contain("<contained-in>").And.Not.Contain("<subsetOf>"));
    }

    [Test]
    public void Suite_PersistAndLoadModelPreserveSettingsAndReferences()
    {
        var filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.nbits");
        try
        {
            var settings = new SuiteSettings
            {
                CsvProfile = new CsvProfile('\t', '\'', "\n") { FirstRowHeader = true },
            };
            var query = new Query { InlineQuery = "select 1" };
            var model = new TestSuite
            {
                Name = "Suite",
                Settings = settings,
                Tests = [new TestDefinition { Name = "Count check", Systems = [new Execution { Item = query }], Constraints = [new RowCount { Equal = new Equal { Reference = "1" } }] }],
            };
            var writer = new XmlManager();
            writer.Persist(filename, model);

            var restored = new XmlManager().LoadModel(filename);
            var execution = (Execution)restored.Tests.Single().Systems.Single();

            Assert.Multiple(() =>
            {
                Assert.That(restored.Settings.CsvProfile.FieldSeparator, Is.EqualTo('\t'));
                Assert.That(restored.Settings.CsvProfile.TextQualifier, Is.EqualTo('\''));
                Assert.That(restored.Settings.CsvProfile.RecordSeparator, Is.EqualTo("\n"));
                Assert.That(execution.Settings, Is.SameAs(restored.Settings));
                Assert.That(restored.Tests.Single().Content, Does.Contain("Count check"));
            });
        }
        finally
        {
            File.Delete(filename);
        }
    }
}
