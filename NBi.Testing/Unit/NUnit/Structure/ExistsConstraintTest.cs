using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ExistsConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoveryRequest_FactoryCalledOnceWithParametersComingFromRequest()
        {
            var request = new DiscoveryRequestFactory().Build(
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

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryMock = new Mock<AdomdDiscoveryCommandFactory>();
            factoryMock.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryMock.Object;

            var ctr = new ExistsConstraint() { CommandFactory = factory };

            //Method under test
            ctr.Matches(request);

            //Test conclusion            
            factoryMock.Verify(f => f.BuildExact(request), Times.Once());
        }

        [Test]
        public void Matches_GivenDiscoveryRequest_CommandCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var request = new DiscoveryRequestFactory().Build(
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

            var commandMock = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandMock.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandMock.Object);
            var factory = factoryStub.Object;

            var ctr = new ExistsConstraint() { CommandFactory = factory };

            //Method under test
            ctr.Matches(request);

            //Test conclusion            
            commandMock.Verify(c => c.Execute(), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForDimension_TextContainsFewKeyInfo()
        {
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null,
                        "expected-dimension-caption", null, null);

            var elements = new List<IField>();

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var existsConstraint = new ExistsConstraint() { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, existsConstraint);
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
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective-name",
                        "expected-measure-group-caption", null,
                        null, null, null);


            var elements = new List<IField>();

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var existsConstraint = new ExistsConstraint() { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, existsConstraint);
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
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Perspectives,
                        "expected-perspective-name",
                        null, null,
                        null, null, null);


            var elements = new List<IField>();

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var existsConstraint = new ExistsConstraint() { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, existsConstraint);
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
