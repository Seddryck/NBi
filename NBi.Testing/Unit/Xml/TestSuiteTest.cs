using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class TestSuiteTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            TestSuiteXml ts;

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TestSuiteSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                var manager = new XmlManager();
                // Use the Deserialize method to restore the object's state.
                ts = manager.Read(reader);
            }
            return ts;
        }
        
        [Test]
        public void Deserialize_SampleFile_TestSuiteLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Name, Is.EqualTo("The TestSuite"));
        }
  

        [Test]
        public void Deserialize_SampleFile_TestsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests.Count, Is.GreaterThan(2));
        }
        [Test]
        public void Deserialize_SampleFile_TestMembersLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Name, Is.EqualTo("My first test case"));
            Assert.That(ts.Tests[0].UniqueIdentifier, Is.EqualTo("0001"));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Constraints.Count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintSyntacticallyCorrectLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            ts.Tests.GetRange(0,2).ForEach(t => Assert.That(t.Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>()));

        }

        [Test]
        public void Deserialize_SampleFile_ConstraintFasterThanLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Constraints[1], Is.InstanceOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).MaxTimeMilliSeconds, Is.EqualTo(5000));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintEqualToLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[2].Constraints[0], Is.InstanceOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[2].Constraints[0]).ResultSetFile, Is.Not.Empty);
        }
        
        [Test]
        public void Deserialize_SampleFile_TestCaseMembersLoaded()
        {
            //Create the queryfile to read
            DiskOnFile.CreatePhysicalFile("Select all products.sql", "NBi.Testing.Unit.Xml.Resources.SelectAllProducts.sql");
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Systems[0], Is.InstanceOf<QueryXml>());
            Assert.That(((QueryXml)ts.Tests[1].Systems[0]).GetQuery(), Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(((QueryXml)ts.Tests[1].Systems[0]).InlineQuery, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(((QueryXml)ts.Tests[1].Systems[0]).Filename, Is.Null);
            Assert.That(((QueryXml)ts.Tests[1].Systems[1]).GetQuery(), Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(((QueryXml)ts.Tests[1].Systems[1]).InlineQuery, Is.Null);
            Assert.That(((QueryXml)ts.Tests[1].Systems[1]).Filename, Is.Not.Null.And.Not.Empty);
            
            Assert.That(((QueryXml)ts.Tests[2].Systems[0]).ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Deserialize_SampleFile_TestCategoriesLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Categories.Count, Is.EqualTo(2));
            Assert.That(ts.Tests[1].Categories, Has.Member("Category 1"));
            Assert.That(ts.Tests[1].Categories, Has.Member("Category 2"));
        }

        [Test]
        public void Deserialize_SampleFile_EqualToWithQuery()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[3].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[3].Constraints[0]).ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(((EqualToXml)ts.Tests[3].Constraints[0]).QueryFile, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Deserialize_SampleFile_CountExactly()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[4].Constraints[0], Is.TypeOf<CountXml>());
            Assert.That(((CountXml)ts.Tests[4].Constraints[0]).Exactly, Is.EqualTo(10));
            Assert.That(((CountXml)ts.Tests[4].Constraints[0]).Specification.IsExactlySpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[4].Constraints[0]).Specification.IsLessThanSpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[4].Constraints[0]).Specification.IsMoreThanSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_CountMoreAndLess()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[4].Constraints[0], Is.TypeOf<CountXml>());
            Assert.That(((CountXml)ts.Tests[5].Constraints[0]).MoreThan, Is.EqualTo(10));
            Assert.That(((CountXml)ts.Tests[5].Constraints[0]).LessThan, Is.EqualTo(15));
            Assert.That(((CountXml)ts.Tests[5].Constraints[0]).Specification.IsExactlySpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[5].Constraints[0]).Specification.IsLessThanSpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[5].Constraints[0]).Specification.IsMoreThanSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_MembersWithLevel()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[4].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[4].Systems[0]).Path, Is.EqualTo("[dimension].[hierarchy].[level]"));
            Assert.That(((MembersXml)ts.Tests[4].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((MembersXml)ts.Tests[4].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_MembersWithHierarchyAndFunction()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[5].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[5].Systems[0]).Path, Is.EqualTo("[dimension].[hierarchy]"));
            Assert.That(((MembersXml)ts.Tests[5].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((MembersXml)ts.Tests[5].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsCaptionNotIgnoringCasImplicitely()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[6].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[6].Constraints[0]).Caption, Is.EqualTo("xyz"));
            Assert.That(((ContainsXml)ts.Tests[6].Constraints[0]).IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsCaptionIgnoringCaseExplicitely()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((ContainsXml)ts.Tests[7].Constraints[0]).Caption.ToLower(), Is.EqualTo("xyz"));
            Assert.That(((ContainsXml)ts.Tests[7].Constraints[0]).IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_StructureContainsCaptionNotIgnoringCaseExplicitely()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[8].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[8].Systems[0]).Path, Is.EqualTo("[dimension].[hierarchy]"));
            Assert.That(((StructureXml)ts.Tests[8].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((StructureXml)ts.Tests[8].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_StructureWithoutPathButWithMeasureGroup()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[9].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[9].Systems[0]).MeasureGroup, Is.EqualTo("MeasureGroupName"));
            Assert.That(((StructureXml)ts.Tests[9].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((StructureXml)ts.Tests[9].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithoutDisplayFolder()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[9].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[9].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithDisplayFolder()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[10].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[10].Constraints[0]).DisplayFolder, Is.EqualTo("aBc"));
            Assert.That(((ContainsXml)ts.Tests[11].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithDisplayFolderRoot()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[11].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[11].Constraints[0]).DisplayFolder, Is.EqualTo(""));
            Assert.That(((ContainsXml)ts.Tests[11].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_MembersWithChildrenOf()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[12].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[12].Systems[0]).Path, Is.EqualTo("[dimension].[hierarchy].[level]"));
            Assert.That(((MembersXml)ts.Tests[12].Systems[0]).ChildrenOf, Is.EqualTo("aBc"));
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalNothingSpecified()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[13].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[13].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Alphabetical));
            Assert.That(((OrderedXml)ts.Tests[13].Constraints[0]).Descending, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalDescendingSpecified()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[14].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[14].Constraints[0]).Descending, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintChronologicalSpecified()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[15].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[15].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Chronological));
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalSpecificSpecified()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[16].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[16].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Specific));
            Assert.That(((OrderedXml)ts.Tests[16].Constraints[0]).Definition, Has.Count.EqualTo(3));
            Assert.That(((OrderedXml)ts.Tests[16].Constraints[0]).Definition[0], Is.EqualTo("Leopold"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsNotAttributeCorrectlyRead()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[17].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[17].Constraints[0]).Not, Is.EqualTo(true));
        }
    }
}
