using System.Collections.Generic;
using Moq;
using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Metadata;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ContainsConstraintTest
    {
        [Test]
        public void Matches_GivenDiscoverCommandForDimension_EngineCalledOnceWithParametersComingFromDiscoverCommandy()
        {
            var exp = "Expected hierarchy";
            var cmd = DiscoveryFactory.BuildForDimension(
                        "connectionString",
                        "perspective",
                        "[dimension]");

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
        public void Matches_GivenDiscoverCommandForMeasure_EngineCalledOnceWithParametersComingFromDiscoverCommand()
        {
            var exp = "Expected measure";
            var cmd = DiscoveryFactory.BuildForMeasureGroup(
                        "connectionString",
                        "perspective",
                        "measure-group");


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
        public void WriteTo_FailingAssertion_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var cmd = DiscoveryFactory.BuildForDimension(
                        "connectionString",
                        "perspective",
                        "[dimension]");


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
                 assertionText=ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining(((PathDiscoveryCommand)cmd).PerspectiveName).And
                                            .StringContaining(((PathDiscoveryCommand)cmd).Path).And
                                            .StringContaining("dimension").And
                                            .StringContaining("hierarchy").And
                                            .StringContaining(exp));
        }


        

       
    }
}
