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
    public class ContainConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoveryRequest_FactoryCalledOnceWithParametersComingFromRequest()
        {
            var exp = "Expected hierarchy";
            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("dimension-caption", DiscoveryTarget.Dimensions)
                        });

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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

            //Method under test
            containsConstraint.Matches(request);

            //Test conclusion            
            factoryMock.Verify(f => f.BuildExact(request), Times.Once());
        }

        [Test]
        public void Matches_GivenDiscoveryRequest_CommandCalledOnceWithParametersComingFromDiscoveryCommand()
        {
            var exp = "Expected measure";
            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("measure-group", DiscoveryTarget.MeasureGroups)
                        });


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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

            //Method under test
            containsConstraint.Matches(request);

            //Test conclusion            
            commandMock.Verify(c => c.Execute(), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertionForOneDimension_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.Dimensions,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("dimension-caption", DiscoveryTarget.Dimensions)
                        });


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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

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
        public void WriteTo_FailingAssertionForOneMeasureGroup_TextContainsFewKeyInfo()
        {
            var exp = "Expected measure";
            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.MeasureGroups,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("measure-group-caption", DiscoveryTarget.MeasureGroups)
                        });


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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

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
        public void WriteTo_FailingAssertionForOneHierarchy_TextContainsFewKeyInfo()
        {
            var exp = "Expected level";
            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.Hierarchies,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("dimension-caption", DiscoveryTarget.Dimensions)
                                , new CaptionFilter("hierarchy-caption", DiscoveryTarget.Hierarchies)
                        });


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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

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

        [Test]
        public void WriteTo_FailingAssertionForMultipleHierarchies_TextContainsFewKeyInfo()
        {
            var exp = new List<string>();
            exp.Add("Expected h1");
            exp.Add("Expected h2");

            var request = new DiscoveryRequestFactory().BuildDirect(
                        "connectionString",
                        DiscoveryTarget.Hierarchies,
                        new List<IFilter>()
                            {
                                new CaptionFilter("perspective-name", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("dimension-caption", DiscoveryTarget.Dimensions)
                        });


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

            var containsConstraint = new ContainConstraint(exp) { CommandFactory = factory };

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
                                            .StringContaining("hierarchies").And
                                            .Not.StringContaining("Expected h1"));
        }
      
    }
}
