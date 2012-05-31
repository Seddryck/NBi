using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
using NBi.Core.Analysis.Metadata;
using NBi.NUnit.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Structure
{
    [TestFixture]
    public class ContainsConstraintTest
    {
        [Test]
        public void Matches_GivenMetadataQuery_EngineCalledOnceWithParametersComingFromMetadataQuery()
        {
            var exp = "Expected hierarchy";
            var mq = new MetadataQuery() {Path="[dimension]", Perspective="perspective"};


            var elStub = new Mock<IElement>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IElement>();
            elements.Add(el1);
            elements.Add(el2);

            var meMock = new Mock<IMetadataExtractor>();
            meMock.Setup(engine => engine.GetPartialMetadata(mq.Path, mq.Perspective))
                .Returns(elements);
            var me = meMock.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            containsConstraint.Matches(mq);
         
            //Test conclusion            
            meMock.Verify(engine => engine.GetPartialMetadata(mq.Path, mq.Perspective), Times.Once());
        }

        [Test]
        public void WriteTo_FailingAssertion_TextContainsFewKeyInfo()
        {
            var exp = "Expected hierarchy";
            var mq = new MetadataQuery() { Path = "[dimension]", Perspective = "perspective" };


            var elStub = new Mock<IElement>();
            var el1 = elStub.Object;
            var el2 = elStub.Object;
            var elements = new List<IElement>();
            elements.Add(el1);
            elements.Add(el2);

            var meStub = new Mock<IMetadataExtractor>();
            meStub.Setup(engine => engine.GetPartialMetadata(mq.Path, mq.Perspective))
                .Returns(elements);
            var me = meStub.Object;

            var containsConstraint = new ContainsConstraint(exp) { MetadataExtractor = me };

            //Method under test
            string assertionText = null;
            try
            {
                Assert.That(mq, containsConstraint);
            }
            catch (AssertionException ex)
            {
                 assertionText=ex.Message;
            }

            //Test conclusion            
            Assert.That(assertionText, Is.StringContaining(mq.Perspective).And
                                            .StringContaining(mq.Path).And
                                            .StringContaining("dimension").And
                                            .StringContaining("hierarchy").And
                                            .StringContaining(exp));
        }


        

       
    }
}
