using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ExistsConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoveryCommandForDimension_EngineCalledOnceWithParametersComingFromDiscoveryCommandy()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null,
                        "expected-dimension-caption", null, null);

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

            var existsConstraint = new ExistsConstraint() { MetadataExtractor = me };

            //Method under test
            existsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetPartialMetadata(cmd), Times.Once());
        }

        [Test]
        public void Matches_GivenDiscoveryCommandForMeasure_EngineCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective",
                        "measure-group", "measure",
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

            var existsConstraint = new ExistsConstraint() { MetadataExtractor = me };

            //Method under test
            existsConstraint.Matches(cmd);

            //Test conclusion            
            meMock.Verify(engine => engine.GetPartialMetadata(cmd), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForDimension_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null,
                        "expected-dimension-caption", null, null);

            var elements = new List<IField>();

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var existsConstraint = new ExistsConstraint() { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("expected-dimension-caption"));
        }

        [Test]
        public void WriteTo_FailingAssertionForMeasureGroup_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective-name",
                        "expected-measure-group-caption", null,
                        null, null, null);


            var elements = new List<IField>();

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var existsConstraint = new ExistsConstraint() { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("perspective-name").And
                                            .StringContaining("expected-measure-group-caption"));
        }

        [Test]
        public void WriteTo_FailingAssertionForPerspective_TextContainsFewKeyInfo()
        {
            var cmd = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Perspectives,
                        "expected-perspective-name",
                        null, null,
                        null, null, null);


            var elements = new List<IField>();

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(cmd))
                .Returns(elements);
            var me = meStub.Object;

            var existsConstraint = new ExistsConstraint() { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(cmd, existsConstraint);
            }
            catch (AssertionException ex)
            {
                assertionText = ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining("expected-perspective-name"));
        }


        

       
    }
}
