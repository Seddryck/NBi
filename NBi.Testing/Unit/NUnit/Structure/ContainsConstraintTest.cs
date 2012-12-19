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
    public class ContainsConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoveryRequest_FactoryCalledOnceWithParametersComingFromRequest()
        {
            var exp = "Expected hierarchy";
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null, null,
                        "dimension-caption", null, null);

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

            var containsConstraint = new ContainsConstraint(exp) { CommandFactory = factory };

            //Method under test
            containsConstraint.Matches(request);

            //Test conclusion            
            factoryMock.Verify(f => f.BuildExact(request), Times.Once());
        }

        [Test]
        public void Matches_GivenDiscoveryRequest_CommandCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var exp = "Expected measure";
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective",
                        "measure-group", null, null,
                        null, null, null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var commandMock = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandMock.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandMock.Object);
            var factory = factoryStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { CommandFactory = factory };

            //Method under test
            containsConstraint.Matches(request);

            //Test conclusion            
            commandMock.Verify(c => c.Execute(), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForDimension_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        "perspective-name",
                        null, null, null,
                        "dimension-caption", null, null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, containsConstraint);
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
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        "perspective-name",
                        "measure-group-caption", null, null,
                        null, null, null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, containsConstraint);
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
            var request = new DiscoveryRequestFactory().Build(
                        "connectionString",
                        DiscoveryTarget.Hierarchies,
                        "perspective-name",
                        null, null, null,
                        "dimension-caption", "hierarchy-caption", null);


            var elStub = new Mock<IField>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IField>();
            elements.Add(el1);
            elements.Add(el2);

            var commandStub = new Mock<AdomdDiscoveryCommand>("connectionString");
            commandStub.Setup(f => f.Execute())
                .Returns(elements);

            var factoryStub = new Mock<AdomdDiscoveryCommandFactory>();
            factoryStub.Setup(f => f.BuildExact(request))
                .Returns(commandStub.Object);
            var factory = factoryStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { CommandFactory = factory };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(request, containsConstraint);
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
