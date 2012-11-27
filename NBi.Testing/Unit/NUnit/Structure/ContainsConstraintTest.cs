using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ContainsConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoveryCommandForDimension_EngineCalledOnceWithParametersComingFromDiscoveryCommandy()
        {
            var exp = "Expected hierarchy";
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null,
                        "dimension-caption", null, null);

            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var meMock = new Mock<IMetadataExtractor>();
            meMock.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meMock.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            containsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetPartialMetadata(cmd), Times.Once());
        }

        [Test]
        public void Matches_GivenDiscoveryCommandForMeasureGroup_EngineCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var exp = "Expected measure";
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective",
                        "measure-group", null,
                        null, null, null);


            var elStub = new Mock<IFieldWithDisplayFolder>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IFieldWithDisplayFolder>();
            elements.Add(el1);
            elements.Add(el2);

            var meMock = new Mock<IMetadataExtractor>();
            meMock.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meMock.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            containsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetPartialMetadata(cmd), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForDimension_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null,
                        "dimension-caption", null, null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("Expected hierarchy"));
        }

        [Test]
        public void WriteTo_FailingAssertionForMeasureGroup_TextContainsFewKeyInfo()
        {
            var exp = "Expected measure";
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective-name",
                        "measure-group-caption", null,
                        null, null, null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("measure-group-caption").And
                                            .StringContaining("Expected measure"));
        }

        [Test]
        public void WriteTo_FailingAssertionForHierarchy_TextContainsFewKeyInfo()
        {
            var exp = "Expected level";
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Hierarchies,
                        "perspective-name",
                        null, null,
                        "dimension-caption", "hierarchy-caption", null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, containsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("dimension-caption").And
                                            .StringContaining("hierarchy-caption").And
                                            .StringContaining("Expected level"));
        }


        

       
    }
}
